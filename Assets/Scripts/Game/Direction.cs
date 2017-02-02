using UnityEngine;

namespace Ketsu.Game
{
    public enum Direction
    {
        Left, Right, Forward, Back
    }

	public static class Extensions
	{
		public static Direction Opposite(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Left: return Direction.Right;
				case Direction.Right: return Direction.Left;
				case Direction.Forward: return Direction.Back;
				case Direction.Back: return Direction.Forward;
				default: return direction;
			}
		}

		public static Vector3 ToVector3(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Left: return Vector3.right;
				case Direction.Right: return Vector3.left;
				case Direction.Forward: return Vector3.back;
				case Direction.Back: return Vector3.forward;
				default: return Vector3.zero;
			}
		}
	}
}