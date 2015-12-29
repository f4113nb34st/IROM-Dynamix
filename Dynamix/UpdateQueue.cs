namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// A simple collection of values. Allows duplicates. Not thread-safe.
	/// Fast add, remove, and iteration. Minimalistic memory footprint.
	/// First in, First out access.
	/// Custom-made for the thread-specific update queues.
	/// 
	/// NOTE: Linked-list-of-array-backed because we need extremely fast push and pop of a linked list, 
	/// with the reduced garbage production of arrays.
	/// </summary>
	internal sealed class UpdateQueue<T> where T : class
	{
		/// <summary>
		/// Node for the linked list.
		/// </summary>
		private class Node
		{
			public T[] Buffer;
			public int Size;
			public Node Next;
			public Node Prev;
		}
		
		/// <summary>
		/// The size of each created node.
		/// </summary>
		public readonly int nodeSize;
		
		/// <summary>
		/// True if this UpdateQueue is currently being managed for updates.
		/// </summary>
		public bool masterInProgress = false;
		
		/// <summary>
		/// The root node. Not actually in the collection.
		/// </summary>
		private readonly Node root;
		
		public UpdateQueue()
		{
			root = new Node();
			root.Next = root.Prev = root;
			//default to 256 node size
			nodeSize = 256;
		}
		
		/// <summary>
		/// Inserts the given node between the given prev and next.
		/// </summary>
		/// <param name="node">The new node.</param>
		/// <param name="prev">The previous node.</param>
		/// <param name="next">The new node.</param>
		private void Insert(Node node, Node prev, Node next)
		{
			node.Prev = prev;
			node.Next = next;
			prev.Next = next.Prev = node;
		}
		
		/// <summary>
		/// Removes the given <see cref="Node"/> from the collection.
		/// </summary>
		/// <param name="node">The node to remove.</param>
		private void Remove(Node node)
		{
			node.Prev.Next = node.Next;
			node.Next.Prev = node.Prev;
		}
		
		/// <summary>
		/// Puts the given value into the queue.
		/// </summary>
		/// <param name="value">The value to add.</param>
		public void Offer(T value)
		{
			Node node = root.Prev;
			if(node == root || node.Size == node.Buffer.Length)
			{
				node = new Node();
				node.Buffer = new T[nodeSize];
				node.Size = 0;
				//insert at end
				Insert(node, root.Prev, root);
			}
			//add to buffer and increase size
			node.Buffer[node.Size] = value;
			node.Size++;
		}
		
		/// <summary>
		/// Removes and returns the first value from the queue.
		/// Returns null if the queue is empty.
		/// </summary>
		public T Poll()
		{
			Node node = root.Next;
			if(node.Size == 0)
			{
				if(node.Prev == root) return null;
				Remove(node);
				node = node.Prev;
			}
			node.Size--;
			return node.Buffer[node.Size];
		}
	}
}
