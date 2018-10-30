using Bubbles.MonotonicFunctions;
using UnityEngine;

namespace Bubbles.Templates
{
	/// <summary>
	/// A more practical implementation of a base bubble class for
	/// direct implementation as a GameObject.
	/// </summary>
	public abstract class Bubble<Position> : MonoBehaviour
		where Position : IPosition<Position>
	{

		/// <summary>
		/// Return the time at which the system's state was measured.
		/// </summary>
		/// <returns>The measurement time.</returns>
		public abstract float GetMeasurementTime();

		/// <summary>
		/// Return the position of the bubble at the time at which
		/// the system was measured.
		/// </summary>
		/// <returns>The position.</returns>
		public abstract Position GetPosition();

		/// <summary>
		/// Return a monotonic function representing the radius of the
		/// bubble's sphere of influence as a function of time.
		/// </summary>
		/// <returns>The boundary radius.</returns>
		public abstract MonotonicFunction GetBoundaryRadius();

		/// <summary>
		/// Set the state of the GameObject to represent the state of the
		/// physical system at the time t.
		/// </summary>
		/// <param name="t">T.</param>
		public abstract void SetState(float t);

		/// <summary>
		/// Offset the time at which the system's state was measured such
		/// that S'(t - offset) = S(t) for all t.  This will be called
		/// periodically to ensure that no floating point errors accumulate
		/// as the time value increases in magnitude.
		/// </summary>
		/// <param name="deltaTime">Delta time.</param>
		public abstract void RebaseTime(float offset);

		/// <summary>
		/// Remeasure the properties of the system at the given time.
		/// </summary>
		/// <param name="t">T.</param>
		public abstract void Remeasure(float t);

		/// <summary>
		/// At each time step, set the state of the GameObject to represent
		/// the state of the physical system at the current time.
		/// </summary>
		public void Update()
		{
			SetState(Time.time);
		}

	}
}
