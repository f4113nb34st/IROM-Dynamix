namespace IROM.Dynamix
{
	using System;
	using IROM.Util;
	
	/// <summary>
	/// Dynamix <see cref="Viewport"/> value class.
	/// </summary>
	public class DViewport : DElement<Viewport>
	{
		//backing vars
		private DVec2D min;
		private DVec2D max;
		
		/// <summary>
		/// The <see cref="DViewport"/> minimum coordinates (inclusive).
		/// </summary>
		public DVec2D Min
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
		/// The <see cref="DViewport"/> maximum coordinates (exclusive).
		/// </summary>
		public DVec2D Max
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
		/// The <see cref="DViewport"/> position.
		/// </summary>
		public DVec2D Position
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
		/// The <see cref="DViewport"/> size.
		/// </summary>
		public DVec2D Size
		{
			get{return Max - Min;}
			set{Max = Min + value;}
		}
		
		/// <summary>
		/// The <see cref="DViewport"/> x.
		/// </summary>
		public DDouble X
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
		/// The <see cref="DViewport"/> y.
		/// </summary>
		public DDouble Y
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
		/// The <see cref="DViewport"/> width.
		/// </summary>
		public DDouble Width
		{
			get{return Max.X - Min.X;}
			set{Max.X = Min.X + value;}
		}
		
		/// <summary>
		/// The <see cref="DViewport"/> height.
		/// </summary>
		public DDouble Height
		{
			get{return Max.Y - Min.Y;}
			set{Max.Y = Min.Y + value;}
		}
		
		/// <summary>
        /// Creates a new <see cref="DViewport"/> with the given values.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        public DViewport(DDouble x, DDouble y, DDouble w, DDouble h) : this(new DVec2D(x, y), new DVec2D(x + w, y + h))
        {
        	
        }
        
        /// <summary>
        /// Creates a new <see cref="DViewport"/> with the given values.
        /// </summary>
        /// <param name="min">The minimum coordinates.</param>
        /// <param name="max">The maximum coordinates.</param>
        public DViewport(DVec2D min, DVec2D max)
        {
        	Min = min;
        	Max = max;
        }
        
        protected override void Update()
		{
			Value = new Viewport(min.Value, max.Value);
		}
		
		protected internal override DElement<Viewport> GetExtension()
		{
			return new DViewport(new DVec2D(() => Value.Min.X, () => Value.Min.Y), new DVec2D(() => Value.Max.X, () => Value.Max.Y));
		}
        
        /// <summary>
        /// Returns true if this <see cref="DViewport"/> is a valid, space-filling view.
        /// </summary>
        /// <returns>True if valid.</returns>
        public DBool IsValid()
        {
        	return Min.X <= Max.X && Min.Y <= Max.Y;
        }
        
        /// <summary>
        /// Ensures this <see cref="DViewport"/> is at least the given size.
        /// </summary>
        /// <param name="width">The min allowed width.</param>
        /// <param name="height">The min allowed height.</param>
        public void EnsureSize(DDouble width, DDouble height)
        {
        	EnsureSize(new DVec2D(width, height));
        }
        
        /// <summary>
        /// Ensures this <see cref="DViewport"/> is at least the given size.
        /// </summary>
        /// <param name="size">The min allowed size.</param>
        public void EnsureSize(DVec2D size)
        {
        	Size = DVectorUtil.Max(Size, size);
        }
        
        /// <summary>
        /// Returns true if the given <see cref="DPoint2D"/> is within this <see cref="DViewport"/>'s bounds.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <returns>True if within, else false.</returns>
        public DBool Contains(DVec2D vec)
        {
        	return vec.X >= Min.X && vec.Y >= Min.Y && vec.X <= Max.X && vec.Y <= Max.Y;
        }
        
		/// <summary>
        /// Implicit cast from <see cref="DVec2D"/>.
        /// Resulting <see cref="DViewport"/> extends into quadrant 1 from the origin.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting view.</returns>
        public static explicit operator DViewport(DVec2D vec)
        {
        	return new DViewport(0, 0, vec.X, vec.Y);
        }
        
        /// <summary>
        /// Adds the given vec to the view's position.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved view.</returns>
        public static DViewport operator +(DViewport view, DVec2D vec)
        {
        	//passed by value, so we can just add vec to view pos and pass it back
        	view.Position += vec;
            return view;
        }

        /// <summary>
        /// Subtracts the given vec from the view's position.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved view.</returns>
        public static DViewport operator -(DViewport view, DVec2D vec)
        {
        	//passed by value, so we can just add vec to view pos and pass it back
        	view.Position -= vec;
            return view;
        }

        /// <summary>
        /// Multiplies the given view's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized view.</returns>
        public static DViewport operator *(DViewport view, DDouble val)
        {
        	DVec2D middle = (view.Max + view.Min) / (DDouble)2.0;
        	view.Min -= middle;
        	view.Min *= val;
        	view.Min += middle;
        	view.Max -= middle;
        	view.Max *= val;
        	view.Max += middle;
            return view;
        }

        /// <summary>
        /// Divides the given view's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized view.</returns>
        public static DViewport operator /(DViewport view, DDouble val)
        {
        	DVec2D middle = (view.Max + view.Min) / (DDouble)2.0;
        	view.Min -= middle;
        	view.Min /= val;
        	view.Min += middle;
        	view.Max -= middle;
        	view.Max /= val;
        	view.Max += middle;
            return view;
        }
        
        /// <summary>
        /// Multiplies the given view's size by the given value, about the given vector.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="vec">The anchor vec.</param>
        /// <returns>The resized view.</returns>
        public DViewport ResizeAbout(DDouble val, DVec2D vec)
        {
        	DViewport view = this;
        	view.Min -= vec;
        	view.Min *= val;
        	view.Min += vec;
        	view.Max -= vec;
        	view.Max *= val;
        	view.Max += vec;
        	return view;
        }
        
        public override bool Equals(object obj)
        {
			return this == (obj as DElement<Viewport>);
        }
		
		public static DBool operator ==(DViewport ele, DElement<Viewport> ele2)
		{
			if(ele == null || ele2 == null) return false;
			return (DBool)(() => ele.Value == ele2.Value);
		}
		
		public static DBool operator !=(DViewport ele, DElement<Viewport> ele2)
		{
			return !(ele == ele2);
		}
	}
}