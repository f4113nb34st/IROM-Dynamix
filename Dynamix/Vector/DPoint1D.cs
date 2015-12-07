namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="Point1D"/> value class.
	/// </summary>
	public class DPoint1D : DElement<Point1D>
	{
		//backing vars
		private DInt x;
		
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
        
        public DPoint1D(Func<int> xExp){X = xExp;}
        public DPoint1D(DInt x){X = x;}
        
		protected override void Update()
		{
			Value = new Point1D(x.Value);
		}
		
		protected internal override DElement<Point1D> GetExtension()
		{
			return new DPoint1D(() => Value.X);
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
        			default: throw new IndexOutOfRangeException(index + " out of Point1D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Point1D range.");
        		}
        	}
        }

        /// <summary>
        /// Implicit cast to the data type.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The value.</returns>
        public static implicit operator DInt(DPoint1D point)
        {
            return point.X;
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator DPoint1D(DInt val)
        {
        	return new DPoint1D(val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator DPoint1D(int val)
        {
        	return new DPoint1D(val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DVec1D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator DVec1D(DPoint1D point)
        {
        	return new DVec1D(point.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="DPoint2D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator DPoint2D(DPoint1D point)
        {
            return new DPoint2D(point.X, 0);
        }

		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static DPoint1D operator -(DPoint1D point)
        {
            return new DPoint1D(-point.X);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static DPoint1D operator +(DPoint1D point, DPoint1D point2)
        {
            return new DPoint1D(point.X + point2.X);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static DPoint1D operator -(DPoint1D point, DPoint1D point2)
        {
            return new DPoint1D(point.X - point2.X);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static DPoint1D operator *(DPoint1D point, DPoint1D point2)
        {
            return new DPoint1D(point.X * point2.X);
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static DPoint1D operator *(DPoint1D point, DVec1D value)
        {
        	return new DPoint1D((DInt)(point.X * value.X));
        }

        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint1D operator /(DPoint1D point, DPoint1D point2)
        {
            return new DPoint1D(point.X / point2.X);
        }
        
        /// <summary>
        /// Divides the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The quotient point.</returns>
        public static DPoint1D operator /(DPoint1D point, DVec1D value)
        {
        	return new DPoint1D((DInt)(point.X / value.X));
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static DPoint1D operator %(DPoint1D point, DPoint1D point2)
        {
            return new DPoint1D(point.X % point2.X);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Point1D>);
        }
		
		public static DBool operator ==(DPoint1D ele, DElement<Point1D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DPoint1D ele, DElement<Point1D> ele2)
		{
			return !(ele == ele2);
		}
	}
}
