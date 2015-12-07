namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="DPoint2D"/> value class.
	/// </summary>
	public class DPoint2D : DElement<Point2D>
	{
		//backing vars
		private DInt x;
		private DInt y;
		
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
        
        public DPoint2D(Func<int> xExp, Func<int> yExp){X = xExp; Y = yExp;}
        public DPoint2D(DInt x, DInt y){X = x; Y = y;}
        
        protected override void Update()
		{
        	Value = new Point2D(x.Value, y.Value);
		}
		
		protected internal override DElement<Point2D> GetExtension()
		{
			return new DPoint2D((() => Value.X), (() => Value.Y));
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
        			default: throw new IndexOutOfRangeException(index + " out of Point2D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Point2D range.");
        		}
        	}
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator DPoint2D(DInt val)
        {
        	return new DPoint2D(val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator DPoint2D(int val)
        {
        	return new DPoint2D(val, val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DVec2D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator DVec2D(DPoint2D point)
        {
        	return new DVec2D(point.X, point.Y);
        }
        
        /// <summary>
        /// Implicit cast to <see cref="DPoint1D"/>. Drops extra data.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator DPoint1D(DPoint2D point)
        {
            return new DPoint1D(point.X);
        }

        /// <summary>
        /// Implicit cast to <see cref="DPoint3D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator DPoint3D(DPoint2D point)
        {
            return new DPoint3D(point.X, point.Y, 0);
        }
        
        /// <summary>
        /// Implicit cast to <see cref="Vec2D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec2D(DPoint2D vec)
        {
        	return vec.Value;
        }

		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static DPoint2D operator -(DPoint2D point)
        {
            return new DPoint2D(-point.X, -point.Y);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static DPoint2D operator +(DPoint2D point, DPoint2D point2)
        {
            return new DPoint2D(point.X + point2.X, point.Y + point2.Y);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static DPoint2D operator -(DPoint2D point, DPoint2D point2)
        {
            return new DPoint2D(point.X - point2.X, point.Y - point2.Y);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static DPoint2D operator *(DPoint2D point, DPoint2D point2)
        {
            return new DPoint2D(point.X * point2.X, point.Y * point2.Y);
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static DPoint2D operator *(DPoint2D point, DVec2D value)
        {
        	return new DPoint2D((DInt)(point.X * value.X), (DInt)(point.Y * value.Y));
        }

        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint2D operator /(DPoint2D point, DPoint2D point2)
        {
            return new DPoint2D(point.X / point2.X, point.Y / point2.Y);
        }
        
        /// <summary>
        /// Divides the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint2D operator /(DPoint2D point, DVec2D value)
        {
        	return new DPoint2D((DInt)(point.X / value.X), (DInt)(point.Y / value.Y));
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static DPoint2D operator %(DPoint2D point, DPoint2D point2)
        {
            return new DPoint2D(point.X % point2.X, point.Y % point2.Y);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Point2D>);
        }
		
		public static DBool operator ==(DPoint2D ele, DElement<Point2D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DPoint2D ele, DElement<Point2D> ele2)
		{
			return !(ele == ele2);
		}
	}
}
