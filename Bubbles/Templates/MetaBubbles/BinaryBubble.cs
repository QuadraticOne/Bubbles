using Bubbles.MonotonicFunctions;
using Bubbles.MonotonicFunctions.Functions;
using Bubbles.Templates;
using Bubbles.Templates.Solvers;
using Bubbles.Util;

namespace Bubbles.Templates.MetaBubbles
{
	public class BinaryBubble<Position> : DiscontinuousBubble<Position>
		where Position : class, IPosition<Position>
	{

		/// <summary>
		/// Find the time of the next discontinuity between a binary bubble and
		/// some other discontinuous bubble.
		/// </summary>
		/// <returns>The next discontinuity after.</returns>
		/// <param name="solver">Solver.</param>
		/// <param name="after">After.</param>
		/// <param name="binaryBubble">Binary bubble.</param>
		/// <param name="other">Other.</param>
		public static float PairNextDiscontinuityAfter(BubbleInteractionSolver<Position> solver, 
			float after, BinaryBubble<Position> binaryBubble, DiscontinuousBubble<Position> other)
		{
			var leftNextDiscontinuity = solver.NextDiscontinuityAfter(
				after, binaryBubble.Left, other);
			var rightNextDiscontinuity = solver.NextDiscontinuityAfter(
				after, binaryBubble.Right, other);

			// TODO: cache the result somehow
			return leftNextDiscontinuity < rightNextDiscontinuity
				? leftNextDiscontinuity
				: rightNextDiscontinuity;
		}

		/// <summary>
		/// Resolve the next discontinuity between a binary bubble and some other
		/// discontinuous bubble.
		/// </summary>
		/// <param name="solver">Solver.</param>
		/// <param name="after">After.</param>
		/// <param name="binaryBubble">Binary bubble.</param>
		/// <param name="other">Other.</param>
		public static void PairResolveDiscontinuityAfter(BubbleInteractionSolver<Position> solver,
			float after, BinaryBubble<Position> binaryBubble, DiscontinuousBubble<Position> other)
		{
			// TODO: cache this part from the previous function
			var leftNextDiscontinuity = solver.NextDiscontinuityAfter(
				after, binaryBubble.Left, other);
			var rightNextDiscontinuity = solver.NextDiscontinuityAfter(
				after, binaryBubble.Right, other);

			if (leftNextDiscontinuity < rightNextDiscontinuity)
			{
				solver.ResolveNextDiscontinuityAfter(after, binaryBubble.Left, other);
			}
			else 
			{
				solver.ResolveNextDiscontinuityAfter(after, binaryBubble.Right, other);
			}
		}

		/// <summary>
		/// Find the time at which the next discontinuity occurs between two binary
		/// discontinuous bubbles.
		/// </summary>
		/// <returns>The pair next discontinuity after.</returns>
		/// <param name="solver">Solver.</param>
		/// <param name="after">After.</param>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public static float BinaryPairNextDiscontinuityAfter(
			BubbleInteractionSolver<Position> solver, float after,
			BinaryBubble<Position> a, BinaryBubble<Position> b)
		{
			var aNextDiscontinuity = a.NextDiscontinuity();
			var bNextDiscontinuity = b.NextDiscontinuity();

			var pairs = new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>[]
			{
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Left, b.Left),
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Left, b.Right),
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Right, b.Left),
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Right, b.Right)
			};

			float nextDiscontinuity = aNextDiscontinuity < bNextDiscontinuity
				? aNextDiscontinuity : bNextDiscontinuity;

			foreach (var pair in pairs)
			{
				var pairDiscontinuity = solver.NextDiscontinuityAfter(
					after, pair.First, pair.Second);
				if (pairDiscontinuity < nextDiscontinuity)
				{
					nextDiscontinuity = pairDiscontinuity;
				}
			}

			return nextDiscontinuity;
		}

		/// <summary>
		/// Resolve the next discontinuity between two binary discontinuous bubbles.
		/// This may involve swapping individual bubbles between the binary bubbles.
		/// </summary>
		/// <param name="solver">Solver.</param>
		/// <param name="after">After.</param>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public static void BinaryPairResolveDiscontinuityAfter(
			BubbleInteractionSolver<Position> solver, float after,
			BinaryBubble<Position> a, BinaryBubble<Position> b)
		{
			// TODO: find a way of caching this result from the prior check
			// 		 (might need to be a change implemented in the solver)
			var aNextDiscontinuity = a.NextDiscontinuity();
			var bNextDiscontinuity = b.NextDiscontinuity();

			var pairs = new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>[]
			{
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Left, b.Left),
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Left, b.Right),
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Right, b.Left),
				new Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>>(a.Right, b.Right)
			};

			float nextDiscontinuity = aNextDiscontinuity < bNextDiscontinuity
				? aNextDiscontinuity : bNextDiscontinuity;
			Pair<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>> nextPair = null;

			foreach (var pair in pairs)
			{
				var pairDiscontinuity = solver.NextDiscontinuityAfter(
					after, pair.First, pair.Second);
				if (pairDiscontinuity < nextDiscontinuity)
				{
					nextDiscontinuity = pairDiscontinuity;
					nextPair = pair;
				}
			}

			if (nextPair == null)
			{
				if (aNextDiscontinuity < bNextDiscontinuity)
				{
					a.ResolveNextDiscontinuity();
				}
				else
				{
					b.ResolveNextDiscontinuity();
				}
			}
			else
			{
				solver.ResolveNextDiscontinuityAfter(after, nextPair.First, nextPair.Second);
				a.RecalculateProperties();
				b.RecalculateProperties();
			}
		}

		private enum DiscontinuityType { LEFT, RIGHT, PAIR }

		public DiscontinuousBubble<Position> Left, Right;

		private FunctionAdder functionAdder;
		private IBubbleInteractionSolver<Position> interactionSolver;

		private Position position = null;
		private MonotonicFunction boundaryRadius = null;
		private float leftDistanceFromOrigin = 0.0f;
		private float rightDistanceFromOrigin = 0.0f;

		private float? nextDiscontinuity = null;
		private DiscontinuityType? nextDiscontinuityType = null;

		public void Awake()
		{
			functionAdder = new FunctionAdder();
			interactionSolver = new BubbleInteractionSolver<Position>();
		}

		public override float GetMeasurementTime()
		{
			float leftMeasurementTime, rightMeasurementTime;
			leftMeasurementTime = Left.GetMeasurementTime();
			rightMeasurementTime = Right.GetMeasurementTime();
			if (leftMeasurementTime > rightMeasurementTime)
			{
				return leftMeasurementTime;
			}
			else
			{
				return rightMeasurementTime;
			}
		}

		public override Position GetPosition()
		{
			if (position == null)
			{
				UpdatePosition();
			}
			return position;
		}

		public override MonotonicFunction GetBoundaryRadius()
		{
			if (boundaryRadius == null)
			{
				UpdateBoundaryRadius();
			}
			return boundaryRadius;

		}

		private void UpdatePosition()
		{
			var leftPosition = Left.GetPosition();
			var rightPosition = Right.GetPosition();

			position = leftPosition.Midpoint(rightPosition);
			leftDistanceFromOrigin = leftPosition.SimilarityTo(position);
			rightDistanceFromOrigin = rightPosition.SimilarityTo(position);
		}

		private void UpdateBoundaryRadius()
		{
			if (position == null)
			{
				UpdatePosition();
			}
			boundaryRadius = functionAdder.SumMany(new MonotonicFunction[] {
				Left.GetBoundaryRadius(), new Constant(leftDistanceFromOrigin),
				Right.GetBoundaryRadius(), new Constant(rightDistanceFromOrigin)
			});
		}

		public override float NextDiscontinuity()
		{
			if (nextDiscontinuity == null)
			{
				UpdateDiscontinuityData();
			}
			return (float)nextDiscontinuity;
		}

		public override void ResolveNextDiscontinuity()
		{
			if (nextDiscontinuity == null)
			{
				UpdateDiscontinuityData();
			}
			switch (nextDiscontinuityType)
			{
				case DiscontinuityType.LEFT:
					Left.ResolveNextDiscontinuity();
					break;
				case DiscontinuityType.RIGHT:
					Right.ResolveNextDiscontinuity();
					break;
				case DiscontinuityType.PAIR:
					interactionSolver.ResolveNextDiscontinuityAfter(
						GetMeasurementTime(), Left, Right);
					break;
				default:
					throw new System.ArgumentException("unknown discontinuity type");
			}

			ResetProperties();
			RecalculateProperties();
		}

		private void ResetProperties()
		{
			position = null;
			boundaryRadius = null;
			nextDiscontinuity = null;
		}

		public void RecalculateProperties()
		{
			UpdatePosition();
			UpdateBoundaryRadius();
			UpdateDiscontinuityData();
		}

		private void UpdateDiscontinuityData()
		{
			var leftNextDiscontinuity = Left.NextDiscontinuity();
			var rightNextDiscontinuity = Right.NextDiscontinuity();
			var pairNextDiscontinuity = interactionSolver.NextDiscontinuityAfter(
				GetMeasurementTime(), Left, Right);

			if (leftNextDiscontinuity < rightNextDiscontinuity
				&& leftNextDiscontinuity < pairNextDiscontinuity)
			{
				nextDiscontinuity = leftNextDiscontinuity;
				nextDiscontinuityType = DiscontinuityType.LEFT;	
			}
			else if (rightNextDiscontinuity < pairNextDiscontinuity)
			{
				nextDiscontinuity = rightNextDiscontinuity;
				nextDiscontinuityType = DiscontinuityType.RIGHT;
			}
			else
			{
				nextDiscontinuity = pairNextDiscontinuity;
				nextDiscontinuityType = DiscontinuityType.PAIR;
			}
		}

		public override void SetStateContinuous(float t)
		{
			Left.SetStateContinuous(t);
			Right.SetStateContinuous(t);
		}

		public override void RemeasureContinuous(float t)
		{
			Left.RemeasureContinuous(t);
			Right.RemeasureContinuous(t);
			ResetProperties();
			RecalculateProperties();
		}

		public override void RebaseTime(float offset)
		{
			Left.RebaseTime(offset);
			Right.RebaseTime(offset);
		}

	}
}
