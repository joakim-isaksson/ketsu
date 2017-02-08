using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Map
    {
		public int Height;
		public int Width;

        // MapObject[width][height]
		public MapObject[][] GroundLayer;
		public MapObject[][] ObjectLayer;
		public List<MapObject> DynamicLayer;

        /// <summary>
        /// Initialize and empty map with specific width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Map(int width, int height)
        {
            Width = width;
            Height = height;

            GroundLayer = new MapObject[Width][];
            ObjectLayer = new MapObject[Width][];
            for (int x = 0; x < Width; ++x)
            {
                GroundLayer[x] = new MapObject[Height];
                ObjectLayer[x] = new MapObject[Height];
            }

            DynamicLayer = new List<MapObject>();
        }
    }
}