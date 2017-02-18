using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class PingPongEnemy : MapObject
	{
        public float MoveSpeed;
        public Transform Pumber;

        Map map;

        void Awake()
        {

        }

        void Start()
        {
            UpdatePosition();
            map = MapManager.LoadedMap;
        }

		void Update()
		{
            transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
            UpdatePosition();

            // If pumber is hitting something -> rotate 180 degrees
            IntVector2 pumberPos = IntVector2.FromXZ(Pumber.position);
            if (!map.Contains(pumberPos) || Blocking(pumberPos)) transform.Rotate(Vector3.up, 180.0f);
        }

        MapObject Blocking(IntVector2 point)
        {
            foreach (MapObject obj in map.GetObjects(point))
            {
                if (obj.Layer == MapLayer.Ground) continue;
                if (obj.Layer == MapLayer.Object) return obj;
            }

            // Nothing is blocking
            return null;
        }
    }
}