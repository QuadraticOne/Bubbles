using UnityEngine;
using System;

namespace Bubbles.Templates
{
	/// <summary>
	/// Defines a vector in an n-dimensional space.
	/// </summary>
	public class VectorN : IPosition<VectorN>
	{

		public readonly int Dimension;
		private float[] values;

		public VectorN(int n)
		{
			Dimension = n;
			values = new float[n];
		}

		public VectorN(float[] values)
		{
			this.values = values;
			Dimension = values.Length;
		}

		public VectorN(float x, float y) : this(new float[] {x, y}) {}

		public VectorN(float x, float y, float z) : this(new float[] {x, y, z}) {}

		public float this[int index]
		{
			get
			{
				return values[index];
			}

			set
			{
				values[index] = value;
			}
		}

		public float SimilarityTo(VectorN other)
		{
			float total = 0f;
			for (int i = 0; i < Dimension; i++)
			{
				float difference = this[i] - other[i];
				total += difference * difference;
			}
			return Mathf.Sqrt(total);
		}

		/// <summary>
		/// Map each element of the vector to a new value according
		/// to some function.
		/// </summary>
		/// <param name="f">F.</param>
		public VectorN Map(Func<float, float> f)
		{
			float[] mapped = new float[Dimension];
			for (int i = 0; i < Dimension; i++)
			{
				mapped[i] = f(this[i]);
			}
			return new VectorN(mapped);
		}

		/// <summary>
		/// Map each pair of elements from this vector and another vector
		/// to a new vector, with values being calculated elementwise by
		/// a given function.
		/// </summary>
		/// <returns>The with.</returns>
		/// <param name="f">F.</param>
		/// <param name="other">Other.</param>
		public VectorN MapWith(VectorN other, Func<float, float, float> f)
		{
			float[] mapped = new float[Dimension];
			for (int i = 0; i < Dimension; i++)
			{
				mapped[i] = f(this[i], other[i]);
			}
			return new VectorN(mapped);
		}

		/// <summary>
		/// Calculate the magnitude of the vector.
		/// </summary>
		/// <value>The magnitude.</value>
		public float Magnitude
		{
			get
			{
				float total = 0f;
				foreach (var s in values)
				{
					total += s * s;
				}
				return Mathf.Sqrt(total);
			}
		}

		/// <summary>
		/// The value of the 0th dimension.
		/// </summary>
		/// <value>The x.</value>
		public float x
		{
			get
			{
				return values[0];
			}
		}

		/// <summary>
		/// The value of the 1st dimension.
		/// </summary>
		/// <value>The y.</value>
		public float y
		{
			get
			{
				return values[1];
			}
		}

		/// <summary>
		/// The value of the 2nd dimension.
		/// </summary>
		/// <value>The z.</value>
		public float z
		{
			get
			{
				return values[2];
			}
		}

		public Vector2 ToVector2()
		{
			return new Vector2(x, y);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(x, y, z);
		}

		public static VectorN operator+(VectorN a, VectorN b)
		{
			return a.MapWith(b, (x, y) => x + y);
		}

		public static VectorN operator-(VectorN a, VectorN b)
		{
			return a.MapWith(b, (x, y) => x - y);
		}

		public static VectorN operator*(VectorN v, float k)
		{
			return v.Map(x => x * k);
		}

		public static VectorN operator/(VectorN v, float k)
		{
			return v.Map(x => x / k);
		}

	}
}
