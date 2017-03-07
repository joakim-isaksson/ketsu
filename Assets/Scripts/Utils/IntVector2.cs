using UnityEngine;

namespace Ketsu.Utils
{
	[System.Serializable]
	public class IntVector2
	{
        // FIXME: Do not like these -> is not generic
		public static readonly IntVector2 Zero = new IntVector2(0, 0);
		public static readonly IntVector2 Left = new IntVector2(-1, 0);
		public static readonly IntVector2 Right = new IntVector2(1, 0);
		public static readonly IntVector2 Forward = new IntVector2(0, 1);
		public static readonly IntVector2 Back = new IntVector2(0, -1);

		public int X;
		public int Y;

		public IntVector2(int x, int y)
		{
			X = x;
			Y = y;
		}

		public IntVector2 Add(IntVector2 vector)
		{
			return new IntVector2(X + vector.X, Y + vector.Y);
		}

		public IntVector2 Mirror(IntVector2 point)
		{
			return new IntVector2(
				point.X - (X - point.X),
				point.Y - (Y - point.Y)
			);
		}

        public static IntVector2 FromXY(Vector3 vector3)
        {
            return new IntVector2(
                (int)Mathf.Round(vector3.x),
                (int)Mathf.Round(vector3.y)
            );
        }

        public static IntVector2 FromXZ(Vector3 vector3)
		{
			return new IntVector2(
				(int)Mathf.Round(vector3.x),
				(int)Mathf.Round(vector3.z)
			);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;

			IntVector2 other = (IntVector2)obj;

			return (X == other.X && Y == other.Y);
		}

		public override int GetHashCode()
		{
			int hash = 17;
			unchecked
			{
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
			}
			return hash;
		}

		public override string ToString()
		{
			return "{" + X + ", " + Y + "}";
		}
	}
}