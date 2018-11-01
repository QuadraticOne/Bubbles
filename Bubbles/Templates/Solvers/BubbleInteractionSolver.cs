using Bubbles.Templates;
using Bubbles.Util;
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

		public BubbleInteractionSolver()
		{
			discontinuityFinders =
				new CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float>(
				(_1, _2) => float.PositiveInfinity);
			discontinuityResolvers =
				new CrossCheck<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, int>(
				(_1, _2) => 0);
		}

		public void AddInteraction<TypeA, TypeB>(
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float> nextDiscontinuity,
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, int> resolveDiscontinuity)
			where TypeA : DiscontinuousBubble<Position>
			where TypeB : DiscontinuousBubble<Position>
		{
			discontinuityFinders.AddCheck(nextDiscontinuity);
			discontinuityResolvers.AddCheck(resolveDiscontinuity);
		}

		public float NextDiscontinuity(
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b)
		{
			var output = discontinuityFinders.Check(a, b);
			return output;
		}

		public void ResolveNextDiscontinuity(
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b)
		{
			discontinuityResolvers.Check(a, b);
		}

	}
}
