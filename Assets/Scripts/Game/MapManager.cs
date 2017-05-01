using System.Collections.Generic;
using Ketsu.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	public class MapManager : MonoBehaviour
	{
	    public string SfxMapSolved;

		[HideInInspector]
		public bool Solved;
		public bool Split;

		Home[] homes;

		void Awake()
		{
			Solved = false;
			Split = false;
		}

		void Start()
		{
            homes = FindObjectsOfType<Home>();

            ScreenFaider.Instance.FadeOut(1.0f, null);

            //save current level to level selection
            LevelSelection.currentLevel = SceneManager.GetActiveScene().buildIndex-1;

		}

        /// <summary>
        /// Get all active MapObjects from given point
        /// </summary>
        /// <param name="point">Point to get the MapObjects from</param>
        /// <returns>List of MapObjects from given point</returns>
        public static List<MapObject> GetObjects(Vector3 point)
        {
            return GetObjects(point, false, 0);
        }

        /// <summary>
        /// Get all active MapObjects from given point and layer
        /// </summary>
        /// <param name="point">Point to get the MapObjects from</param>
        /// <param name="layer">Get objects from given layer</param>
        /// <returns>List of MapObjects from given point</returns>
        public static List<MapObject> GetObjects(Vector3 point, MapObjectLayer layer)
        {
            return GetObjects(point, true, layer);
        }

        static List<MapObject> GetObjects(Vector3 point, bool filter, MapObjectLayer layer)
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
				if (obj != null && obj.gameObject.activeSelf && (!filter || obj.Layer == layer)) list.Add(obj);
			}

			return list;
		}

        public bool CheckSolved()
        {
            if (Solved) return true;

			foreach (Home home in homes)
			{
                if (home.Occupants == 0)
                {
                    Solved = false;
                    return false;
                }
			}

		    if (Gas.GasToCollect > 0) return false;
            
            AkSoundEngine.PostEvent(SfxMapSolved, gameObject);

            Solved = true;
            return true;
		}
	}
}