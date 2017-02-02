using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Character : MonoBehaviour
    {
        public float MovementTime;

		public CharacterType Type;

        [HideInInspector]
        public IntVector2 Position;

        void Awake()
        {

        }

        void Start()
        {
            Position = new IntVector2(
                (int)Mathf.Round(transform.position.x),
                (int)Mathf.Round(transform.position.z)
            );
        }

        void Update()
        {

        }

        public void MoveTo(Direction direction, Action callback)
        {
            IntVector2 newPos = Position.Add(direction.ToIntVector2());

            if (!CanMoveTo(newPos)) return;
            Position = newPos;

            StartCoroutine(AnimateTo(newPos, callback));
        }

		bool CanMoveTo(IntVector2 target)
		{
            Map map = MapManager.Instance.CurrentMap;

            if (target.X < 0 || target.X >= map.Width || target.Y < 0 || target.Y >= map.Height) return false;

            MapObject obstacle = map.Obstacles[target.Y][target.X];
            if (obstacle == null || !obstacle.Blocking) return true;
            else return false;
		}

        IEnumerator AnimateTo(IntVector2 target, Action callback)
        {
			Vector3 start = transform.position;
            Vector3 end = new Vector3(target.X, 0, target.Y);
            
            float timePassed = 0.0f;
            do
            {
                yield return null;
                timePassed += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, Mathf.Min(timePassed / MovementTime, 1.0f));
            } while (timePassed < MovementTime);

            callback();
        }
    }
}