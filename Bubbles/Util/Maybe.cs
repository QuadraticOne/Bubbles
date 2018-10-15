using System;

namespace Bubbles.Util
{
	/// <summary>
	/// Stores the result of a computation which may fail.
	/// </summary>
	public class Maybe<T>
	{

		private bool hasResult;
		private T result;

		/// <summary>
		/// Create a Maybe value which stores nothing.
		/// </summary>
		public Maybe()
		{
			hasResult = false;
			result = default(T);
		}

		/// <summary>
		/// Create a Maybe value which stores a concrete result.
		/// </summary>
		/// <param name="result">Result.</param>
		public Maybe(T result)
		{
			hasResult = true;
			this.result = result;
		}

		/// <summary>
		/// Return true iff the computation gave a valid result.
		/// </summary>
		/// <returns><c>true</c> if this instance has result; otherwise, <c>false</c>.</returns>
		public bool HasResult()
		{
			return hasResult;
		}

		/// <summary>
		/// Retrieve the result of the computation.  Does not check if there actually
		/// was a result first.
		/// </summary>
		/// <returns>The result.</returns>
		public T GetResult()
		{
			return result;
		}

		/// <summary>
		/// Retrieve the result of the computation after checking that a result exists.
		/// Will throw an exception if used on a Maybe that does not contain a result.
		/// </summary>
		/// <returns>The result safe.</returns>
		public T GetResultSafe()
		{
			if (!hasResult)
			{
				throw new System.Exception(
					"tried to access a maybe with no result");
			}
			return result;
		}

		/// <summary>
		/// Map the contents of the monad according to some transfer function.  If
		/// the Maybe does not contain a result, nor will the output of this function.
		/// </summary>
		/// <returns>The map.</returns>
		/// <param name="transferFunction">Transfer function.</param>
		/// <typeparam name="U">The 1st type parameter.</typeparam>
		public Maybe<U> FMap<U>(Func<T, U> transferFunction)
		{
			if (hasResult)
			{
				return new Maybe<U>(transferFunction(result));
			}
			else
			{
				return new Maybe<U>();
			}
		}

	}
}
