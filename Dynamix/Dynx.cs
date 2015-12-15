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
	public class Dynx<T> : Dynx where T : struct
	{
		//static testing for value type inequality, null if not valid
		private static readonly Func<T, T, bool> NotEqual;
		static Dynx()
		{
			try
			{
				ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
				ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
				NotEqual = Expression.Lambda<Func<T, T, bool>>(Expression.NotEqual(paramA, paramB), paramA, paramB).Compile();
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
				if(currentThread == Thread.CurrentThread)
				{
					currentParents.AddLast(this);
				}
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
		public Func<T> Exp
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
		public Dynx(Func<T> exp)
		{
			Exp = exp;
		}
		
		public override void Update()
		{
			T prev = baseValue;
			if(baseExp != null)
				baseValue = baseExp();
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
					baseExp();
					foreach(Dynx dynx in currentParents)
					{
						yield return dynx;
					}
					currentParents.Clear();
					currentThread = null;
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
		/// <param name="a">The dependant.</param>
		/// <param name="weak">True if this is a weak reference.</param>
		public void Subscribe(Action a, bool weak = false)
		{
			Node<GCHandle> node = new Node<GCHandle>();
			node.Value = GCHandle.Alloc(a, weak ? GCHandleType.Weak : GCHandleType.Normal);
			node.Next = childrenRoot;
			childrenRoot = node;
		}
		
		/// <summary>
		/// Removes dependent from this source.
		/// </summary>
		/// <param name="a">The dependant.</param>
		public void Unsubscribe(Action a)
		{
			Node<GCHandle> prev = null;
			for(Node<GCHandle> node = childrenRoot; node != null; 
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
						node = prev;
					}
				}
			}
		}
		
		/// <summary>
		/// Returns an enumeration of listeners in this source.
		/// </summary>
		/// <returns>The enumeration.</returns>
		protected internal IEnumerable<Action> GetListeners()
		{
			Node<GCHandle> prev = null;
			for(Node<GCHandle> node = childrenRoot; node != null; 
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
						node = prev;
					}
					continue;
				}
				yield return value;
			}
		}
	}
}
