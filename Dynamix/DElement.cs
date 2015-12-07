namespace IROM.Dynamix
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// Base class for dynamix elements.
	/// </summary>
	public abstract class DElement<T> : DSource
	{
		// disable once StaticFieldInGenericType
		private static readonly Stack<DSource> sourceStack = new Stack<DSource>();
		protected static DSource currentSource
		{
			get{return sourceStack.Peek();}
			set
			{
				if(value == null) sourceStack.Pop();
				else              sourceStack.Push(value);
			}
		}
		
		// Backing fields
		private T baseValue;
		protected internal T Value
		{
			get
			{
				if(currentSource != null) Subscribe(currentSource.OnChange);
				return baseValue;
			}
			set
			{
				baseValue = value;
			}
		}
		
		public override void OnChange()
		{
			Update();
			base.OnChange();
		}
		
		/// <summary>
		/// Updates this <see cref="DElement{T}">DElement</see>.
		/// </summary>
		protected abstract void Update();
		
		/// <summary>
		/// Returns a new <see cref="DElement{T}">DElement</see> that is based on this <see cref="DElement{T}">DElement</see>.
		/// </summary>
		/// <returns>An extension.</returns>
		protected internal abstract DElement<T> GetExtension();
		
		public override string ToString()
		{
			return Value.ToString();
		}
		
		public static implicit operator T(DElement<T> ele)
		{
			return ele.Value;
		}
		
		public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return Value.GetHashCode();
        }
	}
}
