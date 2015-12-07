namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix double value class.
	/// </summary>
	public class DDouble : DBasic<double>
	{
		public DDouble(Func<double> src) : base(src){}
		
		protected internal override DElement<double> GetExtension()
		{
			return (DDouble)(() => Value);
		}
		
		public static implicit operator DDouble(Func<double> exp)
		{
			return new DDouble(exp);
		}
		
		public static implicit operator DDouble(double value)
		{
			return (DDouble)(() => value);
		}
		
		public static implicit operator DDouble(int value)
		{
			return (DDouble)(() => (double)value);
		}
		
		public static implicit operator DDouble(DInt dd)
		{
			return (DDouble)(() => (double)dd.Value);
		}
		
		public static DDouble operator -(DDouble ele)
		{
			return (DDouble)(() => -ele.Value);
		}
		
		public static DDouble operator +(DDouble ele, DDouble ele2)
		{
			return (DDouble)(() => ele.Value + ele2.Value);
		}
		
		public static DDouble operator -(DDouble ele, DDouble ele2)
		{
			return (DDouble)(() => ele.Value - ele2.Value);
		}
		
		public static DDouble operator *(DDouble ele, DDouble ele2)
		{
			return (DDouble)(() => ele.Value * ele2.Value);
		}
		
		public static DDouble operator /(DDouble ele, DDouble ele2)
		{
			return (DDouble)(() => ele.Value / ele2.Value);
		}
		
		public static DDouble operator %(DDouble ele, DDouble ele2)
		{
			return (DDouble)(() => ele.Value % ele2.Value);
		}
		
		public override bool Equals(object obj)
        {
			return this == (obj as DElement<double>);
        }
		
		public static DBool operator ==(DDouble ele, DElement<double> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DDouble ele, DElement<double> ele2)
		{
			return !(ele == ele2);
		}
		
		public static DBool operator >(DDouble ele, DElement<double> ele2)
		{
			return (DBool)(() => ele.Value > ele2.Value);
		}
		
		public static DBool operator <(DDouble ele, DElement<double> ele2)
		{
			return (DBool)(() => ele.Value < ele2.Value);
		}
		
		public static DBool operator >=(DDouble ele, DElement<double> ele2)
		{
			return (DBool)(() => ele.Value >= ele2.Value);
		}
		
		public static DBool operator <=(DDouble ele, DElement<double> ele2)
		{
			return (DBool)(() => ele.Value <= ele2.Value);
		}
		
		static DDouble()
		{
			AutoConfig.SetParser<DDouble>((string str, out DDouble result) =>
            {
            	double temp;
            	bool success = double.TryParse(str, out temp);
            	result = temp;
            	return success;
            });
		}
	}
}
