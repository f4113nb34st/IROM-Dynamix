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
				foreach(Action listener in val.GetListeners())
				{
					value.Subscribe(listener);
				}
				val = value;
			}
		}
	}
}
