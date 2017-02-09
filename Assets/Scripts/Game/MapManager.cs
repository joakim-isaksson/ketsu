using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class MapManager : MonoBehaviour
    {
        [HideInInspector]
        public static MapManager Instance = null;

		public Map CurrentMap;

        /// <summary>
        /// Destroy this singleton instance
        /// </summary>
        /*public static void DestroySingleton()
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }*/

        void Awake()
        {
            // Make this an indestructible singleton
            if (Instance == null) Instance = this;
            else if (!Instance.Equals(this)) Destroy(gameObject);
            //DontDestroyOnLoad(gameObject);

            LoadMap("TestMap");
        }

        void Start()
        {
            
        }

        void Update()
        {

        }
        
        public void LoadMap(string name)
        {
            // TODO:
            // Load map from <name>.json file from hardcoded resource path

            // TODO: Use size data from the json file
            CurrentMap = new Map(16, 12);

            // Find map objects and add them to the data structure
            foreach (MapObject obj in FindObjectsOfType<MapObject>())
            {

                switch (obj.GetComponent<MapObject>().Layer)
                {
                    case MapLayer.Ground:
                        CurrentMap.GroundLayer[obj.Position.X][obj.Position.Y] = obj;
                        break;
                    case MapLayer.Object:
                        CurrentMap.ObjectLayer[obj.Position.X][obj.Position.Y] = obj;
                        break;
                    case MapLayer.Dynamic:
                        CurrentMap.DynamicLayer.Add(obj);
                        break;

                }
            }
        }
    }
}