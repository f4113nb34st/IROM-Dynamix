namespace IROM.Dynamix
{
	using System;
	using System.Threading;
	
	/// <summary>
	/// Utilities for working with arrays.
	/// </summary>
	internal static class ArrayUtil
	{
		/// <summary>
		/// Adds the given value to the end of the given array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="value">The value to add.</param>
		public static void Add<E>(ref E[] array, E value)
		{
			E[] newArray = new E[array.Length + 1];
			//copy old values
			for(int i = 0; i < array.Length; i++) newArray[i] = array[i];
			//add new value
			newArray[newArray.Length - 1] = value;
			//set array to new
			array = newArray;
		}
		
		/// <summary>
		/// Removes the given value from the given array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="value">The value to remove.</param>
		public static void Remove<E>(ref E[] array, E value)
		{
			//find value in array and the remove the index
			RemoveIndex(ref array, Array.IndexOf(array, value));
		}
		
		/// <summary>
		/// Removes the given index from the given array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index to remove.</param>
		public static void RemoveIndex<E>(ref E[] array, int index)
		{
			//remove the index if exists
			if(index == -1) return;
			E[] newArray = new E[array.Length - 1];
			//copy values
			for(int i = 0;         i < index;          i++) newArray[i] = array[i];
			for(int i = index + 1; i < array.Length; i++) newArray[i - 1] = array[i];
			array = newArray;
		}
		
		/// <summary>
		/// Locks around the given array reference instead of a specific array object.
		/// </summary>
		/// <param name="array">The array reference to lock.</param>
		/// <returns>Returns the unlock action.</returns>
		public static IDisposable Lock<E>(ref E[] array)
		{
			while(true)
			{
				E[] snapshot = array;
				//get lock on array
				Monitor.Enter(snapshot);
				//if array changed while we waited, try entering again
				if(snapshot != array)
				{
					Monitor.Exit(snapshot);
					continue;
				}
				return new Unlocker{obj = snapshot};
			}
		}
		
		/// <summary>
		/// Lock object for a specific array.
		/// </summary>
		private struct Unlocker : IDisposable
		{
			internal object obj;
	
			public void Dispose()
			{
				Monitor.Exit(obj);
			}
		}
	}
}
