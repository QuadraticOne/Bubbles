﻿using System.Collections.Generic;
using Bubbles.Util;

namespace Bubbles.MonotonicFunctions
{
	public class SummedFunction : MonotonicFunction
	{

		private IEnumerable<MonotonicFunction> functions;
		private Maybe<Interval> domain;

		private SummedFunction(IEnumerable<MonotonicFunction> functions)
		{
			this.functions = functions;
			this.domain = new Maybe<Interval>(
				new Interval(float.NegativeInfinity, float.PositiveInfinity));
			foreach (var function in this.functions)
			{
				if (this.domain.HasResult())
				{
					this.domain = this.domain.GetResult().IntersectionWith(function.Domain());
				}
			}
		}

		/// <summary>
		/// Returns the valid domain of the function, or throws an exception if
		/// the valid domain is a null set.
		/// </summary>
		/// <returns>The interval.</returns>
		public override Interval Domain()
		{
			return domain.GetResultSafe();
		}

		public override float At(float x)
		{
			float value = 0;
			foreach (var function in functions)
			{
				value += function.At(x);
			}
			return value;
		}

		public override float DerivativeAt(float x)
		{
			float value = 0;
			foreach (var function in functions)
			{
				value += function.DerivativeAt(x);
			}
			return value;
		}

		public override float SecondDerivativeAt(float x)
		{
			float value = 0;
			foreach (var function in functions)
			{
				value += function.SecondDerivativeAt(x);
			}
			return value;
		}

		/// <summary>
		/// Return a monotonic function which is the sum of two other monotonic
		/// functions, which will be valid on the domain that is common to both
		/// domains of the input functions.
		/// </summary>
		/// <returns>The of pair.</returns>
		/// <param name="addend">Addend.</param>
		/// <param name="augend">Augend.</param>
		public static SummedFunction SumOfPair(
			MonotonicFunction addend, MonotonicFunction augend)
		{
			var functions = new List<MonotonicFunction>();
			functions.Add(addend);
			functions.Add(augend);
			return new SummedFunction(functions);
		}

		/// <summary>
		/// Return a monotonic function which is the sum of a number of other
		/// monotonic functions, which will be valid on the domain that is
		/// common to the domains of all the input functions.
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="">.</param>
		public static SummedFunction SumOf(IEnumerable<MonotonicFunction> functions)
		{
			return new SummedFunction(functions);
		}

	}
}
