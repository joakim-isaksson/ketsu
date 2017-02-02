using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Map
{
    public class Map
    {
		public int Height;
		public int Width;

		public List<List<MapObject>> Ground;
		public List<List<MapObject>> Obstacles;
		public List<MapObject> Objects;
    }
}