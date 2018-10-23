using Bubbles.Util;

namespace Bubbles.MonotonicFunctions
{
	/// <summary>
	/// Defines a function which either increases or decreases over time,
	/// subject to the constraint that f(x) -> f'(x) >= 0 or f'(x) <= 0
	/// for all x.
	/// </summary>
	public abstract class MonotonicFunction
	{

		protected const float DEFAULT_EPS = 1e-4f;

		public enum MonotonicFunctionType
		{
			Decreasing,
			Constant,
			Increasing
		}

		/// <summary>
		/// Return the interval upon which the function is monotonic.  Returns
		/// an unbounded interval by default.
		/// </summary>
		/// <returns>The interval.</returns>
		public virtual Interval Domain()
		{
			return new Interval(float.NegativeInfinity, float.PositiveInfinity);
		}

		/// <summary>
		/// Return the value of the function at the specified x-value.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		public abstract float At(float x);

		/// <summary>
		/// Calculate the derivative of the function at the given point.
		/// If this function is not overridden, the gradient will be
		/// calculated numerically.
		/// </summary>
		/// <returns>The <see cref="System.Single"/>.</returns>
		/// <param name="x">The x coordinate.</param>
		public virtual float DerivativeAt(float x)
		{
			return NumericalDerivativeAt(x);
		}

		/// <summary>
		/// Numerically determine the derivative of the function at the
		/// given point.
		/// </summary>
		/// <returns>The <see cref="System.Single"/>.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="eps">Eps.</param>
		public float NumericalDerivativeAt(float x, float eps = DEFAULT_EPS)
		{
			return (At(x + eps) - At(x - eps)) / (2f * eps);
		}

		/// <summary>
		/// Calculate the second derivative of the function at the given
		/// point.  If this function is not overridden, the second derivative
		/// will be calculated numericallly.
		/// </summary>
		/// <returns>The <see cref="System.Single"/>.</returns>
		/// <param name="x">The x coordinate.</param>
		public virtual float SecondDerivativeAt(float x)
		{
			return NumericalSecondDerivativeAt(x);
		}

		/// <summary>
		/// Numerically determine the second derivative of the function
		/// at a given point.
		/// </summary>
		/// <returns>The <see cref="System.Single"/>.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="eps">Eps.</param>
		public float NumericalSecondDerivativeAt(float x, float eps = DEFAULT_EPS)
		{
			return (DerivativeAt(x + eps) - DerivativeAt(x - eps)) / (2f * eps);
		}

		/// <summary>
		/// Try to determine the type of the function (increasing, decreasing, or
		/// constant) by testing the first derivative at a point.  If the first
		/// derivative is zero, the second derivative will also be checked.  If it
		/// is also zero, only then will the function be presumed to be constant.
		/// </summary>
		/// <returns>The function type.</returns>
		/// <param name="x">The x coordinate.</param>
		public MonotonicFunctionType DetermineFunctionType(float x = 0.0f)
		{
			var firstDerivative = DerivativeAt(x);
			if (firstDerivative > 0)
			{
				return MonotonicFunctionType.Increasing;
			}
			else if (firstDerivative < 0)
			{
				return MonotonicFunctionType.Decreasing;
			}
			else
			{
				var secondDerivative = SecondDerivativeAt(x);
				if (secondDerivative < 0)
				{
					return MonotonicFunctionType.Decreasing;
				}
				else if (secondDerivative > 0)
				{
					return MonotonicFunctionType.Increasing;
				}
				else
				{
					return MonotonicFunctionType.Constant;
				}
			}
		}

		/// <summary>
		/// Find the smallest possible interval which bounds the zero of the
		/// function in its monotonic domain, if one exists.  If the zero can
		/// be calculated exactly, this should be represented by an interval
		/// whose values are (x - DEFAULT_EPS) and (x + DEFAULT_EPS).
		/// </summary>
		/// <returns>The zero.</returns>
		/// <param name="maxExpansionIterations">Max expansion iterations.</param>
		/// <param name="expansionFactor">Expansion factor.</param>
		/// <param name="maxContractionIterations">Max contraction iterations.</param>
		/// <param name="maximumIntervalWidth">Maximum interval width.</param>
		public virtual Maybe<Interval> BoundZero(int maxExpansionIterations = 20,
			int expansionFactor = 3, int maxContractionIterations = 20,
			float maximumIntervalWidth = 0.01f)
		{
			var containsZero = LocateZero(InitialSearchDomain(),
				maxExpansionIterations, expansionFactor);
			if (containsZero.HasResult())
			{
				return new Maybe<Interval>(PinpointZero(containsZero.GetResult(),
					maxContractionIterations, maximumIntervalWidth));
			}
			else
			{
				return containsZero;
			}
		}

