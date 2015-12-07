namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// Math extension for Dynamix variables. See <see cref="Math"/> for information for each method.
	/// </summary>
	public static class DMath
	{
		public static DDouble Acos(DDouble value)
		{
			return (DDouble)(() => Math.Acos(value));
		}
		
		public static DDouble Asin(DDouble value)
		{
			return (DDouble)(() => Math.Asin(value));
		}

		public static DDouble Atan(DDouble value)
		{
			return (DDouble)(() => Math.Atan(value));
		}

		public static DDouble Atan2(DDouble value, DDouble value2)
		{
			return (DDouble)(() => Math.Atan2(value, value2));
		}

		public static DDouble Ceiling(DDouble value)
		{
			return (DDouble)(() => Math.Ceiling(value));
		}

		public static DDouble Cos(DDouble value)
		{
			return (DDouble)(() => Math.Cos(value));
		}

		public static DDouble Cosh(DDouble value)
		{
			return (DDouble)(() => Math.Cosh(value));
		}

		public static DDouble Floor(DDouble value)
		{
			return (DDouble)(() => Math.Floor(value));
		}

		public static DDouble Sin(DDouble value)
		{
			return (DDouble)(() => Math.Sin(value));
		}
		
		public static DDouble Tan(DDouble value)
		{
			return (DDouble)(() => Math.Tan(value));
		}

		public static DDouble Sinh(DDouble value)
		{
			return (DDouble)(() => Math.Sinh(value));
		}

		public static DDouble Tanh(DDouble value)
		{
			return (DDouble)(() => Math.Tanh(value));
		}

		public static DDouble Round(DDouble value)
		{
			return (DDouble)(() => Math.Round(value));
		}

		public static DDouble Truncate(DDouble value)
		{
			return (DDouble)(() => Math.Truncate(value));
		}

		public static DDouble Sqrt(DDouble value)
		{
			return (DDouble)(() => Math.Sqrt(value));
		}

		public static DDouble Log(DDouble value)
		{
			return (DDouble)(() => Math.Log(value));
		}

		public static DDouble Log10(DDouble value)
		{
			return (DDouble)(() => Math.Log10(value));
		}

		public static DDouble Exp(DDouble value)
		{
			return (DDouble)(() => Math.Exp(value));
		}

		public static DDouble Pow(DDouble value, DDouble value2)
		{
			return (DDouble)(() => Math.Pow(value, value2));
		}

		public static DInt Abs(DInt value)
		{
			return (DInt)(() => Math.Abs(value));
		}

		public static DDouble Abs(DDouble value)
		{
			return (DDouble)(() => Math.Abs(value));
		}

		public static DInt Max(DInt value, DInt value2)
		{
			return (DInt)(() => Math.Max(value, value2));
		}

		public static DDouble Max(DDouble value, DDouble value2)
		{
			return (DDouble)(() => Math.Max(value, value2));
		}

		public static DInt Min(DInt value, DInt value2)
		{
			return (DInt)(() => Math.Min(value, value2));
		}

		public static DDouble Min(DDouble value, DDouble value2)
		{
			return (DDouble)(() => Math.Min(value, value2));
		}

		public static DDouble Log(DDouble value, DDouble value2)
		{
			return (DDouble)(() => Math.Log(value, value2));
		}

		public static DInt Sign(DInt value)
		{
			return (DInt)(() => Math.Sign(value));
		}

		public static DInt Sign(DDouble value)
		{
			return (DInt)(() => Math.Sign(value));
		}
	}
}
