using Bubbles.Util;

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

	}
}
