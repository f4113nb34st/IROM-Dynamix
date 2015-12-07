namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="DVec4D"/> value class.
	/// </summary>
	public class DVec4D : DElement<Vec4D>
	{
		//backing vars
		private DDouble x;
		private DDouble y;
		private DDouble z;
		private DDouble w;
		
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
        
        /// <summary>
        /// The z value.
        /// </summary>
        public DDouble Z
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
        public DDouble W
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
        
        public DVec4D(Func<double> xExp, Func<double> yExp, Func<double> zExp, Func<double> wExp){X = xExp; Y = yExp; Z = zExp; W = wExp;}
        public DVec4D(DDouble x, DDouble y, DDouble z, DDouble w){X = x; Y = y; Z = z; W = w;}
        
        protected override void Update()
		{
        	Value = new Vec4D(x.Value, y.Value, z.Value, w.Value);
		}
		
		protected internal override DElement<Vec4D> GetExtension()
		{
			return new DVec4D((() => Value.X), (() => Value.Y), (() => Value.Z), (() => Value.W));
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
        			case 2: return Z;
        			case 3: return W;
        			default: throw new IndexOutOfRangeException(index + " out of Vec4D range.");
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
        			default: throw new IndexOutOfRangeException(index + " out of Vec4D range.");
        		}
        	}
        }
        
        /// <summary>
        /// Returns the squared length of this <see cref="DVec4D"/>.
        /// </summary>
        /// <returns>The squared length.</returns>
        public DDouble LengthSq()
        {
        	return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }
        
        /// <summary>
        /// Returns the length of this <see cref="DVec4D"/>.
        /// </summary>
        /// <returns>The length.</returns>
        public DDouble Length()
        {
        	return DMath.Sqrt(LengthSq());
        }
        
        /// <summary>
        /// Returns the direction of this <see cref="DVec4D"/>.
        /// </summary>
        /// <returns>The direction <see cref="DVec4D"/></returns>
        public DVec4D Normalized()
        {
        	return this / Length();
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec4D(DDouble val)
        {
        	return new DVec4D(val, val, val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec4D(double val)
        {
        	return new DVec4D(val, val, val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec4D(int val)
        {
        	return new DVec4D(val, val, val, val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DPoint4D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator DPoint4D(DVec4D vec)
        {
        	return new DPoint4D((DInt)vec.X, (DInt)vec.Y, (DInt)vec.Z, (DInt)vec.W);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DVec3D"/>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator DVec3D(DVec4D vec)
        {
            return new DVec3D(vec.X, vec.Y, vec.Z);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static DVec4D operator -(DVec4D vec)
        {
            return new DVec4D(-vec.X, -vec.Y, -vec.Z, -vec.W);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static DVec4D operator +(DVec4D vec, DVec4D vec2)
        {
            return new DVec4D(vec.X + vec2.X, vec.Y + vec2.Y, vec.Z + vec2.Z, vec.W + vec2.W);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static DVec4D operator -(DVec4D vec, DVec4D vec2)
        {
            return new DVec4D(vec.X - vec2.X, vec.Y - vec2.Y, vec.Z - vec2.Z, vec.W - vec2.W);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static DVec4D operator *(DVec4D vec, DVec4D vec2)
        {
            return new DVec4D(vec.X * vec2.X, vec.Y * vec2.Y, vec.Z * vec2.Z, vec.W * vec2.W);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static DVec4D operator /(DVec4D vec, DVec4D vec2)
        {
            return new DVec4D(vec.X / vec2.X, vec.Y / vec2.Y, vec.Z / vec2.Z, vec.W / vec2.W);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static DVec4D operator %(DVec4D vec, DVec4D vec2)
        {
            return new DVec4D(vec.X % vec2.X, vec.Y % vec2.Y, vec.Z % vec2.Z, vec.W % vec2.W);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Vec4D>);
        }
		
		public static DBool operator ==(DVec4D ele, DElement<Vec4D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DVec4D ele, DElement<Vec4D> ele2)
		{
			return !(ele == ele2);
		}
        
        /// <summary>
        /// Performs the dot product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The dot product.</returns>
        public static DDouble Dot(DVec4D vec, DVec4D vec2)
        {
        	return vec.X * vec2.X + vec.Y * vec2.Y + vec.Z * vec2.Z + vec.W * vec2.W;
        }
	}
}
