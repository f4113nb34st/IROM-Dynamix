namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// Dynamix int value class.
	/// </summary>
	public class DInt : DBasic<int>
	{
		public DInt(Func<int> src) : base(src){}
		
		protected internal override DElement<int> GetExtension()
		{
			return (DInt)(() => Value);
		}
		
		public static implicit operator DInt(Func<int> exp)
		{
			return new DInt(exp);
		}
		
		public static implicit operator DInt(int value)
		{
			return (DInt)(() => value);
		}
		
		public static explicit operator DInt(DDouble dd)
		{
			return (DInt)(() => (int)dd.Value);
		}
		
		public static DInt operator -(DInt ele)
		{
			return (DInt)(() => -ele.Value);
		}
		
		public static DInt operator ~(DInt ele)
		{
			return (DInt)(() => ~ele.Value);
		}
		
		public static DInt operator +(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value + ele2.Value);
		}
		
		public static DInt operator -(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value - ele2.Value);
		}
		
		public static DInt operator *(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value * ele2.Value);
		}
		
		public static DInt operator /(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value / ele2.Value);
		}
		
		public static DInt operator %(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value % ele2.Value);
		}
		
		public static DInt operator &(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value & ele2.Value);
		}
		
		public static DInt operator |(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value | ele2.Value);
		}
		
		public static DInt operator ^(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value ^ ele2.Value);
		}
		
		public static DInt LeftShift(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value << ele2.Value);
		}
		
		public static DInt RightShift(DInt ele, DInt ele2)
		{
			return (DInt)(() => ele.Value >> ele2.Value);
		}
		
		public override bool Equals(object obj)
        {
			return this == (obj as DElement<int>);
        }
		
		public static DBool operator ==(DInt ele, DElement<int> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DInt ele, DElement<int> ele2)
		{
			return !(ele == ele2);
		}
		
		public static DBool operator >(DInt ele, DElement<int> ele2)
		{
			return (DBool)(() => ele.Value > ele2.Value);
		}
		
		public static DBool operator <(DInt ele, DElement<int> ele2)
		{
			return (DBool)(() => ele.Value < ele2.Value);
		}
		
		public static DBool operator >=(DInt ele, DElement<int> ele2)
		{
			return (DBool)(() => ele.Value >= ele2.Value);
		}
		
		public static DBool operator <=(DInt ele, DElement<int> ele2)
		{
			return (DBool)(() => ele.Value <= ele2.Value);
		}
	}
}
