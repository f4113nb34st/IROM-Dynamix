namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="ARGB"/> value class.
	/// </summary>
	public class DARGB : DBasic<ARGB>
	{
		/// <summary>
        /// Returns the greyscale version of this <see cref="RGB"/>
        /// </summary>
        public DARGB Greyscale
        {
        	get
        	{
        		return (DARGB)(() => Value.Greyscale);
        	}
        }
		
		public DARGB(Func<ARGB> src) : base(src){}
		
		protected internal override DElement<ARGB> GetExtension()
		{
			return (DARGB)(() => Value);
		}
		
		public static implicit operator DARGB(Func<ARGB> exp)
		{
			return new DARGB(exp);
		}
		
		public static implicit operator DARGB(ARGB value)
		{
			return (DARGB)(() => value);
		}
		
		public static implicit operator DARGB(RGB value)
		{
			return (DARGB)(() => value);
		}
		
		public static DARGB operator +(DARGB ele, DARGB ele2)
		{
			return (DARGB)(() => ele.Value + ele2.Value);
		}
		
		public static DARGB operator -(DARGB ele, DARGB ele2)
		{
			return (DARGB)(() => ele.Value - ele2.Value);
		}
		
		public static DARGB operator *(DARGB ele, DDouble ele2)
		{
			return (DARGB)(() => ele.Value * ele2.Value);
		}
		
		public static DARGB operator *(DARGB ele, DVec4D ele2)
		{
			return (DARGB)(() => ele.Value * ele2.Value);
		}
		
		public static DARGB operator /(DARGB ele, DDouble ele2)
		{
			return (DARGB)(() => ele.Value / ele2.Value);
		}
		
		public static DARGB operator /(DARGB ele, DVec4D ele2)
		{
			return (DARGB)(() => ele.Value / ele2.Value);
		}
		
		public static DARGB operator &(DARGB ele, DARGB ele2)
		{
			return (DARGB)(() => ele.Value & ele2.Value);
		}
		
		public override bool Equals(object obj)
        {
			return this == (obj as DElement<ARGB>);
        }
		
		public static DBool operator ==(DARGB ele, DElement<ARGB> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DARGB ele, DElement<ARGB> ele2)
		{
			return !(ele == ele2);
		}
		
		static DARGB()
		{
			AutoConfig.SetParser<DARGB>((string str, out DARGB result) =>
            {
            	ARGB temp;
            	bool success = ARGB.TryParse(str, out temp);
            	result = temp;
            	return success;
            });
		}
	}
}
