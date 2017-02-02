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
            // load data from json file instead of fetching from the scene
            // Instantiate object to scene from prefabs

            CurrentMap = new Map(16, 12);
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                MapObject obstacle = obj.GetComponent<MapObject>();
                CurrentMap.Obstacles[obstacle.Position.Y][obstacle.Position.X] = obstacle;
            }
        }
    }
}