namespace IROM.Dynamix
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Linq;
	using System.Linq.Expressions;
	
	/// <summary>
	/// A generic dynamix variable.
	/// </summary>
	public sealed class Dynx<T> : Dynx
	{
		//static testing for value type inequality, null if not valid
		private static readonly Func<T, T, bool> NotEqual;
		static Dynx()
		{
			try
			{
				if(typeof(T).IsValueType || typeof(T) == typeof(string))
				{
					ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
					ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
					NotEqual = Expression.Lambda<Func<T, T, bool>>(Expression.NotEqual(paramA, paramB), paramA, paramB).Compile();
				}else
				{
					NotEqual = null;
				}
			}catch(InvalidOperationException)
			{
				NotEqual = null;
			}catch(ArgumentException)
			{
				NotEqual = null;
			}
		}
		
		/// <summary>
		/// Current value storage.
		/// </summary>
		private T baseValue;
		
		/// <summary>
		/// Current expression storage.
		/// </summary>
		private Func<bool, T> baseExp;
		
		/// <summary>
		/// The root node of a linked list of filters.
		/// </summary>
		private Node<Func<T, T>> filterRoot;
		
		/// <summary>
		/// The current value of this <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public T Value
		{
			get
			{
				Ping();
				return baseValue;
			}
			set
			{
				baseValue = value;
				Exp = null;
			}
		}
		
		/// <summary>
		/// The current expression for this <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public Func<bool, T> Exp
		{
			get
			{
				return baseExp;
			}
			set
			{
				foreach(Dynx dynx in GetParents())
				{
					dynx.Unsubscribe(UpdateListener);
				}
				baseExp = value;
				foreach(Dynx dynx in GetParents())
				{
					dynx.Subscribe(UpdateListener, true);
				}
				Update();
			}
		}
		
		/// <summary>
		/// Creates a new <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public Dynx(){}
		
		/// <summary>
		/// Creates a new <see cref="Dynx{T}">Dynx</see> variable with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public Dynx(T value)
		{
			Value = value;
		}
		
		/// <summary>
		/// Creates a new <see cref="Dynx{T}">Dynx</see> variable with the given expression value.
		/// </summary>
		/// <param name="exp">The expression.</param>
		public Dynx(Func<bool, T> exp)
		{
			Exp = exp;
		}
		
		/// <summary>
		/// Pings this <see cref="Dynx{T}">Dynx</see> variable, informing it of a dependant.
		/// Used for manual test handling.
		/// </summary>
		public void Ping()
		{
			if(currentThread == Thread.CurrentThread)
			{
				currentParents.AddLast(this);
			}
		}
		
		public override void Update()
		{
			T prev = baseValue;
			if(baseExp != null)
				//evaluate with test flag off
				baseValue = baseExp(false);
			for(var node = filterRoot; node != null; node = node.Next)
			{
				baseValue = node.Value(baseValue);
			}
			if(baseExp == null || (NotEqual == null || NotEqual(baseValue, prev)))
			{
				foreach(Action listener in GetListeners())
				{
					listener();
				}
			}
		}
		
		/// <summary>
		/// Returns an enumerator to this variable's current parents.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerable<Dynx> GetParents()
		{
			if(baseExp != null)
			{
				lock(parentLock)
				{
					currentThread = Thread.CurrentThread;
					//call with test flag set
					baseExp(true);
					foreach(Dynx dynx in currentParents)
					{
						yield return dynx;
					}
					currentParents.Clear();
					currentThread = null;
				}
			}
		}
		
		/// <summary>
		/// Adds a new filter.
		/// </summary>
		/// <param name="filter">The filter to add.</param>
		public void AddFilter(Func<T, T> filter)
		{
			var node = new Node<Func<T, T>>();
			node.Value = filter;
			
			//add to end of filter collection
			if(filterRoot == null) filterRoot = node;
			else
			{
				var prev = filterRoot;
				while(prev.Next != null)
				{
					prev = prev.Next;
					//don't double subscribe
					if(prev.Value == filter)
					{
						return;
					}
				}
				prev.Next = node;
			}
		}
		
		/// <summary>
		/// Removes the given filter.
		/// </summary>
		/// <param name="filter">The filter to remove.</param>
		public void RemoveFilter(Func<T, T> filter)
		{
			var prev = filterRoot;
			for(var node = filterRoot; node != null; 
			    prev = node, node = node.Next)
			{
				if(node.Value == filter)
				{
					if(node == filterRoot)
					{
						filterRoot = node.Next;
					}else
					{
						prev.Next = node.Next;
						node = prev;
					}
					break;
				}
			}
		}
	}
	
	/// <summary>
	/// Base non-generic class for Dynx variables.
	/// </summary>
	public abstract class Dynx
	{
		//
		//Parentage determination info
		internal static object parentLock = new object();
		internal static volatile Thread currentThread;
		internal static LinkedList<Dynx> currentParents = new LinkedList<Dynx>();
		//
		//
		
		/// <summary>
		/// Node in relation list.
		/// </summary>
		protected internal class Node<T>
		{
			public Node<T> Next;
			public T Value;
		}
		
		/// <summary>
		/// The root node of a linked list of weak references to listeners.
		/// </summary>
		protected internal Node<GCHandle> childrenRoot;
		
		/// <summary>
		/// Reference to our own update so GC is tied to this object, not the delegate created.
		/// </summary>
		protected Action UpdateListener;
		
		protected Dynx()
		{
			UpdateListener = Update;
		}
		
		/// <summary>
		/// Reevaluates this variable.
		/// </summary>
		public abstract void Update();
		
		/// <summary>
		/// Adds a dependent to this source.
		/// </summary>
		/// <param name="listener">The dependant.</param>
		/// <param name="weak">True if this is a weak reference.</param>
		public void Subscribe(Action listener, bool weak = false)
		{
			var node = new Node<GCHandle>();
			node.Value = GCHandle.Alloc(listener, weak ? GCHandleType.Weak : GCHandleType.Normal);
			
			//add to end of listener collection
			if(childrenRoot == null) childrenRoot = node;
			else
			{
				var prev = childrenRoot;
				while(prev.Next != null)
				{
					prev = prev.Next;
					//don't double subscribe
					if((prev.Value.Target as Action) == listener)
					{
						node.Value.Free();
						return;
					}
				}
				prev.Next = node;
			}
		}
		
		/// <summary>
		/// Adds a dependant Dynx var to this source. 
		/// DO NOT CALL MANUALLY unless you know what you are doing.
		/// </summary>
		/// <param name="dynx">The dependant.</param>
		/// <param name="weak">True if this is a weak reference.</param>
		public void Subscribe(Dynx dynx, bool weak = false)
		{
			Subscribe(dynx.UpdateListener, weak);
		}
		
		/// <summary>
		/// Removes dependent from this source.
		/// </summary>
		/// <param name="a">The dependant.</param>
		public void Unsubscribe(Action a)
		{
			var prev = childrenRoot;
			for(var node = childrenRoot; node != null; 
			    prev = node, node = node.Next)
			{
				Action value = node.Value.Target as Action;
				if(value == null || value == a)
				{
					if(node == childrenRoot)
					{
						childrenRoot = node.Next;
					}else
					{
						prev.Next = node.Next;
						node.Value.Free();
						node = prev;
					}
					//if done, stop iterating
					if(value == a)
						break;
				}
			}
		}
		
		/// <summary>
		/// Removes dependent Dynx var from this source.
		/// DO NOT CALL MANUALLY unless you know what you are doing.
		/// </summary>
		/// <param name="dynx">The dependant.</param>
		public void Unsubscribe(Dynx dynx)
		{
			Unsubscribe(dynx.UpdateListener);
		}
		
		/// <summary>
		/// Returns an enumeration of listeners in this source.
		/// </summary>
		/// <returns>The enumeration.</returns>
		protected internal IEnumerable<Action> GetListeners()
		{
			var prev = childrenRoot;
			for(var node = childrenRoot; node != null; 
			    prev = node, node = node.Next)
			{
				Action value = node.Value.Target as Action;
				if(value == null)
				{
					if(node == childrenRoot)
					{
						childrenRoot = node.Next;
					}else
					{
						prev.Next = node.Next;
						node.Value.Free();
						node = prev;
					}
					continue;
				}
				yield return value;
			}
		}
	}
}
