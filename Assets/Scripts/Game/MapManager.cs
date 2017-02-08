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
        public static void DestroySingleton()
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }

        void Awake()
        {
            // Make this an indestructible singleton
            if (Instance == null) Instance = this;
            else if (!Instance.Equals(this)) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
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

            // Initialize the map object data structure
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Ground"))
            {
                MapObject ground = obj.GetComponent<MapObject>();
                CurrentMap.Obstacles[ground.Position.X][ground.Position.Y] = ground;
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                MapObject obstacle = obj.GetComponent<MapObject>();
                CurrentMap.Obstacles[obstacle.Position.X][obstacle.Position.Y] = obstacle;
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                MapObject mapObject = obj.GetComponent<MapObject>();
                CurrentMap.Objects.Add(mapObject);
            }
        }
    }
}