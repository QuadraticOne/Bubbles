using Bubbles.Util;
using UnityEngine;

namespace Bubbles.MonotonicFunctions
{
	/// <summary>
	/// Defines a function which has at least some interval on its domain
	/// for which its value either increases or remains constant.
	/// </summary>
	public abstract class MonotonicFunction
	{

		/// <summary>
		/// Return the domain in which this function is monotonically increasing.
		/// By default, returns an infinite domain.
		/// </summary>
		public virtual Interval Domain()
		{
			return new Interval(float.NegativeInfinity, float.PositiveInfinity);
		}

		/// <summary>
		/// Return the value of the function at the specified x-coordinate.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		public abstract float At(float x);

	}
}
