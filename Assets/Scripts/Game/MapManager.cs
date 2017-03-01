using Ketsu.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.Game
{
    public class MapManager : MonoBehaviour
    {
        public string MapName;
        public int StartingKetsuPower;
        public IntVector2 MapSize;

        public Text WinText;

        [HideInInspector]
        public bool Solved { get; private set; }

        [HideInInspector]
		public static Map LoadedMap;

        CharHome[] charHomes;

        void Awake()
        {
            LoadedMap = LoadMap(MapName, MapSize.X, MapSize.Y);
            WinText.gameObject.SetActive(false);
        }

        void Start()
        {
            CharController controller = FindObjectOfType<CharController>();
            controller.KetsuPower += StartingKetsuPower;

            charHomes = FindObjectsOfType<CharHome>();
        }

        void Update()
        {

        }
        
        public Map LoadMap(string name, int width, int height)
        {
            // TODO:
            // Load map from <name>.json file from hardcoded resource path
            // Use size data from the json file

            Map map = new Map(width, height);

            // Find map objects and add them to the data structure
            foreach (MapObject obj in FindObjectsOfType<MapObject>())
            {
                obj.UpdatePositionFromWorld();
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

        public void CheckSolved()
        {
            foreach (CharHome home in charHomes)
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