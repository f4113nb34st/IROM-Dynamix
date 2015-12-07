namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="DVec2D"/> value class.
	/// </summary>
	public class DVec2D : DElement<Vec2D>
	{
		//backing vars
		private DDouble x;
		private DDouble y;
		
		/// <summary>
        /// The x value.
        /// </summary>
        public DDouble X
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
        public DDouble Y
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
        
        public DVec2D(Func<double> xExp, Func<double> yExp){X = xExp; Y = yExp;}
        public DVec2D(DDouble x, DDouble y){X = x; Y = y;}
        
        protected override void Update()
		{
        	Value = new Vec2D(x.Value, y.Value);
		}
		
		protected internal override DElement<Vec2D> GetExtension()
		{
			return new DVec2D((() => Value.X), (() => Value.Y));
		}
        
        /// <summary>
        /// Accesses the given value of this vector.
        /// </summary>
        public DDouble this[int index]
        {
        	get
        	{
        		switch(index)
        		{
        			case 0: return X;
        			case 1: return Y;
        			default: throw new IndexOutOfRangeException(index + " out of Vec2D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Vec2D range.");
        		}
        	}
        }
        
        /// <summary>
        /// Returns the squared length of this <see cref="DVec2D"/>.
        /// </summary>
        /// <returns>The squared length.</returns>
        public DDouble LengthSq()
        {
        	return (X * X) + (Y * Y);
        }
        
        /// <summary>
        /// Returns the length of this <see cref="DVec2D"/>.
        /// </summary>
        /// <returns>The length.</returns>
        public DDouble Length()
        {
        	return DMath.Sqrt(LengthSq());
        }
        
        /// <summary>
        /// Returns the direction of this <see cref="DVec2D"/>.
        /// </summary>
        /// <returns>The direction <see cref="DVec2D"/></returns>
        public DVec2D Normalized()
        {
        	return this / Length();
        }
         
        /// <summary>
        /// Rotates this <see cref="DVec2D"/> the given number of radians.
        /// </summary>
        /// <param name="theta">The angle in radians.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public DVec2D Rotate(DDouble theta)
        {
        	DDouble cos = DMath.Cos(theta);
        	DDouble sin = DMath.Sin(theta);
        	return new DVec2D((X * cos) - (Y * sin), (X * sin) + (Y * cos));
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec2D(DDouble val)
        {
        	return new DVec2D(val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec2D(double val)
        {
        	return new DVec2D(val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec2D(int val)
        {
        	return new DVec2D(val, val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DPoint2D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator DPoint2D(DVec2D vec)
        {
        	return new DPoint2D((DInt)vec.X, (DInt)vec.Y);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DVec1D"/>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator DVec1D(DVec2D vec)
        {
            return new DVec1D(vec.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="DVec3D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator DVec3D(DVec2D vec)
        {
            return new DVec3D(vec.X, vec.Y, 0);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static DVec2D operator -(DVec2D vec)
        {
            return new DVec2D(-vec.X, -vec.Y);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static DVec2D operator +(DVec2D vec, DVec2D vec2)
        {
            return new DVec2D(vec.X + vec2.X, vec.Y + vec2.Y);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static DVec2D operator -(DVec2D vec, DVec2D vec2)
        {
            return new DVec2D(vec.X - vec2.X, vec.Y - vec2.Y);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static DVec2D operator *(DVec2D vec, DVec2D vec2)
        {
            return new DVec2D(vec.X * vec2.X, vec.Y * vec2.Y);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static DVec2D operator /(DVec2D vec, DVec2D vec2)
        {
            return new DVec2D(vec.X / vec2.X, vec.Y / vec2.Y);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static DVec2D operator %(DVec2D vec, DVec2D vec2)
        {
            return new DVec2D(vec.X % vec2.X, vec.Y % vec2.Y);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Vec2D>);
        }
		
		public static DBool operator ==(DVec2D ele, DElement<Vec2D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DVec2D ele, DElement<Vec2D> ele2)
		{
			return !(ele == ele2);
		}
		
		/// <summary>
        /// Performs the dot product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The dot product.</returns>
        public static DDouble Dot(DVec2D vec, DVec2D vec2)
        {
        	return vec.X * vec2.X + vec.Y * vec2.Y;
        }
	}
}
