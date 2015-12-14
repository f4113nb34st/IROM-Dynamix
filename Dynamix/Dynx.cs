namespace IROM.Dynamix
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// A generic dynamix variable of 
	/// </summary>
	public class Dynx<T> : Dynx
	{
		// disable once StaticFieldInGenericType
		private static readonly bool IsValueType = typeof(T).IsValueType;
		
		/// <summary>
		/// Current value storage.
		/// </summary>
		protected T baseValue;
		/// <summary>
		/// Current expression storage.
		/// </summary>
		protected Func<T> baseExp;
		
		/// <summary>
		/// The current value of this <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public T Value
		{
			get
			{
				if(currentSource != null) Subscribe(currentSource.Update);
				return baseValue;
			}
			set
			{
				Exp = () => value;
			}
		}
		
		/// <summary>
		/// The current expression for this <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public Func<T> Exp
		{
			get
			{
				return baseExp;
			}
			protected set
			{
				baseExp = value;
				Update();
			}
		}
		
		public Dynx(){}
		
		public Dynx(T value)
		{
			Value = value;
		}
		
		public Dynx(Func<T> exp)
		{
			Exp = exp;
		}
		
		public override void Update()
		{
			T prev = baseValue;
			if(baseExp != null)
				baseValue = baseExp();
			if(!IsValueType || !baseValue.Equals(prev))
			{
				lock(sourceLock)
				{
					currentSource = this;
					foreach(Action listener in GetListeners())
					{
						listener();
					}
					currentSource = null;
				}
			}
		}
		
		public static implicit operator T(Dynx<T> dynx)
		{
			return dynx.Value;
		}
		
		public static implicit operator Dynx<T>(T value)
		{
			return new Dynx<T>(value);
		}
		
		public static implicit operator Dynx<T>(Func<T> exp)
		{
			return new Dynx<T>(exp);
		}
	}
	
	/// <summary>
	/// Base non-generic class for Dynx variables.
	/// </summary>
	public abstract class Dynx
	{
		//
		//Parentage determination info
		//
		private static readonly Stack<Dynx> sourceStack = new Stack<Dynx>();
		internal static object sourceLock = new object();
		internal static Dynx currentSource
		{
			get{return ((sourceStack.Count > 0) ? sourceStack.Peek() : null);}
			set
			{
				if(value == null) sourceStack.Pop();
				else              sourceStack.Push(value);
			}
		}
		//
		//
		//
		
		/// <summary>
		/// The root node of a linked list of weak references to listeners.
		/// </summary>
		protected internal HandleNode root = new HandleNode();
		
		/// <summary>
		/// Node in weak linked list.
		/// </summary>
		protected internal class HandleNode
		{
			internal static bool Remove = false;
			public HandleNode Next;
			public GCHandle Handle;
		}
		
		/// <summary>
		/// Reevaluates this variable.
		/// </summary>
		public abstract void Update();
		
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
		protected internal IEnumerable<Action> GetListeners()
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
	}
}
