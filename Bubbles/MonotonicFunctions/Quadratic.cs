using Bubbles.Util;
using UnityEngine;

namespace Bubbles.MonotonicFunctions
{
	public class Quadratic : MonotonicFunction
	{

		public readonly float
			QuadraticCoefficient,
			LinearCoefficient,
			ConstantCoefficient;

		public Quadratic(float quadraticCoefficient, float linearCoefficient,
			float constantCoefficient)
		{
			QuadraticCoefficient = quadraticCoefficient;
			LinearCoefficient = linearCoefficient;
			ConstantCoefficient = constantCoefficient;
		}

		public override float At(float x)
		{
			return QuadraticCoefficient * x * x + LinearCoefficient * x + ConstantCoefficient;
		}

		public override float DerivativeAt (float x)
		{
			return 2 * QuadraticCoefficient * x + LinearCoefficient;
		}

		public override float SecondDerivativeAt (float x)
		{
			return 2 * QuadraticCoefficient;
		}

		public override Interval Domain()
		{
			return QuadraticCoefficient > 0
				? new Interval(ApexX() + DEFAULT_EPS, float.PositiveInfinity)
				: new Interval(float.NegativeInfinity, ApexX() - DEFAULT_EPS);
		}

		/// <summary>
		/// Return the x-coordinate of the function's stationary point.
		/// </summary>
		/// <returns>The x.</returns>
		public float ApexX()
		{
			return (-LinearCoefficient) / (2 * QuadraticCoefficient);
		}

		/// <summary>
		/// Return the y-coordinate of the function's stationary point.
		/// </summary>
		/// <returns>The y.</returns>
		public float ApexY()
		{
			return this.At(ApexX());
		}

		/// <summary>
		/// Calculate and return the discriminant of the quadratic.
		/// </summary>
		public float Discriminant()
		{
			return LinearCoefficient * LinearCoefficient
				- 4 * QuadraticCoefficient * ConstantCoefficient;
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
			float discriminant = Discriminant();
			if (discriminant < 0f)
			{
				return new Maybe<Interval>();
			}
			else
			{
				// If the function is convex then we want the zero
				// that is to the left of the maximum
				int sign = QuadraticCoefficient < 0 ? - 1 : 1;
				float signFlippedTerm = Mathf.Sqrt(discriminant
					- 4 * QuadraticCoefficient * LinearCoefficient);
				float zero = (-LinearCoefficient + sign * signFlippedTerm)
					/ (2 * QuadraticCoefficient);
				return new Maybe<Interval>(new Interval(
					zero - DEFAULT_EPS,
					zero + DEFAULT_EPS));
			}
		}

		/// <summary>
		/// Return the function that results from summing two quadratics.
		/// </summary>
		/// <param name="other">Other.</param>
		public Quadratic Add(Quadratic other)
		{
			return new Quadratic(
				this.QuadraticCoefficient + other.QuadraticCoefficient,
				this.LinearCoefficient + other.LinearCoefficient,
				this.ConstantCoefficient + other.ConstantCoefficient
			);
		}

	}
}
