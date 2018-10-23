namespace Bubbles.Util
{
	public class Pair<T1, T2>
	{
		public readonly T1 First;
		public readonly T2 Second;

		public Pair(T1 first, T2 second)
		{
			First = first;
			Second = second;
		}

		public override string ToString ()
		{
			return "(" + First.ToString() + ", " + Second.ToString() + ")";
		}
	}
}
