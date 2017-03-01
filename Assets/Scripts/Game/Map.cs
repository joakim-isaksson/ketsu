using Ketsu.Utils;
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

        public List<MapObject> GetObjects(IntVector2 point)
        {
            List<MapObject> list = new List<MapObject>();

            if (!Contains(point)) return list;

            MapObject obj = GroundLayer[point.X][point.Y];
            if (obj != null && obj.gameObject.activeSelf) list.Add(obj);

            obj = ObjectLayer[point.X][point.Y];
            if (obj != null && obj.gameObject.activeSelf) list.Add(obj);

            foreach (MapObject o in DynamicLayer)
            {
                if (o.Position.Equals(point) && o.gameObject.activeSelf) list.Add(o);
            }

            return list;
        }

        public bool Contains(IntVector2 point)
        {
            if (point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height) return false;
            else return true;
        }
    }
}