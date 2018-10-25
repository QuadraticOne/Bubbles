using Bubbles.Util;
using Bubbles.MonotonicFunctions.Functions;
using UnityEngine;
using System;

namespace Bubbles.MonotonicFunctions
{
	/// <summary>
	/// Defines a function which has at least some interval on its domain
	/// for which its value either increases or remains constant.
	/// </summary>
	public class FunctionAdder : CrossCheck<MonotonicFunction, MonotonicFunction, MonotonicFunction>
	{

		/// <summary>
		/// Create a new FunctionAdder, which can be used to efficiently sum
		/// together monotonic functions.
		/// </summary>
		public FunctionAdder() : base()
		{
			AddCheck<Constant, Constant>((f, g) => SumConstantConstant(f, g));
			AddCheck<Linear, Linear>((f, g) => SumLinearLinear(f, g));
			AddCheck<Quadratic, Quadratic>((f, g) => SumQuadraticQuadratic(f, g));

			AddCheckWithReverse<Linear, Constant>((f, g) => SumLinearConstant(f, g));
			AddCheckWithReverse<Quadratic, Constant>((f, g) => SumQuadraticConstant(f, g));
			AddCheckWithReverse<Quadratic, Linear>((f, g) => SumQuadraticLinear(f, g));
		}

		/// <summary>
		/// Sum the two input functions and return the result.
		/// </summary>
		/// <param name="f">F.</param>
		/// <param name="g">The green component.</param>
		public MonotonicFunction Sum(MonotonicFunction f, MonotonicFunction g)
		{
			return Check(f, g).GetResult();
		}

		/// <summary>
		/// Add the given check, then add the same check again but with the
		/// arguments reversed.
		/// </summary>
		/// <param name="checkFunc">Check func.</param>
		/// <typeparam name="ChildA">The 1st type parameter.</typeparam>
		/// <typeparam name="ChildB">The 2nd type parameter.</typeparam>
		private void AddCheckWithReverse<ChildA, ChildB>(
			Func<ChildA, ChildB, MonotonicFunction> checkFunc)
			where ChildA : class
			where ChildB : class
		{
			AddCheck<ChildA, ChildB>((f, g) => checkFunc(f, g));
			AddCheck<ChildB, ChildA>((g, f) => checkFunc(f, g));
		}

		private static MonotonicFunction SumConstantConstant(Constant a, Constant b)
		{
			return new Constant(a.Value + b.Value);
		}

		private static MonotonicFunction SumLinearConstant(Linear l, Constant c)
		{
			return new Linear(l.Gradient, l.Intercept + c.Value);
		}

		private static MonotonicFunction SumLinearLinear(Linear a, Linear b)
		{
			// Gradients cannot be < 0 so no need to check for cancelling values
			return new Linear(a.Gradient + b.Gradient, a.Intercept + b.Intercept);
		}

		private static MonotonicFunction SumQuadraticConstant(Quadratic q, Constant c)
		{
			return new Quadratic(q.QuadraticCoefficient, q.LinearCoefficient,
				q.ConstantCoefficient + c.Value);
		}

		private static MonotonicFunction SumQuadraticLinear(Quadratic q, Linear l)
		{
			return new Quadratic(q.QuadraticCoefficient, q.LinearCoefficient + l.Gradient,
				q.ConstantCoefficient + l.Intercept);
		}

		private static MonotonicFunction SumQuadraticQuadratic(Quadratic a, Quadratic b)
		{
			if (a.QuadraticCoefficient + b.QuadraticCoefficient == 0f)
			{
				return new Linear(
					a.LinearCoefficient + b.LinearCoefficient,
					a.ConstantCoefficient + b.ConstantCoefficient);
			}
			else
			{
				return new Quadratic(
					a.QuadraticCoefficient + b.QuadraticCoefficient,
					a.LinearCoefficient + b.LinearCoefficient,
					a.ConstantCoefficient + b.ConstantCoefficient);
			}
		}

	}
}
