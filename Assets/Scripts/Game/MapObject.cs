using System.Collections.Generic;
using Ketsu.Utils;
using UnityEngine;

namespace Game
{
	public class MapObject : ExtendedMonoBehaviour
	{
		[Header("Map Object")]
		public MapObjectType Type;
		public MapObjectLayer Layer;

		public List<MapObjectType> NotBlockedBy;

        /// <summary>
		/// Check if this object is blocked by the type of given object
		/// </summary>
		/// <param name="obj">Object to check</param>
		/// <returns>True if blocking, false otherwise</returns>
		public bool IsBlockedBy(MapObject obj)
        {
            if (obj == null || Layer != obj.Layer || !obj.gameObject.activeSelf || NotBlockedBy.Contains(obj.Type)) return false;
            else return true;
        }
    
        /// <summary>
        /// Check if this object is blocked in the given point
        /// </summary>
        /// <param name="point">Point to check for blocking objects</param>
        /// <returns>True if something is blocking, false otherwise</returns>
        public bool IsBlocked(Vector3 point)
		{
			List<MapObject> objects = MapManager.GetObjects(point, Layer);
			foreach (MapObject obj in objects)
			{
                if (!obj.gameObject.activeSelf || NotBlockedBy.Contains(obj.Type)) continue;
                else return true;
            }

			return false;
		}
    }
}