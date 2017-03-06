using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class MapObject : ExtendedMonoBehaviour
	{
		public MapObjectType Type = MapObjectType.Undefined;
		public MapLayer Layer = MapLayer.Dynamic;

		[HideInInspector]
		public IntVector2 Position;

		/// <summary>
		/// Update object's position from the object's world position
		/// </summary>
		public void UpdatePositionFromWorld()
		{
			Position = IntVector2.FromXZ(transform.position);
		}
	}
}