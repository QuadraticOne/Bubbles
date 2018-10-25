using Bubbles.Util;
using Bubbles.MonotonicFunctions;

namespace Bubbles.MonotonicFunctions.Functions
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

	}
}
