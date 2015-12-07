namespace IROM.Dynamix
{
	using System;
	
	/// <summary>
	/// Defines a variety of utility methods for vectors.
	/// </summary>
	public static class DVectorUtil
	{
		/// <summary>
        /// Component-wise clips the given <see cref="DVec1D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DVec1D Clip(DVec1D value, DVec1D min, DVec1D max)
        {
        	DVec1D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise clips the given <see cref="DVec2D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DVec2D Clip(DVec2D value, DVec2D min, DVec2D max)
        {
        	DVec2D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	
        	if(value.Y <= min.Y) vec.Y = min.Y;
        	else if(value.Y >= max.Y) vec.Y = max.Y;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise clips the given <see cref="DVec3D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DVec3D Clip(DVec3D value, DVec3D min, DVec3D max)
        {
        	DVec3D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	
        	if(value.Y <= min.Y) vec.Y = min.Y;
        	else if(value.Y >= max.Y) vec.Y = max.Y;
        	
        	if(value.Z <= min.Z) vec.Z = min.Z;
        	else if(value.Z >= max.Z) vec.Z = max.Z;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise clips the given <see cref="DVec4D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DVec4D Clip(DVec4D value, DVec4D min, DVec4D max)
        {
        	DVec4D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	
        	if(value.Y <= min.Y) vec.Y = min.Y;
        	else if(value.Y >= max.Y) vec.Y = max.Y;
        	
        	if(value.Z <= min.Z) vec.Z = min.Z;
        	else if(value.Z >= max.Z) vec.Z = max.Z;
        	
        	if(value.W <= min.W) vec.W = min.W;
        	else if(value.W >= max.W) vec.W = max.W;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise wraps the given <see cref="DVec1D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DVec1D Wrap(DVec1D value, DVec1D min, DVec1D max)
        {
        	DVec1D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise wraps the given <see cref="DVec2D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DVec2D Wrap(DVec2D value, DVec2D min, DVec2D max)
        {
        	DVec2D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	
        	vec.Y -= min.Y;
			vec.Y %= max.Y - min.Y;
        	if(vec.Y< 0) vec.Y += max.Y - min.Y;
        	vec.Y += min.Y;
        	return vec;
        }
		
        /// <summary>
        /// Component-wise wraps the given <see cref="DVec3D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DVec3D Wrap(DVec3D value, DVec3D min, DVec3D max)
        {
        	DVec3D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	
        	vec.Y -= min.Y;
			vec.Y %= max.Y - min.Y;
        	if(vec.Y< 0) vec.Y += max.Y - min.Y;
        	vec.Y += min.Y;
        	
        	vec.Z -= min.Z;
			vec.Z %= max.Z - min.Z;
        	if(vec.Z< 0) vec.Z += max.Z - min.Z;
        	vec.Z += min.Z;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise wraps the given <see cref="DVec4D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DVec4D Wrap(DVec4D value, DVec4D min, DVec4D max)
        {
        	DVec4D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	
        	vec.Y -= min.Y;
			vec.Y %= max.Y - min.Y;
        	if(vec.Y< 0) vec.Y += max.Y - min.Y;
        	vec.Y += min.Y;
        	
        	vec.Z -= min.Z;
			vec.Z %= max.Z - min.Z;
        	if(vec.Z< 0) vec.Z += max.Z - min.Z;
        	vec.Z += min.Z;
        	
        	vec.W -= min.W;
			vec.W %= max.W - min.W;
        	if(vec.W< 0) vec.W += max.W - min.W;
        	vec.W += min.W;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise clips the given <see cref="DPoint1D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DPoint1D Clip(DPoint1D value, DPoint1D min, DPoint1D max)
        {
        	DPoint1D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise clips the given <see cref="DPoint2D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DPoint2D Clip(DPoint2D value, DPoint2D min, DPoint2D max)
        {
        	DPoint2D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	
        	if(value.Y <= min.Y) vec.Y = min.Y;
        	else if(value.Y >= max.Y) vec.Y = max.Y;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise clips the given <see cref="DPoint3D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DPoint3D Clip(DPoint3D value, DPoint3D min, DPoint3D max)
        {
        	DPoint3D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	
        	if(value.Y <= min.Y) vec.Y = min.Y;
        	else if(value.Y >= max.Y) vec.Y = max.Y;
        	
        	if(value.Z <= min.Z) vec.Z = min.Z;
        	else if(value.Z >= max.Z) vec.Z = max.Z;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise clips the given <see cref="DPoint4D"/> to between min and max.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public static DPoint4D Clip(DPoint4D value, DPoint4D min, DPoint4D max)
        {
        	DPoint4D vec = value;
        	if(value.X <= min.X) vec.X = min.X;
        	else if(value.X >= max.X) vec.X = max.X;
        	
        	if(value.Y <= min.Y) vec.Y = min.Y;
        	else if(value.Y >= max.Y) vec.Y = max.Y;
        	
        	if(value.Z <= min.Z) vec.Z = min.Z;
        	else if(value.Z >= max.Z) vec.Z = max.Z;
        	
        	if(value.W <= min.W) vec.W = min.W;
        	else if(value.W >= max.W) vec.W = max.W;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise wraps the given <see cref="DPoint1D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DPoint1D Wrap(DPoint1D value, DPoint1D min, DPoint1D max)
        {
        	DPoint1D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise wraps the given <see cref="DPoint2D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DPoint2D Wrap(DPoint2D value, DPoint2D min, DPoint2D max)
        {
        	DPoint2D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	
        	vec.Y -= min.Y;
			vec.Y %= max.Y - min.Y;
        	if(vec.Y< 0) vec.Y += max.Y - min.Y;
        	vec.Y += min.Y;
        	return vec;
        }
		
        /// <summary>
        /// Component-wise wraps the given <see cref="DPoint3D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DPoint3D Wrap(DPoint3D value, DPoint3D min, DPoint3D max)
        {
        	DPoint3D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	
        	vec.Y -= min.Y;
			vec.Y %= max.Y - min.Y;
        	if(vec.Y< 0) vec.Y += max.Y - min.Y;
        	vec.Y += min.Y;
        	
        	vec.Z -= min.Z;
			vec.Z %= max.Z - min.Z;
        	if(vec.Z< 0) vec.Z += max.Z - min.Z;
        	vec.Z += min.Z;
        	return vec;
        }
        
        /// <summary>
        /// Component-wise wraps the given <see cref="DPoint4D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The resulting value.</param>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public static DPoint4D Wrap(DPoint4D value, DPoint4D min, DPoint4D max)
        {
        	DPoint4D vec = value;
        	vec.X -= min.X;
			vec.X %= max.X - min.X;
        	if(vec.X< 0) vec.X += max.X - min.X;
        	vec.X += min.X;
        	
        	vec.Y -= min.Y;
			vec.Y %= max.Y - min.Y;
        	if(vec.Y< 0) vec.Y += max.Y - min.Y;
        	vec.Y += min.Y;
        	
        	vec.Z -= min.Z;
			vec.Z %= max.Z - min.Z;
        	if(vec.Z< 0) vec.Z += max.Z - min.Z;
        	vec.Z += min.Z;
        	
        	vec.W -= min.W;
			vec.W %= max.W - min.W;
        	if(vec.W< 0) vec.W += max.W - min.W;
        	vec.W += min.W;
        	return vec;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DVec1D Max(params DVec1D[] vecs)
        {
        	DVec1D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DVec2D Max(params DVec2D[] vecs)
        {
        	DVec2D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DVec3D Max(params DVec3D[] vecs)
        {
        	DVec3D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DVec4D Max(params DVec4D[] vecs)
        {
        	DVec4D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        		if(vecs[i].W > max.W)
        		{
        			max.W = vecs[i].W;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DVec1D Min(params DVec1D[] vecs)
        {
        	DVec1D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DVec2D Min(params DVec2D[] vecs)
        {
        	DVec2D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DVec3D Min(params DVec3D[] vecs)
        {
        	DVec3D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DVec4D Min(params DVec4D[] vecs)
        {
        	DVec4D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        		if(vecs[i].W < min.W)
        		{
        			min.W = vecs[i].W;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DPoint1D Max(params DPoint1D[] vecs)
        {
        	DPoint1D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DPoint2D Max(params DPoint2D[] vecs)
        {
        	DPoint2D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DPoint3D Max(params DPoint3D[] vecs)
        {
        	DPoint3D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static DPoint4D Max(params DPoint4D[] vecs)
        {
        	DPoint4D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        		if(vecs[i].W > max.W)
        		{
        			max.W = vecs[i].W;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DPoint1D Min(params DPoint1D[] vecs)
        {
        	DPoint1D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DPoint2D Min(params DPoint2D[] vecs)
        {
        	DPoint2D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DPoint3D Min(params DPoint3D[] vecs)
        {
        	DPoint3D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static DPoint4D Min(params DPoint4D[] vecs)
        {
        	DPoint4D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        		if(vecs[i].W < min.W)
        		{
        			min.W = vecs[i].W;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the overlap of the given <see cref="DRectangle"/>s.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="rects">The rectangles.</param>
        /// <returns>The overlapping <see cref="DRectangle"/>.</returns>
        public static DRectangle Overlap(params DRectangle[] rects)
        {
        	DPoint2D min = rects[0].Min;
        	DPoint2D max = rects[0].Max;
        	for(int i = 1; i < rects.Length; i++)
        	{
        		min = Max(min, rects[i].Min);
        		max = Min(max, rects[i].Max);
        	}
        	return new DRectangle(min, max);
        }
        
        /// <summary>
        /// Returns the encompassing <see cref="DRectangle"/> of the given <see cref="DRectangle"/>s.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <returns>The encompassing <see cref="DRectangle"/>.</returns>
        public static DRectangle Encompass(params DRectangle[] rects)
        {
        	DPoint2D min = rects[0].Min;
        	DPoint2D max = rects[0].Max;
        	for(int i = 1; i < rects.Length; i++)
        	{
        		min = Min(min, rects[i].Min);
        		max = Max(max, rects[i].Max);
        	}
        	return new DRectangle(min, max);
        }
        
        /// <summary>
        /// Returns the overlap of the given <see cref="DViewport"/>s.
        /// </summary>
        /// <param name="views">The viewports.</param>
        /// <returns>The overlapping <see cref="DViewport"/>.</returns>
        public static DViewport Overlap(params DViewport[] views)
        {
        	DVec2D min = views[0].Min;
        	DVec2D max = views[0].Max;
        	for(int i = 1; i < views.Length; i++)
        	{
        		min = Max(min, views[i].Min);
        		max = Min(max, views[i].Max);
        	}
        	return new DViewport(min, max);
        }
        
        /// <summary>
        /// Returns the encompassing <see cref="DViewport"/> of the given <see cref="DViewport"/>s.
        /// </summary>
        /// <param name="views">The viewports.</param>
        /// <returns>The encompassing <see cref="DViewport"/>.</returns>
        public static DViewport Encompass(params DViewport[] views)
        {
        	DVec2D min = views[0].Min;
        	DVec2D max = views[0].Max;
        	for(int i = 1; i < views.Length; i++)
        	{
        		min = Min(min, views[i].Min);
        		max = Max(max, views[i].Max);
        	}
        	return new DViewport(min, max);
        }
	}
}
