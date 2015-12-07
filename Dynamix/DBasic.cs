namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// Base class for simple (single value, not vectors) dynamix elements.
	/// </summary>
	public abstract class DBasic<T> : DElement<T>
	{
		// Backing fields
		protected Func<T> expression;
		
		protected DBasic(Func<T> exp)
		{
			expression = exp;
			lock(sourceLock)
			{
				currentSource = this;
				// disable once DoNotCallOverridableMethodsInConstructor
				Update();
				currentSource = null;
			}
		}
		
		protected override void Update()
		{
			Value = expression();
		}
	}
}
