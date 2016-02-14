namespace IROM.Dynamix
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// A simple thread-unsafe collection of weakly-referenced values. Does not allow duplicates or null values.
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
		}
		
		/// <summary>
		/// The root node.
		/// </summary>
		private Node root;
		
		/// <summary>
		/// Adds the given value to the back of the collection.
		/// </summary>
		/// <param name="value">The value to add.</param>
		/// <param name="weak">True if the reference should be weak.</param>
		public void Add(T value, bool weak = false)
		{
			Node node = root;
			Node prev = null;
			T nodeValue;
			//first search for duplicates (and remove lost references while we're at it)
			while(node != null)
			{
				nodeValue = node.Handle.Value;
				//if we find duplicate, don't re-add
				if(nodeValue == value)
					return;
				//if lost reference, remove
				if(nodeValue == null)
				{
					//move next
					node = node.Next;
					//if start
					if(prev == null)
					{
						//update root
						root = node;
					}else
					{
						//update prev next
						prev.Next = node;
					}
					//skip move next
					continue;
				}
				//move to next node
				prev = node;
				node = node.Next;
			}
			
			//add new node to end
			Node newNode = new Node{Handle = new WeakHandle<T>(value, weak)};
			if(prev != null)
			{
				prev.Next = newNode;
			}else
			{
				root = newNode;
			}
		}
		
		/// <summary>
		/// Adds the given handle to the front of the collection. 
		/// If it finds an entry with a handle that points to the same value, it steals the old entry for the new handle.
		/// </summary>
		/// <param name="handle">The handle to add.</param>
		/// <param name="value">The value the handle points to. We could get this from the handle itself, but it is faster to pass it.</param>
		public void Add(WeakHandle<T> handle, T value)
		{
			Node node = root;
			Node prev = null;
			T nodeValue;
			//first search for duplicates (and remove lost references while we're at it)
			while(node != null)
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
				{
					//move next
					node = node.Next;
					//if start
					if(prev == null)
					{
						//update root
						root = node;
					}else
					{
						//update prev next
						prev.Next = node;
					}
					//skip move next
					continue;
				}
				//move to next node
				prev = node;
				node = node.Next;
			}
			//when a parent is iterating listeners, calls child update
			//that update creates and subscribes new listener
			//add to beginning so the new listener is not called in that iteration
			Node newNode = new Node{Handle = handle};
			newNode.Next = root;
			root = newNode;
		}
		
		/// <summary>
		/// Removes the given value from the collection.
		/// </summary>
		/// <param name="value">The value to remove.</param>
		public void Remove(T value)
		{
			Node node = root;
			Node prev = null;
			T nodeValue;
			while(node != null)
			{
				nodeValue = node.Handle.Value;
				//if value to remove or lost reference
				if(nodeValue == value || nodeValue == null)
				{
					//move next
					node = node.Next;
					//if start
					if(prev == null)
					{
						//update root
						root = node;
					}else
					{
						//update prev next
						prev.Next = node;
					}
					//if lost reference continue, else done
					if(nodeValue == null) continue;
					else return;
				}
				//move to next node
				prev = node;
				node = node.Next;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new ListenerEnumerator{node = root};
		}
		
		internal struct ListenerEnumerator : IEnumerator<T>
		{
			public Node node;
			private T nodeValue;
			
			public T Current {get{return nodeValue;}}
			
			public bool MoveNext()
			{
				Node prev = node;
				while(true)
				{
					if(node == null) return false;
					nodeValue = node.Handle.Value;
					if(nodeValue != null)
					{
						//move for next time
						node = node.Next;
						return true;
					}else
					{
						//remove node
						prev.Next = node = node.Next;
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
