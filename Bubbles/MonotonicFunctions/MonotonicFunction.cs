namespace Bubbles.MonotonicFunctions
{
	/// <summary>
	/// Defines a function which either increases or decreases over time,
	/// subject to the constraint that f(x) -> f'(x) >= 0 or f'(x) <= 0
	/// for all x.
	/// </summary>
	public abstract class MonotonicFunction
	{

		private const float DEFAULT_EPS = 1e-7f;

		public enum MonotonicFunctionType
		{
			Decreasing,
			Constant,
			Increasing
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

	}
}
