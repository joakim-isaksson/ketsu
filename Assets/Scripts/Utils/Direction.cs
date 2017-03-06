using UnityEngine;

namespace Ketsu.Utils
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
				case Direction.Left: return Vector3.left;
				case Direction.Right: return Vector3.right;
				case Direction.Forward: return Vector3.forward;
				case Direction.Back: return Vector3.back;
				default: return Vector3.zero;
			}
		}

		public static IntVector2 ToIntVector2(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Left: return IntVector2.Left;
				case Direction.Right: return IntVector2.Right;
				case Direction.Forward: return IntVector2.Forward;
				case Direction.Back: return IntVector2.Back;
				default: return IntVector2.Zero;
			}
		}
	}
}