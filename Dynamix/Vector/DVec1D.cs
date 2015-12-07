namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="Vec1D"/> value class.
	/// </summary>
	public class DVec1D : DElement<Vec1D>
	{
		//backing vars
		private DDouble x;
		
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
        
        public DVec1D(Func<double> xExp){X = xExp;}
        public DVec1D(DDouble x){X = x;}
        
		protected override void Update()
		{
			Value = new Vec1D(x.Value);
		}
		
		protected internal override DElement<Vec1D> GetExtension()
		{
			return new DVec1D(() => Value.X);
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
        			default: throw new IndexOutOfRangeException(index + " out of Vec1D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Vec1D range.");
        		}
        	}
        }

        /// <summary>
        /// Implicit cast to the data type.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The value.</returns>
        public static implicit operator DDouble(DVec1D vec)
        {
            return vec.X;
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec1D(DDouble val)
        {
        	return new DVec1D(val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec1D(double val)
        {
        	return new DVec1D(val);
        }
        
        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator DVec1D(int val)
        {
        	return new DVec1D(val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="DPoint1D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator DPoint1D(DVec1D vec)
        {
        	return new DPoint1D((DInt)vec.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="DVec2D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator DVec2D(DVec1D vec)
        {
            return new DVec2D(vec.X, 0);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static DVec1D operator -(DVec1D vec)
        {
            return new DVec1D(-vec.X);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static DVec1D operator +(DVec1D vec, DVec1D vec2)
        {
            return new DVec1D(vec.X + vec2.X);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static DVec1D operator -(DVec1D vec, DVec1D vec2)
        {
            return new DVec1D(vec.X - vec2.X);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static DVec1D operator *(DVec1D vec, DVec1D vec2)
        {
            return new DVec1D(vec.X * vec2.X);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static DVec1D operator /(DVec1D vec, DVec1D vec2)
        {
            return new DVec1D(vec.X / vec2.X);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static DVec1D operator %(DVec1D vec, DVec1D vec2)
        {
            return new DVec1D(vec.X % vec2.X);
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Vec1D>);
        }
		
		public static DBool operator ==(DVec1D ele, DElement<Vec1D> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DVec1D ele, DElement<Vec1D> ele2)
		{
			return !(ele == ele2);
		}
	}
}
