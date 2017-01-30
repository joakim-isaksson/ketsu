using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Map
{
    public class MapManager : MonoBehaviour
    {
        [HideInInspector]
        public static MapManager Instance = null;

        [HideInInspector]
        public int Width;
        [HideInInspector]
        public int Height;
        [HideInInspector]
        public List<List<Tile>> Tiles;

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

        // TODO:
        // load data from json file
        // Instantiate object to scene from prefabs
        public void LoadMap(string name)
        {
            Width = 16;
            Height = 12;

            Tiles = new List<List<Tile>>();
            for (int y = 0; y < Height; ++y)
            {
                List<Tile> row = new List<Tile>();
                for (int x = 0; x < Width; ++x)
                {
                    row.Add(new Tile());
                }
                Tiles.Add(row);
            }

            Tiles[8][6].Blocked = true;
        }
    }
}