using Bubbles.Util;

namespace Bubbles.MonotonicFunctions
{
	public class Constant : MonotonicFunction
	{

		public readonly float Value;

		public Constant(float value)
		{
			Value = value;
		}

		public override float At(float x)
		{
			return Value;
		}

		public override float DerivativeAt(float x)
		{
			return 0f;
		}

		public override float SecondDerivativeAt(float x)
		{
			return 0f;
		}

		public virtual Maybe<Interval> BoundZero(int maxExpansionIterations = 20,
			int expansionFactor = 3, int maxContractionIterations = 20,
			float maximumIntervalWidth = 0.01f)
		{
			return Value == 0f
				? new Maybe<Interval>(Domain())
				: new Maybe<Interval>();
		}

	}
}
