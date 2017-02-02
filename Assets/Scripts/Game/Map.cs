using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Map
    {
		public int Height;
		public int Width;

		public List<List<MapObject>> Ground;
		public List<List<MapObject>> Obstacles;
		public List<MapObject> Objects;

        public Map(int width, int height)
        {
            Height = height;
            Width = width;

            Ground = new List<List<MapObject>>();
            Obstacles = new List<List<MapObject>>();
            for (int y = 0; y < Height; ++y)
            {
                List<MapObject> groundRow = new List<MapObject>();
                List<MapObject> obstaclesRow = new List<MapObject>();
                for (int x = 0; x < Width; ++x)
                {
                    groundRow.Add(null);
                    obstaclesRow.Add(null);
                }
                Ground.Add(groundRow);
                Obstacles.Add(obstaclesRow);
            }

            Objects = new List<MapObject>();
        }
    }
}