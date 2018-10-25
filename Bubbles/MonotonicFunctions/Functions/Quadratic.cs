using Bubbles.Util;
using Bubbles.MonotonicFunctions;

namespace Bubbles.MonotonicFunctions.Functions
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
			if (quadraticCoefficient == 0f)
			{
				throw new System.ArgumentException(
					"quadratic coefficient cannot be 0");
			}
			QuadraticCoefficient = quadraticCoefficient;
			LinearCoefficient = linearCoefficient;
			ConstantCoefficient = constantCoefficient;
		}

		public override Interval Domain()
		{
			float stationaryPoint = (-LinearCoefficient) / (2 * QuadraticCoefficient);
			if (QuadraticCoefficient < 0f)
			{
				return new Interval(float.NegativeInfinity, stationaryPoint);
			}
			else
			{
				return new Interval(stationaryPoint, float.PositiveInfinity);
			}
		}

		public override float At(float x)
		{
			return QuadraticCoefficient * x * x + LinearCoefficient * x + ConstantCoefficient;
		}

	}
}
