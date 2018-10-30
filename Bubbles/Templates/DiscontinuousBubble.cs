using UnityEngine;

namespace Bubbles.Templates
{
	/// <summary>
	/// A practical implementation of a discontinuous bubble, whose state cannot
	/// necessarily be stated as an explicit function of time, but rather has
	/// discontinuities.
	/// </summary>
	public abstract class DiscontinuousBubble<Position> : Bubble<Position>
		where Position : IPosition<Position>
	{

		/// <summary>
		/// Return the time at which the next discontinuity will occur.
		/// It is assumed that the system's state will be continuous
		/// and an explicit function of time between the time at which
		/// it was measured and the time at which the next discontinuity
		/// occurs.  This value should be memoised if possible.
		/// </summary>
		/// <returns>The discontinuity.</returns>
		public abstract float NextDiscontinuity();

		/// <summary>
		/// Set the state of the system to the state in which it will
		/// be immediately after the next discontinuity.  This may include
		/// updating the measured time, next discontinuity time, and
		/// other properties.  After resolving the discontinuity, the
		/// time at which the next discontinuity will occur should be
		/// greater then the time of the discontinuity which just occurred.
		/// </summary>
		public abstract void ResolveDiscontinuity();

		/// <summary>
		/// Remeasure the properties of the system at the given time; it can be
		/// safely assumed that this will only be called for times which are
		/// before the system's next discontinuity.
		/// </summary>
		/// <param name="t">T.</param>
		public abstract void RemeasureContinuous(float t);

		/// <summary>
		/// Remeausre the properties of the system at the given time.  If the
		/// given time is beyond the system's next discontinuity, any necessary
		/// discontinuities will be resolved before remeasuring at the given
		/// time.
		/// </summary>
		/// <param name="t">T.</param>
		public override void Remeasure(float t)
		{
			if (t < NextDiscontinuity())
			{
				RemeasureContinuous(t);
			}
			else
			{
				ResolveDiscontinuity();
				Remeasure(t);
			}
		}

		/// <summary>
		/// Set the state of the GameObject to represent the state of
		/// the system at time t.  It can be assumed that this method will
		/// only be called with values that are in the continuous region of
		/// the system's state - between its measured time and the time
		/// of the next discontinuity.
		/// </summary>
		public abstract void SetStateContinuous(float t);

		/// <summary>
		/// Set the state of the GameObject to represent the state of
		/// the system at time t, automatically resolving any
		/// discontinuities.
		/// </summary>
		/// <param name="t">T.</param>
		public override void SetState(float t)
		{
			if (t < NextDiscontinuity())
			{
				SetStateContinuous(t);
			}
			else
			{
				ResolveDiscontinuity();
				SetState(t);
			}
		}

	}
}
