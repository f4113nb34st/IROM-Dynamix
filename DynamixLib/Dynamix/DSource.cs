namespace IROM.Dynamix
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// Source that triggers Dynamix elements to update.
	/// </summary>
	public unsafe class DSource
	{
		/// <summary>
		/// The root node of a linked list of weak references to listeners.
		/// </summary>
		protected internal HandleNode root = new HandleNode();
		
		/// <summary>
		/// Called when the value associated with this source is changed.
		/// </summary>
		public virtual void OnChange()
		{
			foreach(Action listener in GetListeners())
			{
				listener();
			}
		}
		
		/// <summary>
		/// Add dependent to this source.
		/// </summary>
		/// <param name="a">The dependant.</param>
		public void Subscribe(Action a)
		{
			HandleNode node = new HandleNode();
			node.Handle = GCHandle.Alloc(a, GCHandleType.Weak);
			node.Next = root.Next;
			root.Next = node;
		}
		
		/// <summary>
		/// Removes dependent from this source.
		/// </summary>
		/// <param name="a">The dependant.</param>
		public void Unsubscribe(Action a)
		{
			foreach(Action listener in GetListeners())
			{
				if(listener == a)
				{
					HandleNode.Remove = true;
				}
			}
		}
		
		/// <summary>
		/// Returns an enumeration of listeners in this source.
		/// </summary>
		/// <returns>The enumeration.</returns>
		protected IEnumerable<Action> GetListeners()
		{
			HandleNode prev;
			HandleNode node = root;
			while(node.Next != null)
			{
				prev = node;
				node = node.Next;
				Action value = node.Handle.Target as Action;
				if(value == null)
				{
					prev.Next = node.Next;
					continue;
				}
				HandleNode.Remove = false;
				yield return value;
				if(HandleNode.Remove)
				{
					prev.Next = node.Next;
				}
			}
		}
		
		protected internal class HandleNode
		{
			internal static bool Remove = false;
			public HandleNode Next;
			public GCHandle Handle;
		}
	}
}
