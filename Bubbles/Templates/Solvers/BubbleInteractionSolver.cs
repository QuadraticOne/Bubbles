using Bubbles.Templates;
using Bubbles.Util;
using Bubbles.MonotonicFunctions;
using Bubbles.MonotonicFunctions.Functions;
using System;

namespace Bubbles.Templates.Solvers
{
	/// <summary>
	/// Allows the registration of solvers, which resolve discontinuities between
	/// pairs of bubbles depending on what kind of class they are.
	/// </summary>
	public class BubbleInteractionSolver<Position> : IBubbleInteractionSolver<Position>
		where Position : IPosition<Position>
	{

		private CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float>
			discontinuityFinders;
		private CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, int>
			discontinuityResolvers;
		private CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float>
			broadPhaseCutoff;

		private FunctionAdder functionAdder;

		public BubbleInteractionSolver()
		{
			discontinuityFinders =
				new CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float>(
				(_1, _2) => float.PositiveInfinity);
			discontinuityResolvers =
				new CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, int>(
				(_1, _2) => 0);
			broadPhaseCutoff =
				new CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float>(
				(_1, _2) => 0.05f);

			functionAdder = new FunctionAdder();
		}

		public void AddInteraction<TypeA, TypeB>(
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float> nextDiscontinuity,
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, int> resolveDiscontinuity,
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float> broadPhaseCutoff = null)
			where TypeA : DiscontinuousBubble<Position>
			where TypeB : DiscontinuousBubble<Position>
		{
			discontinuityFinders.AddCheck(nextDiscontinuity);
			discontinuityResolvers.AddCheck(resolveDiscontinuity);

			if (broadPhaseCutoff != null)
			{
				this.broadPhaseCutoff.AddCheck(broadPhaseCutoff);
			}
		}

		public float NextDiscontinuityAfter(float t,
			DiscontinuousBubble<Position> a, DiscontinuousBubble<Position> b)
		{
			var t0 = NextPossibleInteractionAfter(t, a, b);
			if (t0 > broadPhaseCutoff.Check(a, b))
			{
				return t0;
			}
			else
			{
				return discontinuityFinders.Check(a, b);
			}
		}

		public void ResolveNextDiscontinuityAfter(float t,
			DiscontinuousBubble<Position> a, DiscontinuousBubble<Position> b)
		{
			discontinuityResolvers.Check(a, b);
		}

		public float NextPossibleInteractionAfter(float t,
			DiscontinuousBubble<Position> a, DiscontinuousBubble<Position> b)
		{
			var aBoundaryRadius = a.GetBoundaryRadius();
			var bBoundaryRadius = b.GetBoundaryRadius();

			var aCurrentRadius = aBoundaryRadius.At(t);
			var bCurrentRadius = bBoundaryRadius.At(t);

			var similarity = a.GetPosition().SimilarityTo(b.GetPosition());

			if (similarity < aCurrentRadius + bCurrentRadius)
			{
				// Bubbles' regions of influence already overlap
				return float.NegativeInfinity;
			}

			var jointRadius = functionAdder.Sum(aBoundaryRadius, bBoundaryRadius);

			return (functionAdder.Sum(jointRadius,
				new Constant(-(aCurrentRadius + bCurrentRadius)))).Root()
				?? float.PositiveInfinity;
		}

	}
}
