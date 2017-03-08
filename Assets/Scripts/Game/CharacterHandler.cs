using Ketsu.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

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
		public float MaxKetsuPower;
		public float KetsuPower;

		[HideInInspector]
		public Character Fox;
		[HideInInspector]
		public Character Wolf;
		[HideInInspector]
		public Character Ketsu;

		[HideInInspector]
		public Character ActiveCharacter;
		[HideInInspector]
		public Character CharBeforeKetsu;

		MapManager mapManager;
		Vector2 touchStartPos;

		bool waitingForFox;
		bool waitingForWolf;
		bool waitingForKetsu;

		void Awake()
		{
			mapManager = FindObjectOfType<MapManager>();
		}

		void Start()
		{
			// Find characters to control
			foreach (Character character in FindObjectsOfType<Character>())
			{
				switch (character.Type)
				{
					case MapObjectType.Fox:
						Fox = character;
						break;
					case MapObjectType.Wolf:
						Wolf = character;
						break;
					case MapObjectType.Ketsu:
						Ketsu = character;
						break;
				}
			}

			// Set starting characters
			if (Fox == null && Wolf == null && Ketsu == null)
			{
				throw new InvalidOperationException("No characters found from scene! Try adding one...");
			}
			else if (Fox != null && Wolf != null && Ketsu != null)
			{
				throw new InvalidOperationException("Too many characters in the scene! Try removing one...");
			}
			else if (Fox != null && Wolf == null && Ketsu != null)
			{
				throw new InvalidOperationException("Fox and Ketsu in the same scene! Try removing one...");
			}
			else if (Fox == null && Wolf != null && Ketsu != null)
			{
				throw new InvalidOperationException("Wolf and Ketsu in the same scene! Try removing one...");
			}
			else if (Fox != null && Wolf != null && Ketsu == null)
			{
				// Starting with Fox and Wolf
				ActiveCharacter = Fox;
				Ketsu = Instantiate(KetsuPrefab).GetComponent<Character>();
				Ketsu.gameObject.SetActive(false);
			}
			else if (Fox != null && Wolf == null && Ketsu == null)
			{
				// Starting with Fox
				ActiveCharacter = Fox;
			}
			else if (Fox == null && Wolf != null && Ketsu == null)
			{
				// Starting with Wolf
				ActiveCharacter = Wolf;
			}
			else if (Fox == null && Wolf == null && Ketsu != null)
			{
				// Starting with Ketsu
				ActiveCharacter = Ketsu;
				Fox = Instantiate(FoxPrefab).GetComponent<Character>();
				Wolf = Instantiate(WolfPrefab).GetComponent<Character>();
				Fox.gameObject.SetActive(false);
				Wolf.gameObject.SetActive(false);
			}
			else
			{
				throw new InvalidOperationException("Unexpected error - Contact the code monkeys!");
			}
		}

		void Update()
		{
			if (mapManager.Solved) return;

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
			HandleKeyInputs();
#else
            HandleTouchInputs();
#endif
			UpdateKetsuPower();
		}

		void UpdateKetsuPower()
		{
			if (ActiveCharacter.Type == MapObjectType.Ketsu) KetsuPower -= BurnRate * Time.deltaTime;
			else KetsuPower += RegenerationRate * Time.deltaTime;
			KetsuPower = Mathf.Clamp(KetsuPower, 0.0f, MaxKetsuPower);

			KetsuPowerText.text = KetsuPower.ToString("0.00") + " / " + MaxKetsuPower.ToString("0.00");
		}

		/// <summary>
        /// Try to select character from given point
        /// </summary>
        /// <param name="point">Point to try to select the character from</param>
        /// <returns>true if character selected and changed, false otherwise</returns>
		public bool SelectCharacter(Vector3 point)
		{
            // asd asdas d asd asd asd as ds
            foreach (MapObject obj in MapManager.GetObjects(point))
            {
                if (obj.Type == MapObjectType.Fox || obj.Type == MapObjectType.Wolf || obj.Type == MapObjectType.Ketsu)
                {
                    charHandler.
                }
            }
            // asd asd asd as d

            // Character selection
            if (Fox != null && Fox.Position.Equals(selectedTilePos))
			{
				Debug.Log("Fox Selected");
				ActiveCharacter = Fox;
				return true;
			}
			else if (Wolf != null && Wolf.Position.Equals(selectedTilePos))
			{
				Debug.Log("Wolf Selected");
				ActiveCharacter = Wolf;
				return true;
			}
			else if (Ketsu != null && Ketsu.Position.Equals(selectedTilePos))
			{
				Debug.Log("Ketsu Selected");
				ActiveCharacter = Ketsu;
				return true;
			}

			return false;
		}

		public void MoveAction(Vector3 direction)
		{
			// Waiting
			if (ActiveCharacter.HasMoved == true || waitingForFox || waitingForWolf || waitingForKetsu) return;

			Debug.Log("Move Action: " + direction.ToString());

			switch (ActiveCharacter.Type)
			{
				case MapObjectType.Fox:
					waitingForFox = true;
					Fox.MoveTo(direction, delegate
					{
						waitingForFox = false;
						if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
					});
					if (Wolf != null)
					{
						waitingForWolf = true;
						Wolf.MoveTo(direction.Opposite(), delegate
						{
							waitingForWolf = false;
							if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
						});
					}
					break;
				case MapObjectType.Wolf:
					waitingForWolf = true;
					Wolf.MoveTo(direction, delegate
					{
						waitingForWolf = false;
						if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
					});
					if (Fox != null)
					{
						waitingForFox = true;
						Fox.MoveTo(direction.Opposite(), delegate
						{
							waitingForFox = false;
							if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
						});
					}
					break;
				case MapObjectType.Ketsu:
					waitingForKetsu = true;
					Ketsu.MoveTo(direction, delegate
					{
						waitingForKetsu = false;
						if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
					});
					break;
				default:
					break;
			}
		}
	}
}