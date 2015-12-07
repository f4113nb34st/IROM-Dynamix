namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="DVec3D"/> value class.
	/// </summary>
	public class DVec3D : DElement<Vec3D>
	{
		//backing vars
		private DDouble x;
		private DDouble y;
		private DDouble z;
		
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
        
        public DVec3D(Func<double> xExp, Func<double> yExp, Func<double> zExp){X = xExp; Y = yExp; Z = zExp;}
        public DVec3D(DDouble x, DDouble y, DDouble z){X = x; Y = y; Z = z;}
        
        protected override void Update()
		{
        	Value = new Vec3D(x.Value, y.Value, z.Value);
		}
		
		protected internal override DElement<Vec3D> GetExtension()
		{
			return new DVec3D((() => Value.X), (() => Value.Y), (() => Value.Z));
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
        			default: throw new IndexOutOfRangeException(index + " out of Vec3D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			case 2: Z = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Vec3D range.");
        		}
        	}
        }
        
        /// <summary>
        /// Returns the squared length of this <see cref="DVec3D"/>.
        /// </summary>
        /// <returns>The squared length.</returns>
        public DDouble LengthSq()
        {
        	return (X * X) + (Y * Y) + (Z * Z);
        }
        
        /// <summary>
        /// Returns the length of this <see cref="DVec3D"/>.
        /// </summary>
        /// <returns>The length.</returns>
        public DDouble Length()
        {
        	return DMath.Sqrt(LengthSq());
        }
        
        /// <summary>
        /// Returns the direction of this <see cref="DVec3D"/>.
        /// </summary>
        /// <returns>The direction <see cref="DVec3D"/></returns>
        public DVec3D Normalized()
        {
        	return this / Length();
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec3D(DDouble val)
        {
        	return new DVec3D(val, val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec3D(double val)
        {
        	return new DVec3D(val, val, val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec3D(int val)
        {
        	return new DVec3D(val, val, val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DPoint3D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator DPoint3D(DVec3D vec)
        {
        	return new DPoint3D((DInt)vec.X, (DInt)vec.Y, (DInt)vec.Z);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DVec2D"/>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator DVec2D(DVec3D vec)
        {
            return new DVec2D(vec.X, vec.Y);
        }

        /// <summary>
        /// Explicit cast to <see cref="DVec4D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator DVec4D(DVec3D vec)
        {
            return new DVec4D(vec.X, vec.Y, vec.Z, 0);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static DVec3D operator -(DVec3D vec)
        {
            return new DVec3D(-vec.X, -vec.Y, -vec.Z);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static DVec3D operator +(DVec3D vec, DVec3D vec2)
        {
            return new DVec3D(vec.X + vec2.X, vec.Y + vec2.Y, vec.Z + vec2.Z);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static DVec3D operator -(DVec3D vec, DVec3D vec2)
        {
            return new DVec3D(vec.X - vec2.X, vec.Y - vec2.Y, vec.Z - vec2.Z);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static DVec3D operator *(DVec3D vec, DVec3D vec2)
        {
            return new DVec3D(vec.X * vec2.X, vec.Y * vec2.Y, vec.Z * vec2.Z);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static DVec3D operator /(DVec3D vec, DVec3D vec2)
        {
            return new DVec3D(vec.X / vec2.X, vec.Y / vec2.Y, vec.Z / vec2.Z);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static DVec3D operator %(DVec3D vec, DVec3D vec2)
        {
            return new DVec3D(vec.X % vec2.X, vec.Y % vec2.Y, vec.Z % vec2.Z);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Vec3D>);
        }
		
		public static DBool operator ==(DVec3D ele, DElement<Vec3D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DVec3D ele, DElement<Vec3D> ele2)
		{
			return !(ele == ele2);
		}
        
        /// <summary>
        /// Performs the dot product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The dot product.</returns>
        public static DDouble Dot(DVec3D vec, DVec3D vec2)
        {
        	return vec.X * vec2.X + vec.Y * vec2.Y + vec.Z * vec2.Z;
        }
        
        /// <summary>
        /// Performs the cross product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The cross product.</returns>
        public static DVec3D Cross(DVec3D vec, DVec3D vec2)
        {
        	return new DVec3D((vec.Y * vec2.Z) - (vec.Z * vec2.Y), (vec.Z * vec2.X) - (vec.X * vec2.Z), (vec.X * vec2.Y) - (vec.Y * vec2.X));
        }
	}
}
