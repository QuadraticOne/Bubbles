using Bubbles.Util;
using Bubbles.MonotonicFunctions;

namespace Bubbles.MonotonicFunctions.Functions
{
	public class Linear : MonotonicFunction
	{

		public readonly float Gradient, Intercept;

		/// <summary>
		/// Create a new Linear monotonic function.  Assumes that the
		/// value of `gradient` is more than 0.
		/// </summary>
		/// <param name="gradient">Gradient.</param>
		/// <param name="intercept">Intercept.</param>
		public Linear(float gradient, float intercept)
		{
			if (gradient <= 0f)
			{
				throw new System.ArgumentException(
					"cannot have a gradient of less than or equal to 0");
			}
			Gradient = gradient;
			Intercept = intercept;
		}

		public override float At(float x)
		{
			return Intercept + Gradient * x;
		}

	}
}
