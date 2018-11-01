using Bubbles.Templates;
using System;

namespace Bubbles.Templates.Solvers
{
	/// <summary>
	/// Defines the behaviours required of a class which is capable of resolving
	/// discontinuities between pairs of bubbles.
	/// </summary>
	public interface IBubbleInteractionSolver<Position>
		where Position : IPosition<Position>
	{

		void AddInteraction<TypeA, TypeB>(
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float> nextDiscontinuity,
			Action<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>> resolveDiscontinuity)
			where TypeA : DiscontinuousBubble<Position>
			where TypeB : DiscontinuousBubble<Position>;

		float NextDiscontinuity(
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b);

		void ResolveNextDiscontinuity(
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b);

	}
}
