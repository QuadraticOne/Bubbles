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

		/// <summary>
		/// Add a check for an interaction between two different kinds of bubble.  A check
		/// consists of two parts: the first determines the timestamp at which the next
		/// discontinuity occurs; the second remeasures each of the bubbles immediately
		/// before that discontinuity, then updates the properties of each bubble's system
		/// that change across the discontinuity, then remeasures the systems again,
		/// before returning 0.
		/// </summary>
		/// <param name="nextDiscontinuity">Next discontinuity.</param>
		/// <param name="resolveDiscontinuity">Resolve discontinuity.</param>
		/// <typeparam name="TypeA">The 1st type parameter.</typeparam>
		/// <typeparam name="TypeB">The 2nd type parameter.</typeparam>
		void AddInteraction<TypeA, TypeB>(
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float> nextDiscontinuity,
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, int> resolveDiscontinuity)
			where TypeA : DiscontinuousBubble<Position>
			where TypeB : DiscontinuousBubble<Position>;

		/// <summary>
		/// Work out when the next discontinuity between the two bubbles occurs.  If no
		/// discontinuity occurs at all, returns positive infinity.
		/// </summary>
		/// <returns>The discontinuity.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		float NextDiscontinuity(
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b);

		/// <summary>
		/// Resolves the next discontinuity that results from interactions between the
		/// two bubbles.  The behaviour of this method is undefined if there is never
		/// any interaction between them.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		void ResolveNextDiscontinuity(
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b);

	}
}
