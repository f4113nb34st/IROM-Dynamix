namespace IROM.Dynamix
{
	using System;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// Typed, reference-type wrapper for GCHandle.
	/// </summary>
	internal sealed class WeakHandle<T> : IDisposable where T : class
	{
		/// <summary>
		/// The handle we're wrapping.
		/// </summary>
		internal GCHandle handle;
		
		/// <summary>
		/// Creates a new WeakHandle with the given value to reference.
		/// </summary>
		/// <param name="value">The value to reference.</param>
		/// <param name="weak">True if the reference should be weak.</param>
		public WeakHandle(T value, bool weak)
		{
			handle = GCHandle.Alloc(value, weak ? GCHandleType.Weak : GCHandleType.Normal);
		}
		
		~WeakHandle(){Dispose();}
	
		public void Dispose()
		{
			//dispose of our handle
			if(handle.IsAllocated) handle.Free();
		}
		
		/// <summary>
		/// The value currently referenced. Null if disposed or value was GC'd.
		/// </summary>
		public T Value
		{
			get{return handle.IsAllocated ? (handle.Target as T) : null;}
		}
	}
}
