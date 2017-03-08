using Ketsu.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class CharacterHandler : MonoBehaviour
	{
		[Header("Character Prefabs")]
		public GameObject FoxPrefab;
		public GameObject WolfPrefab;
		public GameObject KetsuPrefab;

		[Header("Ketsu Power")]
		public float KetsuMoveCost;
        public float KetsuPower;
        public float MaxKetsuPower;

        [Header("Action Queue")]
        public int MoveQueueSize;

        [HideInInspector]
		public Character ActiveCharacter;

        Queue<Vector3> moveQueue;
		Vector2 touchStartPos;

        int waitingForMoveActions;

        MapManager mapManager;

        Character fox;
        Character wolf;
        Character ketsu;
        Character beforeKetsu;

        void Awake()
		{
            moveQueue = new Queue<Vector3>();
            mapManager = FindObjectOfType<MapManager>();
        }

		void Start()
		{
			// Find characters
			foreach (Character character in FindObjectsOfType<Character>())
			{
				switch (character.Type)
				{
					case MapObjectType.Fox:
						fox = character;
						break;
					case MapObjectType.Wolf:
						wolf = character;
						break;
					case MapObjectType.Ketsu:
						ketsu = character;
						break;
				}
			}

			// Instantiate starting characters
			if (fox == null && wolf == null && ketsu == null)
			{
				throw new InvalidOperationException("No characters found from scene! Try adding one...");
			}
			else if (fox != null && wolf != null && ketsu != null)
			{
				throw new InvalidOperationException("Too many characters in the scene! Try removing one...");
			}
			else if (fox != null && wolf == null && ketsu != null)
			{
				throw new InvalidOperationException("Fox and Ketsu in the same scene! Try removing one...");
			}
			else if (fox == null && wolf != null && ketsu != null)
			{
				throw new InvalidOperationException("Wolf and Ketsu in the same scene! Try removing one...");
			}
			else if (fox != null && wolf != null && ketsu == null)
			{
				// Starting with Fox and Wolf
				ActiveCharacter = fox;
				ketsu = Instantiate(KetsuPrefab).GetComponent<Character>();
				ketsu.gameObject.SetActive(false);
			}
			else if (fox != null && wolf == null && ketsu == null)
			{
				// Starting with Fox
				ActiveCharacter = fox;
			}
			else if (fox == null && wolf != null && ketsu == null)
			{
				// Starting with Wolf
				ActiveCharacter = wolf;
			}
			else if (fox == null && wolf == null && ketsu != null)
			{
				// Starting with Ketsu
				ActiveCharacter = ketsu;
				fox = Instantiate(FoxPrefab).GetComponent<Character>();
				wolf = Instantiate(WolfPrefab).GetComponent<Character>();
				fox.gameObject.SetActive(false);
				wolf.gameObject.SetActive(false);
			}
			else
			{
				throw new InvalidOperationException("Unexpected error - Contact the code monkeys!");
			}
		}

		void Update()
		{

		}

        public void AddKetsuPower(float amount)
        {
            KetsuPower = Mathf.Min(KetsuPower + amount, MaxKetsuPower);
        }

        public bool ConsumeKetsuPower(float amount)
        {
            if (amount <= KetsuPower)
            {
                KetsuPower -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
        /// Try to select character from given point
        /// </summary>
        /// <param name="point">Point to try to select the character from</param>
        /// <returns>true if character selected and changed, false otherwise</returns>
		public bool SelectCharacter(Vector3 point)
		{
            foreach (MapObject obj in MapManager.GetObjects(point))
            {
                if (obj.Type != ActiveCharacter.Type &&
                    (obj.Type == MapObjectType.Fox ||
                    obj.Type == MapObjectType.Wolf ||
                    obj.Type == MapObjectType.Ketsu))
                {
                    Debug.Log("Activated: " + obj.Type);
                    ActiveCharacter = obj.GetComponent<Character>();
                    return true;
                }
            }

			return false;
		}

        /// <summary>
        /// Try to move active character to given direction
        /// </summary>
        /// <param name="direction">Direction to move</param>
        public void MoveAction(Vector3 direction)
        {
            // If queue is not full -> add to queue and call to execute the next move action
            if (moveQueue.Count < MoveQueueSize)
            {
                moveQueue.Enqueue(direction);
                NextMoveAction();
            }
        }

        void NextMoveAction()
		{
            // Check if waiting for other actions to complete
            if (waitingForMoveActions > 0) return;

            // Check if queue is empty
            if (moveQueue.Count == 0) return;

            // Get direction for next move action
            Vector3 direction = moveQueue.Dequeue();

            switch (ActiveCharacter.Type)
			{
				case MapObjectType.Fox:
                case MapObjectType.Wolf:
                    Vector3 foxPos = fox.transform.position + direction;
                    Vector3 wolfPos = wolf.transform.position + VectorUtils.Mirror(direction, Vector3.zero);
                    if (fox.IsBlocked(foxPos) && wolf.IsBlocked(wolfPos))
                    {
                        waitingForMoveActions += 2;
                        fox.MoveTo(direction, OnMoveActionCompleted);
                        wolf.MoveTo(wolfPos, OnMoveActionCompleted);
                    }

                    // ---------------------------
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
                    // ---------------------------

                    break;
                case MapObjectType.Ketsu:
                    Vector3 ketsuPos = ketsu.transform.position + direction;
                    if (ketsu.IsBlocked(ketsuPos))
                    {
                        if (ConsumeKetsuPower(KetsuMoveCost))
                        {
                            waitingForMoveActions++;
                            ketsu.MoveTo(direction, OnMoveActionCompleted);
                        }
                        else
                        {
                            // Explode
                            ketsu.SplitKetsu();
                        }
                    }
                    break;
                default:
                    Debug.Log("Unkown character type: " + ActiveCharacter.Type);
					break;
			}
		}

        void OnMoveActionCompleted()
        {
            waitingForMoveActions--;
            if (waitingForMoveActions == 0) mapManager.CheckSolved();
            NextMoveAction();
        }

        void TransformToKetsu(Character other, Action callback)
        {
            AkSoundEngine.PostEvent(SfxMerge, gameObject);

            AnimateTo(other.Position, delegate
            {

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
                controller.CharBeforeKetsu = controller.ActiveCharacter;
                controller.ActiveCharacter = controller.Ketsu;

                if (callback != null) callback();
            });
        }

        // targetPos is the position where the previously controlled character is moving in the split
        public void SplitKetsu(IntVector2 targetPos, Action callback)
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

            // Check if characters will stay inside the map
            if (!map.Contains(foxPos))
            {
                Debug.Log("Can not split - Fox outside of boarders");
                if (callback != null) callback();
                return;
            }
            if (!map.Contains(wolfPos))
            {
                Debug.Log("Can not split - Wolf outside of boarders");
                if (callback != null) callback();
                return;
            }

            // Check if splitting is blocked by something
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
            controller.ActiveCharacter = controller.CharBeforeKetsu;

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
    }
}