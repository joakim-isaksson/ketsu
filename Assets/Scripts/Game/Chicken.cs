using Ketsu.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Chicken : MapObject
    {
        public float MoveSpeed;
        public Transform Pumber;

        Map map;
        Vector3 target;

        void Awake()
        {

        }

        void Start()
        {
            UpdatePositionFromWorld();

            map = MapManager.LoadedMap;

            SetNewTarget();
        }

        void Update()
        {
            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                transform.position = target;
                SetNewTarget();
            }

            // If pumber is hitting something -> get new target
            IntVector2 pumberPos = IntVector2.FromXZ(Pumber.position);
            if (!map.Contains(pumberPos) || Blocking(pumberPos))
            {
                SetNewTarget();
            }
        }

        void SetNewTarget()
        {
            List<Vector3> directions = new List<Vector3>();
            directions.Add(Vector3.forward);
            directions.Add(Vector3.back);
            directions.Add(Vector3.left);
            directions.Add(Vector3.right);
            directions.Add(Vector3.zero);

            while (directions.Count > 0)
            {
                Vector3 direction = directions[Random.Range(0, directions.Count)];
                directions.Remove(direction);

                IntVector2 targetPos = IntVector2.FromXZ(transform.position + direction);
                if (map.Contains(targetPos) && Blocking(targetPos) == null)
                {
                    target = new Vector3(
                        targetPos.X,
                        transform.position.y,
                        targetPos.Y
                    );
                    transform.LookAt(target);
                    break;
                }
            }
        }

        MapObject Blocking(IntVector2 point)
        {
            foreach (MapObject obj in map.GetObjects(point))
            {
                if (obj != this) continue;
                if (obj.Type == MapObjectType.Water) return obj;
                if (obj.Type == MapObjectType.Tree) continue;
                if (obj.Layer != MapLayer.Ground) return obj;
            }

            // Nothing is blocking
            return null;
        }
    }
}