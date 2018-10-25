namespace Bubbles.Templates
{
	/// <summary>
	/// Defines a data type which can be considered
	/// to be some kind of position, spatially, temporally,
	/// or otherwise, due to its implementation of a
	/// similarity metric.
	/// </summary>
	public interface IPosition<T> where T : IPosition<T>
	{

		/// <summary>
		/// Calculate the similarity of one point to a different point
		/// using the type's similarity metric.  A lower value means
		/// more similar points, where a similarity of 0 implies that
		/// the points are identical.
		/// </summary>
		/// <param name="other">Other.</param>
		float SimilarityTo(T other);

	}
}
