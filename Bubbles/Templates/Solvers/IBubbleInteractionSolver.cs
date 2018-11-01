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
		/// before returning 0.  If a broad phase cutoff function is given for the pair, a
		/// detailed discontinuity check will only be done if the two bubbles' regions of
		/// influence will interact before t + k, where k is the output of the broad phase
		/// cutoff function.
		/// </summary>
		/// <param name="nextDiscontinuity">Next discontinuity.</param>
		/// <param name="resolveDiscontinuity">Resolve discontinuity.</param>
		/// <typeparam name="TypeA">The 1st type parameter.</typeparam>
		/// <typeparam name="TypeB">The 2nd type parameter.</typeparam>
		void AddInteraction<TypeA, TypeB>(
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float> nextDiscontinuity,
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, int> resolveDiscontinuity,
			Func<DiscontinuousBubble<Position>, DiscontinuousBubble<Position>, float> broadPhaseCutoff = null)
			where TypeA : DiscontinuousBubble<Position>
			where TypeB : DiscontinuousBubble<Position>;

		/// <summary>
		/// Work out when the next discontinuity between the two bubbles occurs.  If no
		/// discontinuity occurs at all past the given time, returns positive infinity.
		/// </summary>
		/// <returns>The discontinuity.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		float NextDiscontinuityAfter(float t,
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b);

		/// <summary>
		/// Resolves the next discontinuity that results from interactions between the
		/// two bubbles.  The behaviour of this method is undefined if there is never
		/// any interaction between them after the given time.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		void ResolveNextDiscontinuityAfter(float t,
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b);

		/// <summary>
		/// Return the time at which the bubbles could next interact after the given time,
		/// based on their boundary radii.  If both radii are bounded in time, return
		/// positive infinity.  If the bubbles' regions of influence already overlap,
		/// return negative infinity.
		/// </summary>
		/// <returns>The possible interaction.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		float NextPossibleInteractionAfter(float t,
			DiscontinuousBubble<Position> a,
			DiscontinuousBubble<Position> b);

	}
}
