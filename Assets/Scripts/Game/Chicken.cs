using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Chicken : MapObject
    {
        public float MoveSpeed;
        public Transform Pumber;

        CharController controller;
        Map map;
        Vector3 target;

        void Awake()
        {

        }

        void Start()
        {
            controller = FindObjectOfType<CharController>();

            UpdatePosition();

            map = MapManager.LoadedMap;
        }

        void Update()
        {
            // Check if we need a new target
            if (target == null) SetNewRandomTarget();

            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                transform.position = target;
                SetNewRandomTarget();
            }

            // If pumber is hitting something -> rotate to a random direction
            IntVector2 pumberPos = IntVector2.FromXZ(Pumber.position);
            if (!map.Contains(pumberPos) || Blocking(pumberPos))
            {
                SetNewRandomTarget();
            }
        }

        void SetNewRandomTarget()
        {
            int attempt = 0;
            Vector3 newTarget;
            do
            {
                attempt++;
                switch ((int)(UnityEngine.Random.value * 4))
                {
                    case 0:
                        newTarget = transform.position + Vector3.forward;
                        break;
                    case 1:
                        newTarget = transform.position + Vector3.back;
                        break;
                    case 2:
                        newTarget = transform.position + Vector3.left;
                        break;
                    case 3:
                        newTarget = transform.position + Vector3.right;
                        break;
                    default:
                        newTarget = transform.position + Vector3.zero;
                        break;
                }
            }
            while (Blocking(IntVector2.FromXZ(newTarget)) != null && attempt < 10);

            target = newTarget;
            transform.LookAt(target);
        }

        MapObject Blocking(IntVector2 point)
        {
            foreach (MapObject obj in map.GetObjects(point))
            {
                if (obj.Layer != MapLayer.Ground && obj.Type != MapObjectType.Tree) return obj;
            }

            // Nothing is blocking
            return null;
        }
    }
}