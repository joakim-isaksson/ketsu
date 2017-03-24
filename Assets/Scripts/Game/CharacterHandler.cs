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

        [Header("Sounds")]
        public string TrainsformToKetsuSfx;
        public string SplitKetsuSfx;

        [HideInInspector]
		public Character ActiveCharacter;

        Queue<Vector3> moveQueue;

        int waitingForMoveActions;

        MapManager mapManager;

        Character fox;
        Character wolf;
        Character ketsu;
        Character beforeKetsu;

        void Awake()
		{
            moveQueue = new Queue<Vector3>();
        }

		void Start()
		{
            mapManager = FindObjectOfType<MapManager>();

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

            // Moving FOX and/or WOLF
            if (ActiveCharacter.Type == MapObjectType.Fox || ActiveCharacter.Type == MapObjectType.Wolf)
            {
                Character active = ActiveCharacter;
                Character other = ActiveCharacter.Type == MapObjectType.Fox ? wolf : fox;

                Vector3 activePos = active.transform.position + direction;
                Vector3 otherPos = other != null ? other.transform.position + VectorUtils.Mirror(direction, Vector3.zero) : Vector3.zero;

                // Blockers
                MapObject activeBlocker = active.GetBlocking(activePos);
                MapObject otherBlocker = other != null ? other.GetBlocking(otherPos) : null;

                if (other != null &&
                    ((activeBlocker != null && activeBlocker.Type == other.Type) ||
                    (activeBlocker == null && activePos.Equals(otherPos))))
                {
                    // Turn to ketsu
                    TransformToKetsu(activePos, active, other);
                    return;
                }
                else if (activeBlocker != null)
                {
                    // Active character is blocked
                    Debug.Log("Invalid Move: " + active.Type + " blocked by " + activeBlocker.Type);
                    NextMoveAction();
                    return;
                }
                else if (other != null)
                {
                    if (otherBlocker != null)
                    {
                        // Other character is blocked
                        Debug.Log("Invalid Move: " + other.Type + " blocked by " + otherBlocker.Type);
                        NextMoveAction();
                        return;
                    }
                    else
                    {
                        // Move both haracters
                        waitingForMoveActions += 2;
                        active.MoveTo(activePos, OnMoveActionCompleted);
                        other.MoveTo(otherPos, OnMoveActionCompleted);
                        return;
                    }
                }
                else
                {
                    // Move only the active character
                    waitingForMoveActions++;
                    active.MoveTo(activePos, OnMoveActionCompleted);
                    return;
                }
            }
            else // Moving KETSU
            {
                Vector3 ketsuPos = ketsu.transform.position + direction;

                // Blockers
                MapObject ketsuBlocker = ketsu.GetBlocking(ketsuPos);
                if (ketsuBlocker != null)
                {
                    // Ketsu is blocked
                    Debug.Log("Invalid Move: " + ketsu.Type + " blocked by " + ketsuBlocker.Type);
                    NextMoveAction();
                    return;
                }
                else if (ConsumeKetsuPower(KetsuMoveCost))
                {
                    // Move ketsu
                    waitingForMoveActions++;
                    ketsu.MoveTo(ketsuPos, OnMoveActionCompleted);
                    return;
                }
                else
                {
                    // Split ketsu
                    SplitKetsu(ketsuPos);
                    return;
                }
            }
		}

        void TransformToKetsu(Vector3 mergePos, Character active, Character other)
        {
            waitingForMoveActions += 2;
            other.MoveTo(mergePos, OnMoveActionCompleted);
            active.MoveTo(mergePos, delegate {

                AkSoundEngine.PostEvent(TrainsformToKetsuSfx, gameObject);

                // Update Ketsu position and rotation
                ketsu.transform.rotation = active.transform.rotation;
                ketsu.transform.position = active.transform.position;

                // Deactive and activate characters
                ketsu.gameObject.SetActive(true);
                fox.gameObject.SetActive(false);
                wolf.gameObject.SetActive(false);

                // Set ketsu as the active character
                beforeKetsu = ActiveCharacter;
                ActiveCharacter = ketsu;

                OnMoveActionCompleted();
            });
        }

        public void SplitKetsu(Vector3 targetPos)
        {
            Character active = beforeKetsu;
            Character other = active.Type == MapObjectType.Fox ? wolf : fox;

            Vector3 activePos = targetPos;
            Vector3 otherPos = VectorUtils.Mirror(activePos, ketsu.transform.position);

            // Check blockers
            MapObject activeBlocker = active.GetBlocking(activePos);
            if (activeBlocker != null)
            {
                // Active is blocked
                Debug.Log("Can Not Split: " + active.Type + " blocked by " + activeBlocker.Type);
                NextMoveAction();
                return;
            }
            MapObject otherBlocker = active.GetBlocking(otherPos);
            if (otherBlocker != null)
            {
                // Other is blocked
                Debug.Log("Can Not Split: " + other.Type + " blocked by " + otherBlocker.Type);
                NextMoveAction();
                return;
            }

            // No blockers - Split Ketsu!
            AkSoundEngine.PostEvent(SplitKetsuSfx, gameObject);

            // Deactive and activate characters
            ketsu.gameObject.SetActive(false);
            active.gameObject.SetActive(true);
            other.gameObject.SetActive(true);
            ActiveCharacter = active;

            // Animate
            waitingForMoveActions += 2;
            active.transform.position = ketsu.transform.position;
            other.transform.position = ketsu.transform.position;
            active.MoveTo(activePos, OnMoveActionCompleted);
            other.MoveTo(otherPos, OnMoveActionCompleted);
        }

        void OnMoveActionCompleted()
        {
            waitingForMoveActions--;

            // When movement actions are done -> check if map has been solved
            if (waitingForMoveActions == 0 && mapManager.CheckSolved()) return;

            // If the map is not solved yet -> move to the next action
            NextMoveAction();
        }
    }
}