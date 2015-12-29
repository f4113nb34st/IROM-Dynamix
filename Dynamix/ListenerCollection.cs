namespace IROM.Dynamix
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// A simple thread-safe collection of weakly-referenced values. Does not allow duplicates or null values.
	/// Fast add and iteration. Minimalistic memory footprint.
	/// Custom-made for the listener collections of <see cref="Dynx{T}">Dynx</see>.
	/// 
	/// NOTE: Linked-list-backed because we need extremely fast add, remove, and iteration.
	/// Memory overhead of linked list is less than desirable, but worth the speed boost.
	/// </summary>
	internal struct ListenerCollection<T> : IEnumerable<T> where T : class
	{
		/// <summary>
		/// Node for the linked list.
		/// </summary>
		internal class Node
		{
			public WeakHandle<T> Handle;
			public Node Next;
			public Node Prev;
		}
		
		/// <summary>
		/// The root node. Not actually part of the collection.
		/// </summary>
		private readonly Node root;
		
		internal ListenerCollection(bool dummy)
		{
			root = new Node();
			root.Next = root;
			root.Prev = root;
		}
		
		/// <summary>
		/// Inserts the given node between the given prev and next.
		/// </summary>
		/// <param name="node">The new node.</param>
		/// <param name="prev">The previous node.</param>
		/// <param name="next">The new node.</param>
		private void Insert(Node node, Node prev, Node next)
		{
			//lock to ensure atomic
			lock(root)
			{
				node.Prev = prev;
				node.Next = next;
				prev.Next = next.Prev = node;
			}
		}
		
		/// <summary>
		/// Removes the given <see cref="Node"/> from the collection.
		/// </summary>
		/// <param name="node">The node to remove.</param>
		private void Remove(Node node)
		{
			//lock to ensure atomic
			lock(root)
			{
				node.Prev.Next = node.Next;
				node.Next.Prev = node.Prev;
			}
		}
		
		/// <summary>
		/// Adds the given value to the back of the collection.
		/// </summary>
		/// <param name="value">The value to add.</param>
		/// <param name="weak">True if the reference should be weak.</param>
		public void Add(T value, bool weak = false)
		{
			T nodeValue;
			//first search for duplicates (and remove lost references while we're at it)
			for(Node node = root.Next; node != root; node = node.Next)
			{
				nodeValue = node.Handle.Value;
				//if we find duplicate, don't re-add
				if(nodeValue == value)
					return;
				//if lost reference, remove
				if(nodeValue == null)
					Remove(node);
			}
			
			//add new node to end
			Node newNode = new Node{Handle = new WeakHandle<T>(value, weak)};
			Insert(newNode, root.Prev, root);
		}
		
		/// <summary>
		/// Adds the given handle to the front of the collection. 
		/// If it finds an entry with a handle that points to the same value, it steals the old entry for the new handle.
		/// </summary>
		/// <param name="handle">The handle to add.</param>
		/// <param name="value">The value the handle points to. We could get this from the handle itself, but it is faster to pass it.</param>
		public void Add(ref WeakHandle<T> handle, ref T value)
		{
			T nodeValue;
			//first search for duplicates (and remove lost references while we're at it)
			for(Node node = root.Next; node != root; node = node.Next)
			{
				nodeValue = node.Handle.Value;
				//if we find duplicate, steal entry for new handle
				if(nodeValue == value)
				{
					node.Handle = handle;
					return;
				}
				//if lost reference, remove
				if(nodeValue == null)
					Remove(node);
			}
			//when a parent is iterating listeners, calls child update
			//that update creates and subscribes new listener
			//add to beginning so the new listener is not called in that iteration
			Node newNode = new Node{Handle = handle};
			Insert(newNode, root, root.Next);
		}
		
		/// <summary>
		/// Removes the given value from the collection.
		/// </summary>
		/// <param name="value">The value to remove.</param>
		public void Remove(T value)
		{
			for(Node node = root.Next; node != root; node = node.Next)
			{
				T nodeValue = node.Handle.Value;
				//if value to remove
				if(nodeValue == value)
				{
					Remove(node);
					return;
				}
				//if lost reference
				if(nodeValue == null)
					Remove(node);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new ListenerEnumerator{root = root, node = root};
		}
		
		internal struct ListenerEnumerator : IEnumerator<T>
		{
			public Node root;
			public Node node;
			private T nodeValue;
			
			public T Current {get{return nodeValue;}}
			
			public bool MoveNext()
			{
				while(true)
				{
					node = node.Next;
					if(node == root) return false;
					nodeValue = node.Handle.Value;
					if(nodeValue != null)
					{
						return true;
					}else
					{
						//remove node
						//lock to ensure atomic
						lock(root)
						{
							node.Prev.Next = node.Next;
							node.Next.Prev = node.Prev;
						}
					}
				}
			}
			
			public void Reset(){throw new NotImplementedException();}
			public void Dispose(){}
			object System.Collections.IEnumerator.Current{get{return Current;}}
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){return GetEnumerator();}
	}
}
