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
        public bool HasMoved { get; private set; }

        [HideInInspector]
        public static int KetsuPower;

        PlayerController pc;

        private void Start()
        {
            pc = FindObjectOfType<PlayerController>();
        }

        void LateUpdate()
        {
            HasMoved = false;
        }

        // TODO: Beatufy / Refactor - starting to look like a mess
        public void MoveTo(Direction direction, Action callback)
        {
            IntVector2 targetPos = Position.Add(direction.ToIntVector2());

            // Check boarder restrictions
            Map map = MapManager.Instance.CurrentMap;
            if (targetPos.X < 0 || targetPos.X >= map.Width || targetPos.Y < 0 || targetPos.Y >= map.Height)
            {
                if (callback != null) callback();
                return;
            }

            // Check blocking and try to move if nothing is blocking
            MapObject blocking = BlockingObject(targetPos);
            if (blocking == null)
            {
                // Moving as Ketsu uses Ketsu Power
                if (Type == MapObjectType.Ketsu)
                {
                    if (KetsuPower > 0)
                    {
                        --KetsuPower;
                        Debug.Log("Ketsu Power Left: " + KetsuPower);
                    }
                    else
                    {
                        Debug.Log("Not enough Ketsu Power - breaking down!");
                        HasMoved = true;

                        // TODO Ketsu Breakdown (get directions from there)
                        IntVector2 foxPos = new IntVector2(Position.X - 1, Position.Y);
                        IntVector2 wolfPos = new IntVector2(Position.X + 1, Position.Y);

                        TransformFromKetsu(foxPos, wolfPos, callback);

                        return;
                    }
                }

                // Update position
                HasMoved = true;
                Position = targetPos;

                AkSoundEngine.PostEvent(SfxMove, gameObject);

                AnimateTo(targetPos, callback);
            }

            // Turn to Ketsu
            else if (blocking.Type == MapObjectType.Fox || blocking.Type == MapObjectType.Wolf)
            {
                Character blockingCharacter = blocking.gameObject.GetComponent<Character>();
                if (blockingCharacter.HasMoved)
                {
                    // Update position
                    HasMoved = true;
                    Position = blockingCharacter.Position;

                    TransformToKetsu(blockingCharacter, callback);
                    return;
                }
                else
                {
                    Debug.Log("Beep Poop - Can not ketsu!");
                    if (callback != null) callback();
                    return;
                }
            }

            // Do not move (something is blocking the way)
            else
            {
                Debug.Log("Blocked by: " + blocking.Type.ToString());
                if (callback != null) callback();
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
                if (dObj.gameObject.activeSelf && target.Equals(dObj.Position))
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

        void TransformToKetsu(Character other, Action callback)
        {
            AkSoundEngine.PostEvent(SfxMerge, gameObject);

            AnimateTo(other.Position, delegate {

                // Update Ketsu position and rotation
                pc.Ketsu.transform.rotation = transform.rotation;
                pc.Ketsu.transform.position = new Vector3(
                    other.Position.X,
                    other.transform.position.y,
                    other.Position.Y
                );
                pc.Ketsu.UpdatePosition();

                // Deactive and activate characters
                pc.Ketsu.gameObject.SetActive(true);
                pc.Fox.gameObject.SetActive(false);
                pc.Wolf.gameObject.SetActive(false);

                // Set the active character as Ketsu
                pc.SelectedCharacter = pc.Ketsu;

                if (callback != null) callback();
            });
        }

        void TransformFromKetsu(IntVector2 foxPos, IntVector2 wolfPos, Action callback)
        {
           AkSoundEngine.PostEvent(SfxSplit, gameObject);

            // Deactive and activate characters
            pc.Ketsu.gameObject.SetActive(false);
            pc.Fox.gameObject.SetActive(true);
            pc.Wolf.gameObject.SetActive(true);

            // Set the active character as Fox
            pc.SelectedCharacter = pc.Fox;

            // Update fox position
            pc.Fox.transform.rotation = transform.rotation;
            pc.Fox.transform.position = new Vector3(Position.X, pc.Fox.transform.position.y, Position.Y);
            pc.Fox.Position = foxPos;
            pc.Fox.HasMoved = true;
            pc.Fox.AnimateTo(foxPos, null);

            // Update wolf position
            pc.Wolf.transform.rotation = transform.rotation;
            pc.Wolf.transform.position = new Vector3(Position.X, pc.Wolf.transform.position.y, Position.Y);
            pc.Wolf.Position = wolfPos;
            pc.Wolf.HasMoved = true;
            pc.Wolf.AnimateTo(wolfPos, callback);
        }

        public void AnimateTo(IntVector2 target, Action callback)
        {
            StartCoroutine(RunAnimateTo(target, callback));
        }

        IEnumerator RunAnimateTo(IntVector2 target, Action callback)
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

            if (callback != null) callback();
        }
    }
}