		/// <summary>
		/// Return an interval which is guaranteed to contain the function's zero
		/// if one exists, or Nothing if the function has no zero or it cannot
		/// be found.
		/// </summary>
		/// <returns>The zero.</returns>
		/// <param name="startingInterval">Starting interval.</param>
		/// <param name="maxExpansionIterations">Max expansion iterations.</param>
		/// <param name="expansionFactor">Expansion factor.</param>
		protected virtual Maybe<Interval> LocateZero(Interval startingInterval,
			int maxExpansionIterations, int expansionFactor)
		{
			Interval functionDomain = Domain();
			int iterations = 0;
			Interval searchInterval = startingInterval;
			while (iterations < maxExpansionIterations
				&& !IntervalContainsZero(searchInterval))
			{
				iterations++;
				if (At(searchInterval.UpperBound) < 0)
				{
					searchInterval = searchInterval.ExtendUpwardsProportional(expansionFactor);
					if (searchInterval.UpperBound > functionDomain.UpperBound)
					{
						// Should always be safe since the initial search domain should be
						// a proper subset of the function domain
						Interval crossover = searchInterval
							.IntersectionWith(functionDomain).GetResult();
						return IntervalContainsZero(crossover)
							? new Maybe<Interval>(crossover)
							: new Maybe<Interval>();
					}
				}
				else
				{
					searchInterval = searchInterval.ExtendDownwardsProportional(expansionFactor);
					if (searchInterval.LowerBound < functionDomain.LowerBound)
					{
						// Should always be safe since the initial search domain should be
						// a proper subset of the function domain
						Interval crossover = searchInterval
							.IntersectionWith(functionDomain).GetResult();
						return IntervalContainsZero(crossover)
							? new Maybe<Interval>(crossover)
							: new Maybe<Interval>();
					}
				}
			}
			return IntervalContainsZero(searchInterval)
				? new Maybe<Interval>(searchInterval)
				: new Maybe<Interval>();
		}

		/// <summary>
		/// Given an interval which is known to contain the function's zero, return an interval
		/// whose length is shorter than the given value.  This length may be exceeded if the
		/// calculation is done iteratively, and the maximum number of iterations is exceeded.
		/// </summary>
		/// <returns>The zero.</returns>
		/// <param name="startingInterval">Starting interval.</param>
		/// <param name="maxContractionIterations">Max contraction iterations.</param>
		protected virtual Interval PinpointZero(Interval startingInterval,
			int maxContractionIterations, float maximumWidth)
		{
			int iterations = 0;
			Interval searchInterval = startingInterval;
			while (searchInterval.Width > maximumWidth && iterations < maxContractionIterations)
			{
				iterations++;
				var splitInterval = searchInterval.SplitAt(0.5f);
				searchInterval = At(splitInterval.First.UpperBound) > 0
					? splitInterval.First
					: splitInterval.Second;
			}
			return searchInterval;
		}

		/// <summary>
		/// Determine whether or not the given interval contains this function's
		/// zero, assuming one exists and that the interval is a proper subset
		/// of the domain of this function.
		/// </summary>
		/// <returns><c>true</c>, if contains zero was intervaled, <c>false</c> otherwise.</returns>
		/// <param name="interval">Interval.</param>
		public bool IntervalContainsZero(Interval interval)
		{
			return At(interval.LowerBound) < 0 && At(interval.UpperBound) > 0;
		}

		/// <summary>
		/// Return the best best of a constricted domain in which to start searching
		/// for zeroes of the function.  If the domain extends to infinity in both
		/// directions, the interval [-gaugeWidth, gaugeWidth] will be given.  If
		/// the domain extends to infinity in one direction, the non-infinite bound
		/// will be used as one bound and the other will be calculated so as to
		/// make the width gaugeWidth.  If neither bound is infinity, the domain
		/// of the function will be returned.
		/// </summary>
		/// <returns>The search domain.</returns>
		protected virtual Interval InitialSearchDomain(float gaugeWidth = 10f)
		{
			Interval functionDomain = Domain();
			if (float.IsNegativeInfinity(functionDomain.LowerBound)
				&& float.IsPositiveInfinity(functionDomain.UpperBound))
			{
				return new Interval(-gaugeWidth, gaugeWidth);
			}
			else if (float.IsNegativeInfinity(functionDomain.LowerBound))
			{
				return new Interval(functionDomain.UpperBound - gaugeWidth,
					functionDomain.UpperBound);
			}
			else if (float.IsPositiveInfinity(functionDomain.UpperBound))
			{
				return new Interval(functionDomain.LowerBound,
					functionDomain.LowerBound + gaugeWidth);
			}
			else
			{
				return functionDomain;
			}
		}

	}
}
