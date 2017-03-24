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

            // Moving FOX / WOLF
            if (ActiveCharacter.Type == MapObjectType.Fox || ActiveCharacter.Type == MapObjectType.Wolf)
            {
                Character active = ActiveCharacter;
                Character other = ActiveCharacter.Type == MapObjectType.Fox ? wolf : fox;

                HandleMoveAction(
                    direction,
                    active,
                    active.transform.position,
                    other,
                    other != null ? other.transform.position : Vector3.zero,
                    false
                );
            }

            // Moving KETSU
            else
            {
                HandleMoveAction(
                    direction,
                    ketsu,
                    ketsu.transform.position,
                    null,
                    Vector3.zero,
                    true
                );
            }
        }

        void OnMoveActionCompleted()
        {
            waitingForMoveActions--;

            // When movement actions are done -> check if map has been solved
            if (waitingForMoveActions == 0 && mapManager.CheckSolved()) return;

            // If the map is not solved yet -> move to the next action
            NextMoveAction();
        }

        void HandleMoveAction(Vector3 direction, Character active, Vector3 activeOrigin, Character other, Vector3 otherOrigin, bool isKetsu)
        {
            // Get target information
            TargetInfo activeTarget = new TargetInfo(active, activeOrigin + direction);
            TargetInfo otherTarget = null;
            if (other != null)
            {
                otherTarget = new TargetInfo(other, otherOrigin + VectorUtils.Mirror(direction, Vector3.zero));
            }

            // Check turning to Ketsu
            if (other != null &&
                ((activeTarget.Blocker != null && activeTarget.Blocker.Type == other.Type) ||
                (activeTarget.Blocker == null && activeTarget.Position.Equals(otherTarget.Position))))
            {
                TransformToKetsu(activeTarget.Position, active, other);
                return;
            }

            // Check active blocker
            if (activeTarget.Blocker != null)
            {
                Debug.Log(active.Type + " blocked by " + activeTarget.Blocker.Type);

                if (active.transform.position != activeOrigin)
                {
                    waitingForMoveActions++;
                    active.MoveTo(activeOrigin, OnMoveActionCompleted);
                    return;
                }

                NextMoveAction();
                return;
            }

            // Check if ketsu is going to split
            if (isKetsu && !ConsumeKetsuPower(KetsuMoveCost))
            {
                SplitKetsu(activeTarget.Position);
                return;
            }

            if (other == null)
            {
                if (activeTarget.Ground.Type == MapObjectType.Ice)
                {
                    HandleMoveAction(direction, active, activeTarget.Position, null, Vector3.zero, isKetsu);
                    return;
                }

                // Move the active character
                waitingForMoveActions++;
                active.MoveTo(activeTarget.Position, OnMoveActionCompleted);
                return;
            }

            // Check other blocker
            if (otherTarget.Blocker != null)
            {
                Debug.Log("Invalid Move: " + other.Type + " blocked by " + otherTarget.Blocker.Type);
                NextMoveAction();
                return;
            }

            // Move both characters
            waitingForMoveActions += 2;
            active.MoveTo(activeTarget.Position, OnMoveActionCompleted);
            other.MoveTo(otherTarget.Position, OnMoveActionCompleted);
            return;
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
            
            // Get blockers and ground from the target position
            TargetInfo activeTarget = new TargetInfo(active, targetPos);
            if (activeTarget.Blocker != null)
            {
                // Active is blocked
                Debug.Log("Can Not Split: " + active.Type + " blocked by " + activeTarget.Blocker.Type);
                NextMoveAction();
                return;
            }
            TargetInfo otherTarget = new TargetInfo(other, VectorUtils.Mirror(activeTarget.Position, ketsu.transform.position));
            if (otherTarget.Blocker != null)
            {
                // Other is blocked
                Debug.Log("Can Not Split: " + other.Type + " blocked by " + otherTarget.Blocker.Type);
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
            active.MoveTo(activeTarget.Position, OnMoveActionCompleted);
            other.MoveTo(otherTarget.Position, OnMoveActionCompleted);
        }


        class TargetInfo
        {
            public Vector3 Position;
            public MapObject Ground;
            public MapObject Blocker;

            public TargetInfo(Character character, Vector3 position)
            {
                Position = position;

                foreach (MapObject obj in MapManager.GetObjects(position))
                {
                    if (Blocker == null && character.IsBlockedBy(obj)) Blocker = obj;
                    if (Ground == null && obj.Layer == MapObjectLayer.Ground) Ground = obj;
                    if (Blocker != null && Ground) break;
                }
            }
        }
        
    }
}