namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix string value class.
	/// </summary>
	public class DString : DBasic<string>
	{
		public DString(Func<string> src) : base(src){}
		
		protected internal override DElement<string> GetExtension()
		{
			return (DString)(() => Value);
		}
		
		public static implicit operator DString(Func<string> exp)
		{
			return new DString(exp);
		}
		
		public static implicit operator DString(string value)
		{
			return (DString)(() => value);
		}
		
		public override bool Equals(object obj)
        {
			return this == (obj as DElement<string>);
        }
		
		public static DBool operator ==(DString ele, DElement<string> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DString ele, DElement<string> ele2)
		{
			return !(ele == ele2);
		}
		
		static DString()
		{
			AutoConfig.SetParser<DString>((string str, out DString result) =>
            {
			    result = str;
            	return true;
            });
		}
	}
}
