namespace Ketsu
{
    [System.Serializable]
    public struct IntVector2
    {
        public readonly int X;
        public readonly int Y;

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
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