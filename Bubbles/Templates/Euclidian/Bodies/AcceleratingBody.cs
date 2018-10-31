using Bubbles.Templates.Euclidian;
using Bubbles.MonotonicFunctions;
using Bubbles.MonotonicFunctions.Functions;
using UnityEngine;

namespace Bubbles.Templates.Euclidian.Bodies
{
	/// <summary>
	/// Defines a body in n-dimensional Euclidian space which has a fixed
	/// acceleration.
	/// </summary>
	public class AcceleratingBody : EuclidianBubble
	{

		public int Dimension;
		public float BodyRadius;

		protected float measuredTime;
		protected VectorN position, velocity, acceleration;

		protected MonotonicFunction reach;

		public void Start()
		{
			measuredTime = 2f;
			position = VectorN.FromVector3(transform.position);
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
			float s0 = BodyRadius;
			float v0 = velocity.Magnitude;
			float a0 = acceleration.Magnitude;

			if (a0 != 0.0f)
			{
				float a = 0.5f * a0;
				float b = v0 - 2 * a * t;
				float c = s0 - b * t - a * t * t;
				reach = new Quadratic(a, b, c);
			}
			else if (v0 != 0)
			{
				reach = new Linear(v0, s0);
			}
			else
			{
				reach = new Constant(s0);
			}
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
