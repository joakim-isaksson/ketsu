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
        public float Damage;
        public Transform Pumber;

        Map map;
        bool dased;

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
            if (dased) return;

            transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
            UpdatePosition();

            // If pumber is hitting something -> rotate 180 degrees
            IntVector2 pumberPos = IntVector2.FromXZ(Pumber.position);
            if (!map.Contains(pumberPos) || Blocking(pumberPos)) transform.Rotate(Vector3.up, 180.0f);
        }

        void OnTriggerEnter(Collider other)
        {
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(Damage);
                dased = true;
                DelayedAction(1.0f, delegate { dased = false; });
            }
            
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