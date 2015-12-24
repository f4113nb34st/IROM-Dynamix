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
		/// <summary>
		/// Current value storage.
		/// </summary>
		private T baseValue;
		
		/// <summary>
		/// Current expression storage.
		/// If null, baseValue is treated as a constant.
		/// </summary>
		private Func<T> baseExp;
		
		/// <summary>
		/// The array of filters.
		/// </summary>
		private Func<T, T>[] filters = new Func<T, T>[0];
		
		/// <summary>
		/// The current value of this <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public T Value
		{
			get
			{
				//if evaluating, add as subscriber
				Dynx child;
				currentChildren.TryGetValue(Thread.CurrentThread, out child);
				if(child != null)
				{
					OnUpdateWeak += child.updateListener;
				}
				//return value
				return baseValue;
			}
			set
			{
				//to set as constant, set to dummy, then update and null out
				baseExp = () => value;
				Update();
				baseExp = null;
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
				baseExp = value;
				Update();
			}
		}
		
		/// <summary>
		/// Called to filter values for this <see cref="Dynx{T}">Dynx</see> variable.
		/// Filters are called in the order subscribed with each output becoming the input of the next.
		/// </summary>
		public event Func<T, T> OnFilter
		{
			add
			{
				Func<T, T>[] newArray = new Func<T, T>[filters.Length + 1];
				//copy old values
				for(int i = 0; i < filters.Length; i++) newArray[i] = filters[i];
				//add new value
				newArray[newArray.Length - 1] = value;
				//set array to new
				filters = newArray;
			}
			remove
			{
				int index = Array.IndexOf(filters, value);
				if(index == -1) return;
				Func<T, T>[] newArray = new Func<T, T>[filters.Length - 1];
				//copy values
				for(int i = 0;         i < index;          i++) newArray[i] = filters[i];
				for(int i = index + 1; i < filters.Length; i++) newArray[i - 1] = filters[i];
				filters = newArray;
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
			T newValue = baseValue;
			
			//refresh update listener to dispose of old parentage
			updateListener = Update;
			
			//evaluate and filter with child set so we catch parents
			currentChildren[Thread.CurrentThread] = this;
			
			//base evaluation
			if(baseExp != null)
			{
				newValue = baseExp();
			}
			//call filters
			foreach(var filter in filters)
			{
				newValue = filter(newValue);
			}
			
			//clear child
			currentChildren.Remove(Thread.CurrentThread);
			
			//if changed, update baseValue and call listeners
			if(!EqualityComparer<T>.Default.Equals(newValue, baseValue))
			{
				baseValue = newValue;
				foreach(Action listener in updateListeners)
				{
					listener();
				}
			}
		}
	}
	
	/// <summary>
	/// Base non-generic class for <see cref="Dynx{T}">Dynx</see> variables.
	/// </summary>
	public abstract class Dynx
	{
		/// <summary>
		/// Stores the currently updating <see cref="Dynx{T}">Dynx</see> variable so parentage can be acquired.
		/// <see cref="Dynx{T}">Dynx</see> variables referenced during the update automatically add this variable to their listeners.
		/// </summary>
		internal static Dictionary<Thread, Dynx> currentChildren = new Dictionary<Thread, Dynx>();
		
		/// <summary>
		/// The root node of a linked list of weak references to listeners.
		/// </summary>
		internal WeakCollection<Action> updateListeners = new WeakCollection<Action>();
		
		/// <summary>
		/// Reference to our own update so GC is tied to this object, not the delegate created.
		/// </summary>
		internal Action updateListener;
		
		/// <summary>
		/// Called when the value for this <see cref="Dynx{T}">Dynx</see> variable changes.
		/// </summary>
		public event Action OnUpdate
		{
			add
			{
				updateListeners.Add(value, false);
			}
			remove
			{
				updateListeners.Remove(value);
			}
		}
		
		/// <summary>
		/// Called when the value for this <see cref="Dynx{T}">Dynx</see> variable changes.
		/// Weakly references the listener.
		/// </summary>
		public event Action OnUpdateWeak
		{
			add
			{
				updateListeners.Add(value, true);
			}
			//same as OnUpdate.remove, provided for symmetry
			remove
			{
				updateListeners.Remove(value);
			}
		}
		
		~Dynx()
		{
			updateListeners.Dispose();
		}
		
		/// <summary>
		/// Re-evaluates this <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public abstract void Update();
	}
	
	/// <summary>
	/// Node for a weak linked list.
	/// </summary>
	internal unsafe struct WeakNode
	{
		public GCHandle handle;
		public WeakNode* next;
	}
	
	/// <summary>
	/// A linked list implementation with minimal memory footprint and automatic weak referencing.
	/// </summary>
	internal unsafe struct WeakCollection<T> : IDisposable, IEnumerable<T> where T : class
	{
		internal WeakNode* root;

		public void Dispose()
		{
			//on dispose, free all handles
			for(WeakNode* node = root; node != null; node = (*node).next)
			{
				(*node).handle.Free();
			}
		}
		
		/// <summary>
		/// Adds the given value to the collection.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="weak">True if the reference should be weak.</param>
		public void Add(T value, bool weak = false)
		{
			GCHandle handle = GCHandle.Alloc(value, weak ? GCHandleType.Weak : GCHandleType.Normal);
			
			//first search for duplicates
			WeakNode* last = null;
			for(WeakNode* node = root; node != null; last = node, node = (*node).next)
			{
				//if we find duplicate, don't re-add
				if((*node).handle == handle)
					return;
			}
			
			//add new node
			WeakNode newNode = new WeakNode{handle = handle, next = null};
			if(last != null) (*last).next = &newNode;
			else			 root = &newNode;
		}
		
		public void Remove(T value)
		{
			WeakNode* prev = null;
			for(WeakNode* node = root; node != null; prev = node, node = (*node).next)
			{
				GCHandle handle = (*node).handle;
				T nodeValue = handle.Target as T;
				//if value to remove or lost reference
				if(nodeValue == value || nodeValue == null)
				{
					//remove node
					handle.Free();
					if(prev != null) (*prev).next = (*node).next;
					else 			 root = (*node).next; 
					//if was value, we're done
					if(nodeValue == value) return;
				}
			}
		}

		public IEnumerator<T> GetEnumerator(){return new WeakCollectionEnumerator<T>{collection = this, node = root};}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){return GetEnumerator();}
	}
	
	/// <summary>
	/// Simple enumerator for WeakCollection.
	/// </summary>
	internal unsafe struct WeakCollectionEnumerator<T> : IEnumerator<T> where T : class
	{
		public WeakCollection<T> collection;
		private WeakNode* prevNode;
		public WeakNode* node;
		private T nodeValue;
		
		public bool MoveNext()
		{
			nodeValue = null;
			//each iteration of the loop:
			//node is the current node we are checking for return validity
			//prevNode is the node preceding it in the collection
			while(true)
			{
				if(node == null) break;
				
				GCHandle handle = (*node).handle;
				nodeValue = handle.Target as T;
				if(nodeValue == null)
				{
					handle.Free();
					if(prevNode != null) (*prevNode).next = (*node).next;
					else 			 	 collection.root = (*node).next; 
					//advance
					node = (*node).next;
				}else break;
			}
			//advance for next time
			//we do this here instead of the beginning so when node is initialized to first it gets checked
			if(node != null)
			{
				prevNode = node;
				node = (*node).next;
			}
			return nodeValue != null;
		}
		
		public T Current{get{return nodeValue;}}
		public void Dispose(){}
		object System.Collections.IEnumerator.Current{get{return Current;}}
		public void Reset(){throw new NotImplementedException();}
	}
}
