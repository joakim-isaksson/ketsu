using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Character : MapObject
    {
        public float MovementTime;

		public CharacterType Type;

        public void MoveTo(Direction direction, Action callback)
        {
            IntVector2 newPos = Position.Add(direction.ToIntVector2());

            if (!CanMoveTo(newPos))
            {
                callback();
                return;
            }

            Position = newPos;

            StartCoroutine(AnimateTo(newPos, callback));
        }

		bool CanMoveTo(IntVector2 target)
		{
            Map map = MapManager.Instance.CurrentMap;

            // Boarders
            if (target.X < 0 || target.X >= map.Width || target.Y < 0 || target.Y >= map.Height) return false;

            // Other objects
            foreach (MapObject obj in map.Objects)
            {
                if (target.Equals(obj.Position)) return false;
            }

            // Obstacles
            MapObject obstacle = map.Obstacles[target.Y][target.X];
            if (obstacle != null && obstacle.Blocking) return false;

            return true;
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