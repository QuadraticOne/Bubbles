using System;
using System.Collections.Generic;

namespace Bubbles.Util
{
	/// <summary>
	/// Allows the user to register checks which are defined only for certain
	/// pairs of subclasses of A and B, and then run these checks against
	/// concrete objects which may inherit from A and B.  The checks are stored
	/// in the order in which they are created, and the first matching check
	/// will return a value.
	/// </summary>
	public class CrossCheck<ParentA, ParentB, TResult>
	{

		private List<Func<ParentA, ParentB, Maybe<TResult>>> checks;
		private Func<ParentA, ParentB, TResult> fallback;

		/// <summary>
		/// Create a new CrossCheck class.
		/// </summary>
		public CrossCheck(Func<ParentA, ParentB, TResult> fallback = null)
		{
			checks = new List<Func<ParentA, ParentB, Maybe<TResult>>>();
			if (fallback == null)
			{
				this.fallback = (_1, _2) => default(TResult);
			}
			else
			{
				this.fallback = fallback;
			}
		}

		/// <summary>
		/// Register a check for two objects of certain subclasses, and the result of
		/// the check if it runs for a pair of objects.
		/// </summary>
		/// <param name="checkFunc">Check func.</param>
		/// <typeparam name="ChildA">The 1st type parameter.</typeparam>
		/// <typeparam name="ChildB">The 2nd type parameter.</typeparam>
		public void AddCheck<ChildA, ChildB>(Func<ChildA, ChildB, TResult> checkFunc)
			where ChildA : class
			where ChildB : class
		{
			Func<ParentA, ParentB, Maybe<TResult>> checkFuncRealised = (a, b) =>
				(a is ChildA) && (b is ChildB)
					? new Maybe<TResult>(checkFunc(a as ChildA, b as ChildB))
					: new Maybe<TResult>();
			checks.Add(checkFuncRealised);
		}

		/// <summary>
		/// Run each registered check against each of the given objects if the
		/// check is valid for that pair.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public TResult Check(ParentA a, ParentB b)
		{
			foreach (var checkFunc in checks)
			{
				Maybe<TResult> result = checkFunc(a, b);
				if (result.HasResult())
				{
					return result.GetResult();
				}
			}
			return fallback(a, b);
		}

	}
}
