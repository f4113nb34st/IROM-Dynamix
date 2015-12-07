namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// Wrapper that must be used with all dynamix values.
	/// </summary>
	public class DWrapper<T> where T : DSource
	{
		private T val;
		
		public T Value
		{
			get
			{
				return val;
			}
			set
			{
				T oldVal = val;
				val = value;
				if(oldVal != null)
				{
					foreach(Action listener in oldVal.GetListeners())
					{
						val.Subscribe(listener);
						listener();
					}
				}
			}
		}
		
		public T GetValue()
		{
			return val;
		}
	}
}
