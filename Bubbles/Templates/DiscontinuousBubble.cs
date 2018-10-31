using UnityEngine;
using Bubbles.MonotonicFunctions;

namespace Bubbles.Templates
{
	/// <summary>
	/// A practical implementation of a discontinuous bubble, whose state cannot
	/// necessarily be stated as an explicit function of time, but rather has
	/// discontinuities.
	/// </summary>
	public abstract class DiscontinuousBubble<Position> : MonoBehaviour
		where Position : IPosition<Position>
	{

		/// <summary>
		/// Return the time at which the properties of the enclosed
		/// system were measured.
		/// </summary>
		/// <returns>The measurement time.</returns>
		public abstract float GetMeasurementTime();

		/// <summary>
		/// Return the position of the bubble's origin.
		/// </summary>
		/// <returns>The position.</returns>
		public abstract Position GetPosition();

		/// <summary>
		/// Return a monotonic function describing the bubble's boundary
		/// radius as a function of time.
		/// </summary>
		/// <returns>The boundary radius.</returns>
		public abstract MonotonicFunction GetBoundaryRadius();

		/// <summary>
		/// Return the time at which the next discontinuity occurs.
		/// </summary>
		/// <returns>The discontinuity.</returns>
		public abstract float NextDiscontinuity();

		/// <summary>
		/// Remeasure the state of the enclosed system immediately before
		/// the next discontinuity, update the object's properties to reflect
		/// any changes that occur across the discontinuity, and then remeasure
		/// the enclosed system's properties again.
		/// </summary>
		public abstract void ResolveNextDiscontinuity();

		/// <summary>
		/// Set the state of this GameObject to represent the state of the
		/// enclosed system at the given time.  It may be assumed that t will
		/// always be after the measured time and before the next discontinuity.
		/// </summary>
		/// <param name="t">T.</param>
		public abstract void SetStateContinuous(float t);

		/// <summary>
		/// Remeasure the properties of the enclosed system at the given time.
		/// It may be assumed that t will always be after the measured time
		/// and before the next discontinuity.
		/// </summary>
		/// <param name="t">T.</param>
		public abstract void RemeasureContinuous(float t);

		/// <summary>
		/// Change the listed time of measurement such that the state of the
		/// enclosed system at (t - offset) after rebasing is equal to its state
		/// at (t) before rebasing.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public abstract void RebaseTime(float offset);

		/// <summary>
		/// Set the state of this GameObject to represent the state of the
		/// enclosed system at the given time.  This will automatically resolve
		/// any discontinuities between the current measured time and t.
		/// </summary>
		/// <param name="t">T.</param>
		public virtual void SetState(float t)
		{
			if (t < NextDiscontinuity())
			{
				SetStateContinuous(t);
			}
			else
			{
				ResolveNextDiscontinuity();
				SetState(t);
			}
		}

		/// <summary>
		/// Remeasure the properties of the enclosed system at the given time.
		/// This will automatically resolve any discontinuities between the
		/// current measured time and t.
		/// </summary>
		/// <param name="t">T.</param>
		public virtual void Remeasure(float t)
		{
			if (t < NextDiscontinuity())
			{
				RemeasureContinuous(t);
			}
			else
			{
				ResolveNextDiscontinuity();
				Remeasure(t);
			}
		}

		/// <summary>
		/// At each time step, set the state of the GameObject to
		/// represent the state of the enclosed system at the current time.
		/// </summary>
		public void Update()
		{
			SetState(Time.time);
		}

	}
}
