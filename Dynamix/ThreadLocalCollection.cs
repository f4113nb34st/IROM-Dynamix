namespace IROM.Dynamix
{
	using System;
	using System.Threading;
	
	/// <summary>
	/// Thread-dependant collection implementation, where each thread has it's own collection.
	/// Custom-made for the child stacks and update queues of <see cref="Dynx{T}">Dynx</see>.
	/// 
	/// NOTE: Sorted-array-backed because adding only happens once and we need fast searching.
	/// </summary>
	internal struct ThreadLocalCollection<T> where T : class, new()
	{
		/// <summary>
		/// An entry in the child stack map.
		/// </summary>
		private struct Entry
		{
			public int id;
			public T value;
		}
		
		/// <summary>
		/// The entries in the map.
		/// </summary>
		private Entry[] entries;
		
		public ThreadLocalCollection(bool dummy)
		{
			entries = new Entry[0];
		}
		
		/// <summary>
		/// Returns the collection for Thread.CurrentThread.
		/// </summary>
		/// <returns>The collection for this thread.</returns>
		public T GetForCurrentThread()
		{
			//get id for current thread (hashcode)
			int id = Thread.CurrentThread.ManagedThreadId;//.GetHashCode();
			//the index the entry should have been
			int index;
			T collection = Find(entries, id, out index);
			
			//if not found
			if(collection == null)
			{
				collection = new T();
				using(ArrayUtil.Lock(ref entries))
				{
					//fix index if other resizes moved it
					//if the change is above it won't affect index
					//if the change is below it will make index larger
					//index is right if entries[middle] is the smallest value larger than the id
					//therefore, increase while entries[middle] is smaller
					while(index < entries.Length && entries[index].id < id)
					{
						index++;
					}
					
					//make addition
					Entry[] newEntries = new Entry[entries.Length + 1];
					//copy below index
					for(int i = 0; i < index; i++) newEntries[i] = entries[i];
					//copy value
					newEntries[index] = new Entry{id = id, value = collection};
					//copy above index
					for(int i = index; i < entries.Length; i++) newEntries[i + 1] = entries[i];
					//update entries
					entries = newEntries;
				}
			}
			
			return collection;
		}
		
		/// <summary>
		/// Finds the collection at the given id, or null if not present.
		/// Index gets the location where the entry was or should have been.
		/// </summary>
		/// <param name="entries">The entries to search through.</param>
		/// <param name="id">The id key.</param>
		/// <param name="index">The index the entry was or should have been.</param>
		/// <returns>The collection at the id.</returns>
		private static T Find(Entry[] entries, int id, out int index)
		{
			//search by binary search
			//bottom is start of array
			int bottom = 0;
			//top is end of array
			int top = entries.Length - 1;
			//index will store the compare index
			//init to bottom in case loop is skipped
			index = bottom;
			//temp variable for entry at middle
			Entry entry = default(Entry);
			//while region valid
			while(bottom <= top)
			{
				//find center
				index = (top + bottom) / 2;
				//get entry
				entry = entries[index];
				//if right thread, return stack
				if(entry.id == id)
				{
					return entry.value;
				}else
				//keep going
				//if compare was too big, search below
				if(entry.id > id)
				{
					top = index - 1;
				}else
				//else compare was too small, search above
				{
					bottom = index + 1;
				}
			}
			//set index to location to insert new entry
			index = (entry.id > id) ? index : bottom;
			return null;
		}
	}
}
