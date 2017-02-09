using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Character : MapObject
    {
        [Header("Animations")]
        public float MoveAnimTime;
        public float MoveAnimDelay;

        [Header("Sounds")]
        public string SfxMove;
        public string SfxMerge;
        public string SfxSplit;

        [HideInInspector]
        public bool HasMoved;

        public void MoveTo(Direction direction, Action callback)
        {
            IntVector2 targetPos = Position.Add(direction.ToIntVector2());

            // Check boarder restrictions
            Map map = MapManager.Instance.CurrentMap;
            if (targetPos.X < 0 || targetPos.X >= map.Width || targetPos.Y < 0 || targetPos.Y >= map.Height)
            {
                callback();
                return;
            }

            // Check blocking and move if nothing is blocking
            MapObject blocking = BlockingObject(targetPos);
            if (blocking == null)
            {
                AkSoundEngine.PostEvent(SfxMove, gameObject);

                Position = targetPos;
                StartCoroutine(AnimateTo(targetPos, callback));
            }

            // Turn to Ketsu
            else if (blocking.Type == MapObjectType.Fox || blocking.Type == MapObjectType.Wolf)
            {
                Character character = blocking.GetComponent<Character>();
                if (character.HasMoved)
                {
                    // TODO: Use other method to find the player controller
                    FindObjectOfType<PlayerController>().TurnToKetsu(character);
                    AkSoundEngine.PostEvent(SfxMerge, gameObject);
                }
                else Debug.Log("Can not ketsu!");

                callback();
                return;
            }

            // Do not move (something is blocking the way)
            else
            {
                Debug.Log("Blocked by: " + blocking.Type.ToString());
                callback();
                return;
            }
        }

        // Returns blocking object or null if nothing is blocking in the target area
		MapObject BlockingObject(IntVector2 target)
		{
            Map map = MapManager.Instance.CurrentMap;

            // Blocked by Object Level
            MapObject oObj = map.ObjectLayer[target.X][target.Y];
            if (oObj != null) return oObj;

            // Blocked by Ground Level
            MapObject gObj = map.GroundLayer[target.X][target.Y];
            if (gObj != null)
            {
                switch (gObj.Type)
                {
                    case MapObjectType.Water:
                        return gObj;
                }
            }

            // Blocked by Dynamic Level
            foreach (MapObject dObj in map.DynamicLayer)
            {
                if (target.Equals(dObj.Position))
                {
                    switch (dObj.Type)
                    {
                        case MapObjectType.Wolf:
                        case MapObjectType.Fox:
                            return dObj;
                    }
                }
            }

            // Nothing stops the moving
            return null;
		}

        IEnumerator AnimateTo(IntVector2 target, Action callback)
        {
			Vector3 start = transform.position;
            Vector3 end = new Vector3(target.X, 0, target.Y);

            yield return new WaitForSeconds(MoveAnimDelay);

            transform.LookAt(end);

            float timePassed = 0.0f;
            do
            {
                timePassed += Time.deltaTime;
                float progress = Mathf.Min(timePassed / MoveAnimTime, 1.0f);
                transform.position = Vector3.Lerp(start, end, progress);

                yield return null;

            } while (timePassed < MoveAnimTime);

            callback();
        }
    }
}