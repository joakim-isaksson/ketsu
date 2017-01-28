using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Map
{
    public class MapManager : MonoBehaviour
    {
        [HideInInspector]
        public static MapManager Instance = null;

        Tile[][] Tiles;

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

        public Tile GetTile(int x, int y)
        {
            return Tiles[x][y];
        }
    }
}