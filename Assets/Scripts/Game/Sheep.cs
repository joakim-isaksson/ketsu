using Ketsu.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Sheep : MapObject
    {
        public float MoveSpeed;
        public Transform Pumber;

        Map map;

        void Awake()
        {

        }

        void Start()
        {
            UpdatePositionFromWorld();

            map = MapManager.LoadedMap;
        }

        void Update()
        {
            // Move forward
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);

            // If pumber is hitting something -> turn
            IntVector2 pumberPos = IntVector2.FromXZ(Pumber.position);
            if (!map.Contains(pumberPos) || Blocked(pumberPos) != null)
            {
                transform.Rotate(Vector3.up, 90.0f);
            }
        }

        MapObject Blocked(IntVector2 point)
        {
            foreach (MapObject obj in map.GetObjects(point))
            {
                if (obj != this && obj.Layer != MapLayer.Ground) return obj;
            }

            // Nothing is blocking
            return null;
        }
    }
}