using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.Game
{
	public class MapManager : MonoBehaviour
	{
		public static Vector3 Boarders;

		public Text WinText;

		[HideInInspector]
		public static bool Solved { get; private set; }

		Home[] homes;

		public void Awake()
		{
			homes = FindObjectsOfType<Home>();

			Solved = false;
			WinText.gameObject.SetActive(false);
		}

		public static bool Contains(Vector3 point)
		{
			if (point.x < 0 || point.x >= Boarders.x || point.z < 0 || point.z >= Boarders.z) return false;
			else return true;
		}

        /// <summary>
        /// Get all MapObjects from given point and layer
        /// </summary>
        /// <param name="point">Point to get the MapObjects from</param>
        /// <param name="layer">Get objects from given layer</param>
        /// <returns>List of MapObjects from given point</returns>
        public static List<MapObject> GetObjects(Vector3 point, int layer)
		{
			List<MapObject> list = new List<MapObject>();

			RaycastHit[] hits = Physics.RaycastAll(
				new Vector3(point.x, Camera.main.transform.position.y, point.z),
				Vector3.down,
				Camera.main.farClipPlane
			);

			foreach(RaycastHit hit in hits)
			{
				MapObject obj = hit.collider.GetComponent<MapObject>();
				if (obj != null && obj.Layer == layer) list.Add(obj);
			}

			return list;
		}

		public void CheckSolved()
		{
			foreach (Home home in homes)
			{
				if (home.Inside == null) return;
			}

			OnMapSolved();
		}

		void OnMapSolved()
		{
			if (Solved) return;
			Solved = true;

			WinText.gameObject.SetActive(true);
		}
		
	}
}