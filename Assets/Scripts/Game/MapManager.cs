using Ketsu.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class MapManager : MonoBehaviour
    {
        public string MapName;
        public int StartingKetsuPower;
        public IntVector2 MapSize;

        [HideInInspector]
		public static Map LoadedMap;

        void Awake()
        {
            LoadedMap = LoadMap(MapName, MapSize.X, MapSize.Y);
        }

        void Start()
        {
            CharacterController.KetsuPower += StartingKetsuPower;
        }

        void Update()
        {

        }
        
        public static Map LoadMap(string name, int width, int height)
        {
            // TODO:
            // Load map from <name>.json file from hardcoded resource path
            // Use size data from the json file

            Map map = new Map(width, height);

            // Find map objects and add them to the data structure
            foreach (MapObject obj in FindObjectsOfType<MapObject>())
            {
                obj.UpdatePosition();
                switch (obj.GetComponent<MapObject>().Layer)
                {
                    case MapLayer.Ground:
                        map.GroundLayer[obj.Position.X][obj.Position.Y] = obj;
                        break;
                    case MapLayer.Object:
                        map.ObjectLayer[obj.Position.X][obj.Position.Y] = obj;
                        break;
                    case MapLayer.Dynamic:
                        map.DynamicLayer.Add(obj);
                        break;

                }
            }

            return map;
        }
    }
}