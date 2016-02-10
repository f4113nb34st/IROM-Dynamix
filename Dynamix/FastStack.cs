namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// A simple collection of values. Allows duplicates.
	/// Fast add and iteration. Minimalistic memory footprint.
	/// First in, Last out access.
	/// Custom-made for the thread-specific child stacks of <see cref="Dynx{T}">Dynx</see>.
	/// 
	/// NOTE: Linked-list-backed because we need extremely fast push and pop.
	/// Memory overhead of linked list is negligable because it is used in a stack context.
	/// </summary>
	internal sealed class FastStack<T> where T : class
	{
		/// <summary>
		/// Node for the linked list.
		/// </summary>
		private class Node
		{
			public T Value;
			public Node Next;
		}
		
		/// <summary>
		/// The root (first) node.
		/// </summary>
		private Node top;
		
		/// <summary>
		/// Pushes the given value onto the stack.
		/// </summary>
		/// <param name="value">The value to push.</param>
		public void Push(T value)
		{
			//add new node at front
			Node newNode = new Node{Value = value};
			newNode.Next = top;
			top = newNode;
		}
		
		/// <summary>
		/// Pops the top value off the stack.
		/// </summary>
		public void Pop()
		{
			top = top.Next;
		}
		
		/// <summary>
		/// Returns the top value, or null if there are no values on the stack.
		/// </summary>
		public T Top
		{
			get{return (top != null) ? top.Value : null;}
		}
	}
}
