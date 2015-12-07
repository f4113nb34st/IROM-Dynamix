namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="DPoint4D"/> value class.
	/// </summary>
	public class DPoint4D : DElement<Point4D>
	{
		//backing vars
		private DInt x;
		private DInt y;
		private DInt z;
		private DInt w;
		
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
        
        /// <summary>
        /// The w value.
        /// </summary>
        public DInt W
        {
        	get
        	{
        		return w;
        	}
        	set
        	{
        		if(w != null) w.Unsubscribe(OnChange);
        		w = value;
        		w.Subscribe(OnChange);
        		OnChange();
        	}
        }
        
        public DPoint4D(Func<int> xExp, Func<int> yExp, Func<int> zExp, Func<int> wExp){X = xExp; Y = yExp; Z = zExp; W = wExp;}
        public DPoint4D(DInt x, DInt y, DInt z, DInt w){X = x; Y = y; Z = z; W = w;}
        
        protected override void Update()
		{
        	Value = new Point4D(x.Value, y.Value, z.Value, w.Value);
		}
		
		protected internal override DElement<Point4D> GetExtension()
		{
			return new DPoint4D((() => Value.X), (() => Value.Y), (() => Value.Z), (() => Value.W));
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
        			case 3: return W;
        			default: throw new IndexOutOfRangeException(index + " out of Point4D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			case 2: Z = value; break;
        			case 3: W = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Point4D range.");
        		}
        	}
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The value.</returns>
        public static implicit operator DPoint4D(Point4D point)
        {
        	return new DPoint4D(() => point.X, () => point.Y, () => point.Z, () => point.W);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator DPoint4D(DInt val)
        {
        	return new DPoint4D(val, val, val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator DPoint4D(int val)
        {
        	return new DPoint4D(val, val, val, val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DVec4D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator DVec4D(DPoint4D point)
        {
        	return new DVec4D(point.X, point.Y, point.Z, point.W);
        }
        
        /// <summary>
        /// Implicit cast to <see cref="DPoint3D"/>. Drops extra data.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator DPoint3D(DPoint4D point)
        {
            return new DPoint3D(point.X, point.Y, point.Z);
        }
        
        /// <summary>
        /// Implicit cast to <see cref="Vec4D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec4D(DPoint4D vec)
        {
        	return vec.Value;
        }

		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static DPoint4D operator -(DPoint4D point)
        {
            return new DPoint4D(-point.X, -point.Y, -point.Z, -point.W);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static DPoint4D operator +(DPoint4D point, DPoint4D point2)
        {
            return new DPoint4D(point.X + point2.X, point.Y + point2.Y, point.Z + point2.Z, point.W + point2.W);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static DPoint4D operator -(DPoint4D point, DPoint4D point2)
        {
            return new DPoint4D(point.X - point2.X, point.Y - point2.Y, point.Z - point2.Z, point.W - point2.W);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static DPoint4D operator *(DPoint4D point, DPoint4D point2)
        {
            return new DPoint4D(point.X * point2.X, point.Y * point2.Y, point.Z * point2.Z, point.W * point2.W);
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static DPoint4D operator *(DPoint4D point, DVec4D value)
        {
        	return new DPoint4D((DInt)(point.X * value.X), (DInt)(point.Y * value.Y), (DInt)(point.Z * value.Z), (DInt)(point.W * value.W));
        }

        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint4D operator /(DPoint4D point, DPoint4D point2)
        {
            return new DPoint4D(point.X / point2.X, point.Y / point2.Y, point.Z / point2.Z, point.W / point2.W);
        }
        
        /// <summary>
        /// Divides the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint4D operator /(DPoint4D point, DVec4D value)
        {
        	return new DPoint4D((DInt)(point.X / value.X), (DInt)(point.Y / value.Y), (DInt)(point.Z / value.Z), (DInt)(point.W / value.W));
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static DPoint4D operator %(DPoint4D point, DPoint4D point2)
        {
            return new DPoint4D(point.X % point2.X, point.Y % point2.Y, point.Z % point2.Z, point.W % point2.W);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Point4D>);
        }
		
		public static DBool operator ==(DPoint4D ele, DElement<Point4D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DPoint4D ele, DElement<Point4D> ele2)
		{
			return !(ele == ele2);
		}
	}
}
