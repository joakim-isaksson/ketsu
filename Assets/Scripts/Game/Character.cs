using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Character : MapObject
    {
        public CharType CharType;

        [Header("Animations")]
        public float MoveAnimTime;

        [Header("Sounds")]
        public string SfxMove;
        public string SfxMerge;
        public string SfxSplit;

        [HideInInspector]
        public bool HasMoved { get; private set; }

        [HideInInspector]
        public bool AtHome { get; private set; }

        Map map;
        CharController controller;

        void Start()
        {
            controller = FindObjectOfType<CharController>();
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
            if (!map.Contains(targetPos))
            {
                if (callback != null) callback();
                return;
            }

            // Check if something is blocking the way
            MapObject blocking = Blocking(targetPos);

            // Nothing is blocking -> try to move there
            if (blocking == null)
            {
                // Moving as Ketsu uses Ketsu Power
                if (Type == MapObjectType.Ketsu)
                {
                    if (controller.KetsuPower >= controller.KetsuStepCost)
                    {
                        controller.KetsuPower -= controller.KetsuStepCost;
                        Debug.Log("Ketsu Power Left: " + controller.KetsuPower);
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

        // Returns blocking object or null if nothing is blocking the point
		MapObject Blocking(IntVector2 point)
		{
            foreach (MapObject obj in map.GetObjects(point))
            {
                if (obj.Type == MapObjectType.Water) return obj;
                else if (obj.Layer == MapLayer.Ground) continue;
                else if (obj.Layer == MapLayer.Object) {
                    if (obj.Type == MapObjectType.PowerCrystal || obj.Type == MapObjectType.MaxPowerCrystal) continue;
                    else if (Type == MapObjectType.Fox && obj.Type == MapObjectType.FoxHome) continue;
                    else if (Type == MapObjectType.Wolf && obj.Type == MapObjectType.WolfHome) continue;
                    else return obj;
                }
                else if (obj.Type == MapObjectType.Fox || obj.Type == MapObjectType.Wolf) return obj;
            }

            // Nothing is blocking
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
                controller.Ketsu.UpdatePositionFromWorld();

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

        // targetPos is the position where the previously controlled character is moving in the split
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
            MapObject block = Blocking(foxPos);
            if (block != null)
            {
                Debug.Log("Can not split - Fox is blocked by: " + block.Type);
                if (callback != null) callback();
                return;
            }
            block = Blocking(wolfPos);
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

        public void TakeDamage(float amount)
        {
            controller.KetsuPower -= amount;
            StartCoroutine(FlashColor(Color.white, 0.05f, 3));
        }

        private void OnTriggerEnter(Collider other)
        {

        }

        void AnimateTo(IntVector2 target, Action callback)
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

        IEnumerator FlashColor(Color flashColor, float time, int repeat)
        {
            MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
            Material[] materials = meshRenderer.materials;
            Color[] startingColors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                startingColors[i] = materials[i].color;
            }

            for (int i = 0; i < repeat; i++)
            {
                float timePassed = 0.0f;
                while (timePassed < time)
                {
                    timePassed += Time.deltaTime;
                    float progress = Mathf.Min(timePassed / time, 1.0f);
                    for (int ii = 0; ii < materials.Length; ii++)
                    {
                        materials[ii].color = Color.Lerp(startingColors[ii], flashColor, progress);
                    }
                    yield return null;
                }

                timePassed = 0.0f;
                while (timePassed < time)
                {
                    timePassed += Time.deltaTime;
                    float progress = Mathf.Min(timePassed / time, 1.0f);
                    for (int ii = 0; ii < materials.Length; ii++)
                    {
                        materials[ii].color = Color.Lerp(flashColor, startingColors[ii], progress);
                    }
                    yield return null;
                }
            }
        }


    }
}