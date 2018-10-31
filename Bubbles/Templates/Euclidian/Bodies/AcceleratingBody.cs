using Bubbles.Templates.Euclidian;
using Bubbles.MonotonicFunctions;
using Bubbles.MonotonicFunctions.Functions;

namespace Bubbles.Templates.Euclidian.Bodies
{
	/// <summary>
	/// Defines a body in n-dimensional Euclidian space which has a fixed
	/// acceleration.
	/// </summary>
	public class AcceleratingBody : EuclidianBubble
	{

		public readonly int Dimension;
		public readonly float BodyRadius;

		private float measuredTime;
		private VectorN position, velocity, acceleration;

		private MonotonicFunction reach;

		public AcceleratingBody(int dimensions, float bodyRadius)
		{
			Dimension = dimensions;
			BodyRadius = bodyRadius;

			measuredTime = 0f;
			position = new VectorN(Dimension);
			velocity = new VectorN(Dimension);
			acceleration = new VectorN(Dimension);
		}

		public override float GetMeasurementTime()
		{
			return measuredTime;
		}

		public override VectorN GetPosition()
		{
			return position;
		}

		public override MonotonicFunction GetBoundaryRadius()
		{
			return reach;
		}

		public override void SetState(float t)
		{
			transform.position = PositionAtTime(t).ToVector3();
		}

		public override void RebaseTime(float offset)
		{
			measuredTime -= offset;
			RecalculateReach();
		}

		public override void Remeasure(float t)
		{
			var newPosition = PositionAtTime(t);
			var newVelocity = VelocityAtTime(t);

			// Set values individually then only recalculate reach once
			position = newPosition;
			velocity = newVelocity;
			measuredTime = t;
			RecalculateReach();
		}

		/// <summary>
		/// Determine the new reach function for this accelerating body,
		/// assuming that some of the measured properties have changed since
		/// it was last calculated.
		/// </summary>
		private void RecalculateReach()
		{
			float t = measuredTime;
			float s0 = position.Magnitude;
			float v0 = velocity.Magnitude;
			float a0 = acceleration.Magnitude;

			float a = 0.5f * a0;
			float b = v0 - 2 * a * t;
			float c = s0 - b * t - a * t * t;

			reach = new Quadratic(a, b, c);
		}

		/// <summary>
		/// Measure the body at the new point in time, update its position,
		/// and then calculate its reach again.
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="newPosition">New position.</param>
		public void SetPosition(float time, VectorN newPosition)
		{
			Remeasure(time);
			position = newPosition;
			RecalculateReach();
		}

		/// <summary>
		/// Measure the body at the new point in time, update its velocity,
		/// and then calculate its reach again.
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="newVelocity">New velocity.</param>
		public void SetVelocity(float time, VectorN newVelocity)
		{
			Remeasure(time);
			velocity = newVelocity;
			RecalculateReach();
		}

		/// <summary>
		/// Measure the body at the new point in time, update its acceleration,
		/// and then calculate its reach again.
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="newAcceleration">New acceleration.</param>
		public void SetAcceleration(float time, VectorN newAcceleration)
		{
			Remeasure(time);
			acceleration = newAcceleration;
			RecalculateReach();
		}

		/// <summary>
		/// Calculate the body's position at the given time based off its
		/// most recently measured properties.
		/// </summary>
		/// <returns>The at time.</returns>
		/// <param name="t">T.</param>
		public VectorN PositionAtTime(float t)
		{
			float dt = t - measuredTime;
			var velocityAtTime = VelocityAtTime(t);
			var positionChange = velocity.MapWith(velocityAtTime,
				(v0, v1) => 0.5f * (v0 + v1) * dt);
			return position + positionChange;
		}

		/// <summary>
		/// Calculate the body's velocity at the given time based off its
		/// most recently measured properties.
		/// </summary>
		/// <returns>The at time.</returns>
		/// <param name="t">T.</param>
		public VectorN VelocityAtTime(float t)
		{
			float dt = t - measuredTime;
			return velocity.MapWith(acceleration, (v, a) => v + a * dt);
		}

	}
}
