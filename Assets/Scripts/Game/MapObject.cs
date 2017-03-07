using Ketsu.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class MapObject : ExtendedMonoBehaviour
	{
		[Header("Map Object")]
		public MapObjectType Type;
		public MapObjectLayer Layer;

		public List<MapObjectType> BlockedByTypes;

        /// <summary>
		/// Check if this object is blocked by the type of given object
		/// </summary>
		/// <param name="obj">Object to check</param>
		/// <returns>True if blocking, false otherwise</returns>
		public bool IsBlockedByType(MapObject obj)
        {
            if (obj != null && BlockedByTypes.Contains(obj.Type)) return true;
            else return false;
        }

        /// <summary>
        /// Check if this object is blocked in the given point
        /// </summary>
        /// <param name="point">Point to check for blocking objects</param>
        /// <returns>True if something is blocking, false otherwise</returns>
        public bool IsBlocked(Vector3 point)
		{
			if (!MapManager.Contains(point)) return true;

			List<MapObject> objects = MapManager.GetObjects(point);
			foreach (MapObject obj in objects)
			{
				if (obj != this && BlockedByTypes.Contains(obj.Type)) return true;
			}

			return false;
		}

		/// <summary>
		/// Return blocking object from given list or null if nothing is blocking
		/// </summary>
		/// <param name="point">Point to check for blocking objects</param>
		/// <returns>Blocking object or null if nothing is blocking</returns>
		public MapObject GetBlocking(Vector3 point)
		{
			List<MapObject> objects = MapManager.GetObjects(point);
			foreach (MapObject obj in objects)
			{
				if (obj != this && BlockedByTypes.Contains(obj.Type)) return obj;
			}

            return null;
		}
	}
}