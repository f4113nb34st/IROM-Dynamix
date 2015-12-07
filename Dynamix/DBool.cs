namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// Dynamix boolean value class.
	/// </summary>
	public class DBool : DBasic<bool>
	{
		public DBool(Func<bool> src) : base(src){}
		
		protected internal override DElement<bool> GetExtension()
		{
			return (DBool)(() => Value);
		}
		
		public static implicit operator DBool(Func<bool> exp)
		{
			return new DBool(exp);
		}
		
		public static implicit operator DBool(bool value)
		{
			return (DBool)(() => value);
		}
		
		public static bool operator true(DBool ele)
		{
			return ele.Value;
		}
		
		public static bool operator false(DBool ele)
		{
			return !ele.Value;
		}
		
		public static DBool operator !(DBool ele)
		{
			return (DBool)(() => !ele.Value);
		}
		
		public static DBool operator &(DBool ele, DBool ele2)
		{
			return (DBool)(() => ele.Value & ele2.Value);
		}
		
		public static DBool operator |(DBool ele, DBool ele2)
		{
			return (DBool)(() => ele.Value | ele2.Value);
		}
		
		public static DBool operator ^(DBool ele, DBool ele2)
		{
			return (DBool)(() => ele.Value ^ ele2.Value);
		}
		
		public override bool Equals(object obj)
        {
			return this == (obj as DElement<bool>);
        }
		
		public static DBool operator ==(DBool ele, DElement<bool> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DBool ele, DElement<bool> ele2)
		{
			return !(ele == ele2);
		}
	}
}
