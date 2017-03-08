namespace Ketsu.Utils
{
	[System.Serializable]
	public class IntVector2
	{
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