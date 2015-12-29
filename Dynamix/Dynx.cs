namespace IROM.Dynamix
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
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
		/// The collection of filters.
		/// NOTE: Array-backed instead of other collections because we 
		/// want minimal memory footprint and fast iteration,
		/// but don't need fast add or remove, as those
		/// are relatively infrequent operations.
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
				Dynx top = childStack.GetForCurrentThread().Top;
				if(top != null)
				{
					updateListeners.Add(ref top.updateHandle, ref top.updateListener);
				}
				//return value
				return baseValue;
			}
			set
			{
				//make exp null to mark as constant
				baseExp = null;
				//if value changed, update
				if(!EqualityComparer<T>.Default.Equals(baseValue, value))
				{
					//set value
					baseValue = value;
					//update
					UpdateConstant();
				}
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
				//set expression and update
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
				//add value
				using(ArrayUtil.Lock(ref filters))
				{
					ArrayUtil.Add(ref filters, value);
				}
				//update to allow the new filter to take effect
				Update();
			}
			remove
			{
				//remove value
				using(ArrayUtil.Lock(ref filters))
				{
					ArrayUtil.Remove(ref filters, value);
				}
				//update to allow the filter to fall out of effect
				Update();
			}
		}
		
		/// <summary>
		/// Creates a new <see cref="Dynx{T}">Dynx</see> variable with a constant value of default(T)
		/// </summary>
		public Dynx()
		{
			updateListener = Update;
			updateHandle = new WeakHandle<Action>(updateListener, true);
		}
		
		/// <summary>
		/// Creates a new <see cref="Dynx{T}">Dynx</see> variable with the given constant value.
		/// </summary>
		/// <param name="value">The value.</param>
		public Dynx(T value) : this()
		{
			Value = value;
		}
		
		/// <summary>
		/// Creates a new <see cref="Dynx{T}">Dynx</see> variable with the given expression value.
		/// </summary>
		/// <param name="exp">The expression.</param>
		public Dynx(Func<T> exp) : this()
		{
			Exp = exp;
		}
		
		/// <summary>
		/// Re-evaluates this <see cref="Dynx{T}">Dynx</see> variable.
		/// </summary>
		public void Update()
		{
			//create var for new value
			T newValue = baseValue;
			
			//only evaluate if something might have changed
			if(baseExp != null || filters.Length > 0)
			{
				//use a new set of parents
				using(new ParentContext(this))
				{
					//evaluate expression
					if(baseExp != null)
					{
						newValue = baseExp();
					}
					//filter value
					newValue = Filter(newValue);
				}
			}
			
			//if changed or constant (since constant.Update is only called once), update baseValue and call listeners
			if(!EqualityComparer<T>.Default.Equals(newValue, baseValue))
			{
				//set value to new
				baseValue = newValue;
				//update all listeners
				NotifyListeners();
			}
		}
		
		/// <summary>
		/// Re-evaluates this <see cref="Dynx{T}">Dynx</see> variable. Forces an update because constant initial evaluations report false negatives.
		/// </summary>
		private void UpdateConstant()
		{
			//only evaluate if there are filters to influence it
			if(filters.Length > 0)
			{
				//filter value with new set of parents
				using(new ParentContext(this))
				{
					baseValue = Filter(baseValue);
				}
			}
			
			//guaranteed update
			NotifyListeners();
		}
		
		/// <summary>
		/// Filters the given value with all the filters.
		/// </summary>
		/// <param name="value">The value to filter.</param>
		/// <returns>The resulting value.</returns>
		public T Filter(T value)
		{
			for(int i = 0; i < filters.Length; i++)
			{
				value = filters[i](value);
			}
			return value;
		}
		
		/// <summary>
		/// Notifies all listeners of an update.
		/// </summary>
		private void NotifyListeners()
		{
			UpdateQueue<Action> queue = updateQueue.GetForCurrentThread();
			
			//update all listeners
			foreach(Action listener in updateListeners)
			{
				queue.Offer(listener);
			}
			
			//if master manager hasn't been started, start one
			if(!queue.masterInProgress)
			{
				queue.masterInProgress = true;
				Action listener;
				while((listener = queue.Poll()) != null)
				{
					listener();
				}
				queue.masterInProgress = false;
			}
		}
		
		/// <summary>
		/// Simplex class that handles catching a new set of parents, and disposing of the old ones.
		/// </summary>
		private struct ParentContext : IDisposable
		{
			private readonly FastStack<Dynx> stack;
			private readonly WeakHandle<Action> oldHandle;
			
			public ParentContext(Dynx var)
			{
				//save handle for a bit so updateListeners.Add can detect old entry and steal it
				//this saves frequent unnecessary garbage collection
				oldHandle = var.updateHandle;
				
				//create new handle for new parents
				var.updateHandle = new WeakHandle<Action>(var.updateListener, true);
				
				//set child so we catch parents so we catch parents from evaluation and filtering
				stack = childStack.GetForCurrentThread();
				stack.Push(var);
			}
			
			public void Dispose()
			{
				//clear child
				stack.Pop();
				//dispose old handle
				oldHandle.Dispose();
			}
		}
	}
	
	/// <summary>
	/// Base non-generic class for <see cref="Dynx{T}">Dynx</see> variables.
	/// </summary>
	public abstract class Dynx
	{
		/// <summary>
		/// Stores the queue of listeners awaiting updates. 
		/// All <see cref="Dynx{T}">Dynx</see> variables add their listeners to this collection.
		/// </summary>
		internal static ThreadLocalCollection<UpdateQueue<Action>> updateQueue = new ThreadLocalCollection<UpdateQueue<Action>>(/*dummy*/false);
		
		/// <summary>
		/// Stores the currently updating <see cref="Dynx{T}">Dynx</see> variables so parentage can be acquired.
		/// <see cref="Dynx{T}">Dynx</see> variables referenced during the update automatically add the top variable to their listeners.
		/// </summary>
		internal static ThreadLocalCollection<FastStack<Dynx>> childStack = new ThreadLocalCollection<FastStack<Dynx>>(/*dummy*/false);
		
		/// <summary>
		/// The root node of a linked list of weak references to listeners.
		/// </summary>
		internal ListenerCollection<Action> updateListeners = new ListenerCollection<Action>(/*dummy*/false);
		
		/// <summary>
		/// Reference to our own update so GC is tied to this object, not the delegate created.
		/// </summary>
		internal Action updateListener;
		
		/// <summary>
		/// Reference our current update handle, so it can be invalidated instantly.
		/// </summary>
		internal WeakHandle<Action> updateHandle;
		
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
	}
}
