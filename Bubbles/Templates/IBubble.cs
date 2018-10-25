using Bubbles.MonotonicFunctions;

namespace Bubbles.Templates
{
	/// <summary>
	/// Defines the required behaviour of a bubble, in its most abstract form,
	/// that represents a certain type of physical system.
	/// </summary>
	public interface IBubble<System, Position>
	{

		/// <summary>
		/// Return the position of this bubble at the time at which
		/// its enclosed system was measured.
		/// </summary>
		/// <returns>The position.</returns>
		Position GetPosition();

		/// <summary>
		/// Return a function which can calculate the boundary radius of the
		/// bubble at a given time.
		/// </summary>
		/// <returns>The boundary radius.</returns>
		MonotonicFunction GetBoundaryRadius();

		/// <summary>
		/// Return the state of the enclosed system at the given time.
		/// </summary>
		/// <returns>The at time.</returns>
		/// <param name="t">T.</param>
		System GetStateAtTime(float t);

		/// <summary>
		/// Return the time at which the state of the enclosed system
		/// was measured.
		/// </summary>
		/// <returns>The measured time.</returns>
		float GetMeasuredTime();

	}
}
