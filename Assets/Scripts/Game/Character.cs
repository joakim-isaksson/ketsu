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

        public void MoveTo(Direction direction, Action callback)
        {
            IntVector2 newPos = Position.Add(direction.ToIntVector2());
            
            if (CanMoveTo(newPos))
            {
                AkSoundEngine.PostEvent("Move_" + Type.ToString(), gameObject);

                Position = newPos;
                StartCoroutine(AnimateTo(newPos, callback));
            }
            else
            {
                callback();
                return;
            }
        }

		bool CanMoveTo(IntVector2 target)
		{
            Map map = MapManager.Instance.CurrentMap;

            // Restricted by Boarders
            if (target.X < 0 || target.X >= map.Width || target.Y < 0 || target.Y >= map.Height) return false;

            // Blocked by Static Objects
            MapObject sObj = map.ObjectLayer[target.X][target.Y];
            if (sObj != null) return false;

            // Blocked by Dynamic Objects
            foreach (MapObject dObj in map.DynamicLayer)
            {
                if (target.Equals(dObj.Position))
                {
                    switch (dObj.Type)
                    {
                        case MapObjectType.Wolf:
                        case MapObjectType.Fox:
                            return false;
                    }
                }
            }

            // Nothing stops the moving
            return true;
		}

        IEnumerator AnimateTo(IntVector2 target, Action callback)
        {
			Vector3 start = transform.position;
            Vector3 end = new Vector3(target.X, 0, target.Y);

            transform.LookAt(end);

            float timePassed = 0.0f;
            do
            {
                yield return null;

                timePassed += Time.deltaTime;
                float progress = Mathf.Min(timePassed / MovementTime, 1.0f);
                transform.position = Vector3.Lerp(start, end, progress);

            } while (timePassed < MovementTime);

            callback();
        }
    }
}