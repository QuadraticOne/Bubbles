namespace Bubbles.Util
{
	/// <summary>
	/// Defines an interval in R^1.
	/// </summary>
	public class Interval {

		public readonly float LowerBound, UpperBound;
		public readonly float Width;

		public Interval(float lowerBound, float upperBound)
		{
			if (lowerBound > upperBound)
				throw new System.ArgumentException("lower bound is greater than upper bound");

			LowerBound = lowerBound;
			UpperBound = upperBound;
			Width = UpperBound - LowerBound;
		}

		/// <summary>
		/// Return true iff the point x is strictly inside the interval.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		public bool StrictlyContains(float x)
		{
			return (x > LowerBound) && (x < UpperBound);
		}

		/// <summary>
		/// Return true iff the point x is contained inside the interval, or is equal
		/// to one of its bounds.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		public bool Contains(float x)
		{
			return (x >= LowerBound) && (x <= UpperBound);
		}

		public bool StrictlyAbove(float x)
		{
			return x < LowerBound;
		}

		public bool Above(float x)
		{
			return x <= LowerBound;
		}

		public bool StrictlyBelow(float x)
		{
			return x > UpperBound;
		}

		public bool Below(float x)
		{
			return x >= UpperBound;
		}

		/// <summary>
		/// Return a point on the real line, defined as r * Width units
		/// from the lower bound of this interval.
		/// </summary>
		/// <returns>The within by ratio.</returns>
		/// <param name="r">The red component.</param>
		public float PointByRatio(float r)
		{
			return LowerBound + r * Width;
		}

		/// <summary>
		/// Split the interval into a pair of intervals, dividing it at
		/// a point a certain proportion along its width.
		/// </summary>
		/// <returns>The <see cref="Bubbles.Util.Pair`2[[Bubbles.Util.Interval],[Bubbles.Util.Interval]]"/>.</returns>
		/// <param name="ratio">Ratio.</param>
		public Pair<Interval, Interval> SplitAt(float proportion)
		{
			var splitPoint = PointByRatio(proportion);
			return new Pair<Interval, Interval>(
				new Interval(LowerBound, splitPoint),
				new Interval(splitPoint, UpperBound));
		}

		/// <summary>
		/// Extend the upper bound of the interval by a certain amount.
		/// </summary>
		/// <returns>The upwards.</returns>
		/// <param name="dw">Dw.</param>
		public Interval ExtendUpwards(float dw)
		{
			return new Interval(LowerBound, UpperBound + dw);
		}

		/// <summary>
		/// Extend the lower bound of the interval by a certain amount.
		/// </summary>
		/// <returns>The downwards.</returns>
		/// <param name="dw">Dw.</param>
		public Interval ExtendDownwards(float dw)
		{
			return new Interval(LowerBound - dw, UpperBound);
		}

		/// <summary>
		/// Extend the upper bound of interval by a proportion of its width.
		/// </summary>
		/// <returns>The upwards proportional.</returns>
		/// <param name="proportionOfWidth">Proportion of width.</param>
		public Interval ExtendUpwardsProportional(float proportionOfWidth)
		{
			return ExtendUpwards(Width * proportionOfWidth);
		}

		/// <summary>
		/// Extend the lower bound of interval by a proportion of its width.
		/// </summary>
		/// <returns>The downwards proportional.</returns>
		/// <param name="proportionOfWidth">Proportion of width.</param>
		public Interval ExtendDownwardsProportional(float proportionOfWidth)
		{
			return ExtendDownwards(Width * proportionOfWidth);
		}

	}
}
