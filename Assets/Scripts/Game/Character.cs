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

        [Header("Sounds")]
        public string SfxMove;
        public string SfxMerge;
        public string SfxSplit;

        [HideInInspector]
        public bool HasMoved { get; private set; }

        [HideInInspector]
        public static int KetsuPower;

        Map map;
        CharacterController controller;

        void Start()
        {
            controller = FindObjectOfType<CharacterController>();
            map = MapManager.LoadedMap;
        }

        private void Awake()
        {

        }

        void LateUpdate()
        {
            HasMoved = false;
        }

        public void MoveTo(Direction direction, Action callback)
        {
            IntVector2 targetPos = Position.Add(direction.ToIntVector2());

            // Check boarder restrictions
            if (targetPos.X < 0 || targetPos.X >= map.Width || targetPos.Y < 0 || targetPos.Y >= map.Height)
            {
                if (callback != null) callback();
                return;
            }

            // Check if something is blocking the way
            MapObject blocking = BlockingObject(targetPos);

            // Nothing is blocking -> try to move there
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
                        // KETSU IS BREAKING DOWN;
                        Debug.Log("Out of Ketsu Power!");

                        SplitKetsu(targetPos, callback);

                        return;
                    }
                }

                AkSoundEngine.PostEvent(SfxMove, gameObject);

                // Update position
                HasMoved = true;
                Position = targetPos;

                AnimateTo(targetPos, callback);

                return;
            }

            // Try to turn into Ketsu
            if (blocking.Type == MapObjectType.Fox || blocking.Type == MapObjectType.Wolf)
            {
                Character blockingCharacter = blocking.gameObject.GetComponent<Character>();
                if (blockingCharacter.HasMoved)
                {
                    // Update position
                    HasMoved = true;
                    Position = blockingCharacter.Position;

                    TransformToKetsu(blockingCharacter, callback);
                }
                else
                {
                    Debug.Log("Beep Poop - Can not ketsu!");
                    if (callback != null) callback();
                }

                return;
            }

            // Something is blocking -> Do not move
            Debug.Log("Blocked by: " + blocking.Type.ToString());
            if (callback != null) callback();

            return;
        }

        // Returns blocking object or null if nothing is blocking in the target area
		MapObject BlockingObject(IntVector2 target)
		{
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
                controller.Ketsu.transform.rotation = transform.rotation;
                controller.Ketsu.transform.position = new Vector3(
                    other.Position.X,
                    other.transform.position.y,
                    other.Position.Y
                );
                controller.Ketsu.UpdatePosition();

                // Deactive and activate characters
                controller.Ketsu.gameObject.SetActive(true);
                controller.Fox.gameObject.SetActive(false);
                controller.Wolf.gameObject.SetActive(false);

                // Set the active character as Ketsu
                controller.CharBeforeKetsu = controller.SelectedCharacter;
                controller.SelectedCharacter = controller.Ketsu;

                if (callback != null) callback();
            });
        }

        void SplitKetsu(IntVector2 targetPos, Action callback)
        {
            // Where to split
            IntVector2 foxPos;
            IntVector2 wolfPos;
            if (controller.CharBeforeKetsu.Type == MapObjectType.Fox)
            {
                foxPos = targetPos;
                wolfPos = targetPos.Mirror(Position);
            }
            else
            {
                foxPos = targetPos.Mirror(Position);
                wolfPos = targetPos;
            }

            // Check if splitting is blocked
            MapObject block = BlockingObject(foxPos);
            if (block != null)
            {
                Debug.Log("Can not split - Fox is blocked by: " + block.Type);
                if (callback != null) callback();
                return;
            }
            block = BlockingObject(wolfPos);
            if (block != null)
            {
                Debug.Log("Can not split - Wolf is blocked by: " + block.Type);
                if (callback != null) callback();
                return;
            }

            // Splitting can happen - set control character
            controller.SelectedCharacter = controller.CharBeforeKetsu;

            AkSoundEngine.PostEvent(SfxSplit, gameObject);

            // Deactive and activate characters
            controller.Ketsu.gameObject.SetActive(false);
            controller.Fox.gameObject.SetActive(true);
            controller.Wolf.gameObject.SetActive(true);

            // Update fox position and animate
            controller.Fox.transform.rotation = transform.rotation;
            controller.Fox.transform.position = new Vector3(Position.X, controller.Fox.transform.position.y, Position.Y);
            controller.Fox.Position = foxPos;
            controller.Fox.HasMoved = true;
            controller.Fox.AnimateTo(foxPos, null);

            // Update wolf position and animate
            controller.Wolf.transform.rotation = transform.rotation;
            controller.Wolf.transform.position = new Vector3(Position.X, controller.Wolf.transform.position.y, Position.Y);
            controller.Wolf.Position = wolfPos;
            controller.Wolf.HasMoved = true;
            controller.Wolf.AnimateTo(wolfPos, callback);
        }

        public void AnimateTo(IntVector2 target, Action callback)
        {
            StartCoroutine(RunAnimateTo(target, callback));
        }

        IEnumerator RunAnimateTo(IntVector2 target, Action callback)
        {
			Vector3 start = transform.position;
            Vector3 end = new Vector3(target.X, 0, target.Y);

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