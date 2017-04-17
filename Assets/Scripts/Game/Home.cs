using UnityEngine;

namespace Game
{
	public class Home : MapObject
	{
		public MapObjectType ForType;

        [HideInInspector]
		public int Occupants;

		void Awake()
		{

		}

		void Start()
		{

		}

		void OnTriggerEnter(Collider other)
		{
            MapObject obj = other.GetComponent<MapObject>();
            if (obj != null && obj.Type == ForType) Occupants++;
        }

		private void OnTriggerExit(Collider other)
		{
            MapObject obj = other.GetComponent<MapObject>();
            if (obj != null && obj.Type == ForType) Occupants--;
		}
	}
}