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
		public MapObject[][] Ground;
		public MapObject[][] Obstacles;
		public List<MapObject> Objects;

        /// <summary>
        /// Initialize and empty map with specific width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Map(int width, int height)
        {
            Height = height;
            Width = width;

            Ground = new MapObject[Width][];
            Obstacles = new MapObject[Width][];
            for (int x = 0; x < Width; ++x)
            {
                Ground[x] = new MapObject[Height];
                Obstacles[x] = new MapObject[Height];
            }

            Objects = new List<MapObject>();
        }
    }
}