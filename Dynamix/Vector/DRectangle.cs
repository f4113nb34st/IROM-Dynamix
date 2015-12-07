namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="Rectangle"/> value class.
	/// </summary>
	public class DRectangle : DElement<Rectangle>
	{
		//backing vars
		private DPoint2D min;
		private DPoint2D max;
		
		/// <summary>
		/// The <see cref="DRectangle"/> minimum coordinates (inclusive).
		/// </summary>
		public DPoint2D Min
		{
			get
        	{
        		return min;
        	}
        	set
        	{
        		if(min != null) min.Unsubscribe(OnChange);
        		min = value;
        		min.Subscribe(OnChange);
        		OnChange();
        	}
		}
		
		/// <summary>
		/// The <see cref="DRectangle"/> maximum coordinates (inclusive).
		/// </summary>
		public DPoint2D Max
		{
			get
        	{
        		return max;
        	}
        	set
        	{
        		if(max != null) max.Unsubscribe(OnChange);
        		max = value;
        		max.Subscribe(OnChange);
        		OnChange();
        	}
		}
		
		/// <summary>
		/// The <see cref="DRectangle"/> position.
		/// </summary>
		public DPoint2D Position
		{
			get{return Min;}
			set
			{
				Max -= Min;
				Min = value;
				Max += Min;
			}
		}
		
		/// <summary>
		/// The <see cref="DRectangle"/> size.
		/// </summary>
		public DPoint2D Size
		{
			get{return Max - Min + (DInt)1;}
			set{Max = Min + value - (DInt)1;}
		}
		
		/// <summary>
		/// The <see cref="DRectangle"/> x.
		/// </summary>
		public DInt X
		{
			get{return Min.X;}
			set
			{
				Max.X -= Min.X;
				Min.X = value;
				Max.X += Min.X;
			}
		}
		
		/// <summary>
		/// The <see cref="DRectangle"/> y.
		/// </summary>
		public DInt Y
		{
			get{return Min.Y;}
			set
			{
				Max.Y -= Min.Y;
				Min.Y = value;
				Max.Y += Min.Y;
			}
		}
		
		/// <summary>
		/// The <see cref="DRectangle"/> width.
		/// </summary>
		public DInt Width
		{
			get{return Max.X - Min.X + 1;}
			set{Max.X = Min.X + value - 1;}
		}
		
		/// <summary>
		/// The <see cref="DRectangle"/> height.
		/// </summary>
		public DInt Height
		{
			get{return Max.Y - Min.Y + 1;}
			set{Max.Y = Min.Y + value - 1;}
		}
		
		/// <summary>
        /// Creates a new <see cref="DRectangle"/> with the given values.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        public DRectangle(DInt x, DInt y, DInt w, DInt h) : this(new DPoint2D(x, y), new DPoint2D(x + (w - 1), y + (h - 1)))
        {
        	
        }
        
        /// <summary>
        /// Creates a new <see cref="DRectangle"/> with the given values.
        /// </summary>
        /// <param name="min">The minimum coordinates.</param>
        /// <param name="max">The maximum coordinates.</param>
        public DRectangle(DPoint2D min, DPoint2D max)
        {
        	Min = min;
        	Max = max;
        }
        
        protected override void Update()
		{
			Value = new Rectangle(min.Value, max.Value);
		}
		
		protected internal override DElement<Rectangle> GetExtension()
		{
			return new DRectangle(new DPoint2D(() => Value.Min.X, () => Value.Min.Y), new DPoint2D(() => Value.Max.X, () => Value.Max.Y));
		}
        
        /// <summary>
        /// Returns true if this <see cref="DRectangle"/> is a valid, space-filling rectangle.
        /// </summary>
        /// <returns>True if valid.</returns>
        public DBool IsValid()
        {
        	return Min.X <= Max.X && Min.Y <= Max.Y;
        }
        
        /// <summary>
        /// Ensures this <see cref="DRectangle"/> is at least the given size.
        /// </summary>
        /// <param name="width">The min allowed width.</param>
        /// <param name="height">The min allowed height.</param>
        public void EnsureSize(DInt width, DInt height)
        {
        	EnsureSize(new DPoint2D(width, height));
        }
        
        /// <summary>
        /// Ensures this <see cref="DRectangle"/> is at least the given size.
        /// </summary>
        /// <param name="size">The min allowed size.</param>
        public void EnsureSize(DPoint2D size)
        {
        	Size = DVectorUtil.Max(Size, size);
        }
        
        /// <summary>
        /// Returns true if the given <see cref="DPoint2D"/> is within this <see cref="DRectangle"/>'s bounds.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <returns>True if within, else false.</returns>
        public DBool Contains(DPoint2D vec)
        {
        	return vec.X >= Min.X && vec.Y >= Min.Y && vec.X <= Max.X && vec.Y <= Max.Y;
        }
        
        /// <summary>
        /// Explicit cast to <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rect to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static implicit operator System.Drawing.Rectangle(DRectangle rect)
        {
        	return new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        
        /// <summary>
        /// Explicit cast from <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rect to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static implicit operator DRectangle(System.Drawing.Rectangle rect)
        {
        	return new DRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        
		/// <summary>
        /// Implicit cast from <see cref="Point2D"/>.
        /// Resulting <see cref="Rectangle"/> extends into quadrant 1 from the origin.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static explicit operator DRectangle(DPoint2D vec)
        {
        	return new DRectangle(0, 0, vec.X, vec.Y);
        }
        
        /// <summary>
        /// Adds the given vec to the rect's position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved rectangle.</returns>
        public static DRectangle operator +(DRectangle rect, DPoint2D vec)
        {
        	//passed by value, so we can just add vec to rect pos and pass it back
        	rect.Position += vec;
            return rect;
        }

        /// <summary>
        /// Subtracts the given vec from the rect's position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved rectangle.</returns>
        public static DRectangle operator -(DRectangle rect, DPoint2D vec)
        {
        	//passed by value, so we can just add vec to rect pos and pass it back
        	rect.Position -= vec;
            return rect;
        }

        /// <summary>
        /// Multiplies the given rect's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized rectangle.</returns>
        public static DRectangle operator *(DRectangle rect, DDouble val)
        {
        	DPoint2D middle = (rect.Max + rect.Min) / (DDouble)2.0;
        	rect.Min -= middle;
        	rect.Min *= val;
        	rect.Min += middle;
        	rect.Max -= middle;
        	rect.Max *= val;
        	rect.Max += middle;
            return rect;
        }

        /// <summary>
        /// Divided the given rect's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized rectangle.</returns>
        public static DRectangle operator /(DRectangle rect, DDouble val)
        {
        	DPoint2D middle = (rect.Max + rect.Min) / (DDouble)2.0;
        	rect.Min -= middle;
        	rect.Min /= val;
        	rect.Min += middle;
        	rect.Max -= middle;
        	rect.Max /= val;
        	rect.Max += middle;
            return rect;
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Rectangle>);
        }
		
		public static DBool operator ==(DRectangle ele, DElement<Rectangle> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DRectangle ele, DElement<Rectangle> ele2)
		{
			return !(ele == ele2);
		}
	}
}