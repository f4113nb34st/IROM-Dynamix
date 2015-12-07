namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="DPoint3D"/> value class.
	/// </summary>
	public class DPoint3D : DElement<Point3D>
	{
		//backing vars
		private DInt x;
		private DInt y;
		private DInt z;
		
		/// <summary>
        /// The x value.
        /// </summary>
        public DInt X
        {
        	get
        	{
        		return x;
        	}
        	set
        	{
        		if(x != null) x.Unsubscribe(OnChange);
        		x = value;
        		x.Subscribe(OnChange);
        		OnChange();
        	}
        }
        
        /// <summary>
        /// The y value.
        /// </summary>
        public DInt Y
        {
        	get
        	{
        		return y;
        	}
        	set
        	{
        		if(y != null) y.Unsubscribe(OnChange);
        		y = value;
        		y.Subscribe(OnChange);
        		OnChange();
        	}
        }
        
        /// <summary>
        /// The z value.
        /// </summary>
        public DInt Z
        {
        	get
        	{
        		return z;
        	}
        	set
        	{
        		if(z != null) z.Unsubscribe(OnChange);
        		z = value;
        		z.Subscribe(OnChange);
        		OnChange();
        	}
        }
        
        public DPoint3D(Func<int> xExp, Func<int> yExp, Func<int> zExp){X = xExp; Y = yExp; Z = zExp;}
        public DPoint3D(DInt x, DInt y, DInt z){X = x; Y = y; Z = z;}
        
        protected override void Update()
		{
        	Value = new Point3D(x.Value, y.Value, z.Value);
		}
		
		protected internal override DElement<Point3D> GetExtension()
		{
			return new DPoint3D((() => Value.X), (() => Value.Y), (() => Value.Z));
		}
        
        /// <summary>
        /// Accesses the given value of this point.
        /// </summary>
        public DInt this[int index]
        {
        	get
        	{
        		switch(index)
        		{
        			case 0: return X;
        			case 1: return Y;
        			case 2: return Z;
        			default: throw new IndexOutOfRangeException(index + " out of Point3D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			case 2: Z = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Point3D range.");
        		}
        	}
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator DPoint3D(DInt val)
        {
        	return new DPoint3D(val, val, val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DVec3D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator DVec3D(DPoint3D point)
        {
        	return new DVec3D(point.X, point.Y, point.Z);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DPoint2D"/>. Drops extra data.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator DPoint2D(DPoint3D point)
        {
            return new DPoint2D(point.X, point.Y);
        }

        /// <summary>
        /// Explicit cast to <see cref="DPoint4D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator DPoint4D(DPoint3D point)
        {
            return new DPoint4D(point.X, point.Y, point.Z, 0);
        }

		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static DPoint3D operator -(DPoint3D point)
        {
            return new DPoint3D(-point.X, -point.Y, -point.Z);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static DPoint3D operator +(DPoint3D point, DPoint3D point2)
        {
            return new DPoint3D(point.X + point2.X, point.Y + point2.Y, point.Z + point2.Z);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static DPoint3D operator -(DPoint3D point, DPoint3D point2)
        {
            return new DPoint3D(point.X - point2.X, point.Y - point2.Y, point.Z - point2.Z);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static DPoint3D operator *(DPoint3D point, DPoint3D point2)
        {
            return new DPoint3D(point.X * point2.X, point.Y * point2.Y, point.Z * point2.Z);
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static DPoint3D operator *(DPoint3D point, DVec3D value)
        {
        	return new DPoint3D((DInt)(point.X * value.X), (DInt)(point.Y * value.Y), (DInt)(point.Z * value.Z));
        }

        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint3D operator /(DPoint3D point, DPoint3D point2)
        {
            return new DPoint3D(point.X / point2.X, point.Y / point2.Y, point.Z / point2.Z);
        }
        
        /// <summary>
        /// Divides the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint3D operator /(DPoint3D point, DVec3D value)
        {
        	return new DPoint3D((DInt)(point.X / value.X), (DInt)(point.Y / value.Y), (DInt)(point.Z / value.Z));
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static DPoint3D operator %(DPoint3D point, DPoint3D point2)
        {
            return new DPoint3D(point.X % point2.X, point.Y % point2.Y, point.Z % point2.Z);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Point3D>);
        }
		
		public static DBool operator ==(DPoint3D ele, DElement<Point3D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DPoint3D ele, DElement<Point3D> ele2)
		{
			return !(ele == ele2);
		}
	}
}
