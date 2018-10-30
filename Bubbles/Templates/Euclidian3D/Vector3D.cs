using UnityEngine;

namespace Bubbles.Templates.Euclidian3D
{
	public class Vector3D : IPosition<Vector3D>
	{

		public Vector3 Vector;

		public Vector3D(Vector3 vector)
		{
			Vector = vector;
		}

		public float SimilarityTo(Vector3D other)
		{
			return (this.Vector - other.Vector).magnitude;
		}

	}
}
