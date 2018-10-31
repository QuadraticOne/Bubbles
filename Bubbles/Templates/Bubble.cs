using UnityEngine;

namespace Bubbles.Templates
{
	/// <summary>
	/// A more practical implementation of a base bubble class for
	/// direct implementation as a GameObject.
	/// </summary>
	public abstract class Bubble<Position> : DiscontinuousBubble<Position>
		where Position : IPosition<Position>
	{

		/// <summary>
		/// Returns the time at which the next discontinuity occurs for this
		/// object.  For continuous bubbles this will always result in
		/// positive infinity.
		/// </summary>
		/// <returns>The discontinuity.</returns>
		public override float NextDiscontinuity()
		{
			return float.PositiveInfinity;
		}

		/// <summary>
		/// Continuous bubbles have no discontinuities and so resolving
		/// the next discontinuity does nothing.
		/// </summary>
		public override void ResolveNextDiscontinuity() {}

		/// <summary>
		/// Set the state of this GameObject to represent the state of the
		/// enclosed system at the given time.
		/// </summary>
		/// <param name="t">T.</param>
		public override void SetState(float t)
		{
			SetStateContinuous(t);
		}

		/// <summary>
		/// Remeasure the properties of the enclosed system at the given time.
		/// </summary>
		/// <param name="t">T.</param>
		public override void Remeasure(float t)
		{
			RemeasureContinuous(t);
		}

	}
}
