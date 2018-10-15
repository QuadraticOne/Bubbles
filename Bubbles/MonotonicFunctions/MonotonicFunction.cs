namespace Bubbles.MonotonicFunctions
{
	/// <summary>
	/// Defines a function which either increases or decreases over time,
	/// subject to the constraint that f(x) -> f'(x) >= 0 or f'(x) <= 0
	/// for all x.
	/// </summary>
	public abstract class MonotonicFunction
	{

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
		public float NumericalDerivativeAt(float x, float eps = 1e-7f)
		{
			return (At(x + eps) - At(x - eps)) / (2f * eps);
		}

	}
}
