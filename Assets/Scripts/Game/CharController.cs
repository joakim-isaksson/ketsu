using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.Game
{
    public class CharController : MonoBehaviour
    {
        [Tooltip("Percentage of the screens height")]
        public float DragDistance;

        [Header("Character Prefabs")]
        public GameObject FoxPrefab;
        public GameObject WolfPrefab;
        public GameObject KetsuPrefab;

        [Header("Ketsu Power")]
        public float BurnRate;
        public float RegenerationRate;
        public float KetsuStepCost;
        public float MaxKetsuPower;
        public Text KetsuPowerText;

        [HideInInspector]
        public float KetsuPower;

        //[HideInInspector]
        public Character Fox;
        //[HideInInspector]
        public Character Wolf;
        //[HideInInspector]
        public Character Ketsu;

        //[HideInInspector]
        public Character SelectedCharacter;
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
            mapManager.Awake();
            // Find characters to control
            foreach (MapObject obj in MapManager.LoadedMap.DynamicLayer)
            {
                switch (obj.GetComponent<MapObject>().Type)
                {
                    case MapObjectType.Fox:
                        Fox = obj.GetComponent<Character>();
                        break;
                    case MapObjectType.Wolf:
                        Wolf = obj.GetComponent<Character>();
                        break;
                    case MapObjectType.Ketsu:
                        Ketsu = obj.GetComponent<Character>();
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
                SelectedCharacter = Fox;
                Ketsu = Instantiate(KetsuPrefab).GetComponent<Character>();
                Ketsu.gameObject.SetActive(false);
            }
            else if (Fox != null && Wolf == null && Ketsu == null)
            {
                // Starting with Fox
                SelectedCharacter = Fox;
            }
            else if (Fox == null && Wolf != null && Ketsu == null)
            {
                // Starting with Wolf
                SelectedCharacter = Wolf;
            }
            else if (Fox == null && Wolf == null && Ketsu != null)
            {
                // Starting with Ketsu
                SelectedCharacter = Ketsu;
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
            if (SelectedCharacter.Type == MapObjectType.Ketsu) KetsuPower -= BurnRate * Time.deltaTime;
            else KetsuPower += RegenerationRate * Time.deltaTime;
            KetsuPower = Mathf.Clamp(KetsuPower, 0.0f, MaxKetsuPower);

            KetsuPowerText.text = KetsuPower.ToString("0.00") + " / " + MaxKetsuPower.ToString("0.00");
        }

        void HandleKeyInputs()
        {
            if (Input.GetButtonDown("Left")) MoveAction(Direction.Left);
            else if (Input.GetButtonDown("Right")) MoveAction(Direction.Right);
            else if (Input.GetButtonDown("Forward")) MoveAction(Direction.Forward);
            else if (Input.GetButtonDown("Back")) MoveAction(Direction.Back);
            else if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
                {
                    TapAction(hit.point);
                }
            }
        }

        void HandleTouchInputs()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                // Touch Started
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = touch.position;
                }

                // Touch Ended
                else if (touch.phase == TouchPhase.Ended)
                {
                    // It's a SWIPE
                    if (Vector3.Distance(touchStartPos, touch.position) > Screen.height * DragDistance)
                    {
                        if (Mathf.Abs(touchStartPos.x - touch.position.x) > Mathf.Abs(touchStartPos.y - touch.position.y))
                        { 
                            if ((touchStartPos.x < touch.position.x)) MoveAction(Direction.Right);
                            else MoveAction(Direction.Left);
                        }
                        else
                        { 
                            if (touchStartPos.y < touch.position.y) MoveAction(Direction.Forward);
                            else MoveAction(Direction.Back);
                        }
                    }

                    // It's a TAP
                    else
                    {
                        TapAction(Camera.main.ScreenToWorldPoint(touch.position));
                    }
                }
            }
        }

        void TapAction(Vector3 tapPoint)
        {
            IntVector2 selectedTilePos = new IntVector2(
                (int)Mathf.Round(tapPoint.x),
                (int)Mathf.Round(tapPoint.z)
            );

            Debug.Log("Tap Point: " + tapPoint + ", Selected Tile: " + selectedTilePos + ", Target Char Point: " + SelectedCharacter.transform.position);

            // Character selection
            if (!CharacterSelectionAction(selectedTilePos))
            {
                // Move action
                if (Mathf.Abs(SelectedCharacter.Position.X - selectedTilePos.X) >
                    Mathf.Abs(SelectedCharacter.Position.Y - selectedTilePos.Y))
                {
                    if (SelectedCharacter.Position.X < selectedTilePos.X) MoveAction(Direction.Right);
                    else MoveAction(Direction.Left);
                }
                else
                {
                    if (SelectedCharacter.Position.Y < selectedTilePos.Y) MoveAction(Direction.Forward);
                    else MoveAction(Direction.Back);
                }
            }
        }

        // Return true if character selected
        bool CharacterSelectionAction(IntVector2 selectedTilePos)
        {
            // Character selection
            if (Fox != null && Fox.Position.Equals(selectedTilePos))
            {
                Debug.Log("Fox Selected");
                SelectedCharacter = Fox;
                return true;
            }
            else if (Wolf != null && Wolf.Position.Equals(selectedTilePos))
            {
                Debug.Log("Wolf Selected");
                SelectedCharacter = Wolf;
                return true;
            }
            else if (Ketsu != null && Ketsu.Position.Equals(selectedTilePos))
            {
                Debug.Log("Ketsu Selected");
                SelectedCharacter = Ketsu;
                return true;
            }

            return false;
        }

        void MoveAction(Direction direction)
        {
            // Waiting
            if (SelectedCharacter.HasMoved == true || waitingForFox || waitingForWolf || waitingForKetsu) return;

            Debug.Log("Move Action: " + direction.ToString());

            switch (SelectedCharacter.Type)
            {
                case MapObjectType.Fox:
                    waitingForFox = true;
                    Fox.MoveTo(direction, delegate {
                        waitingForFox = false;
                        if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
                    });
                    if (Wolf != null)
                    {
                        waitingForWolf = true;
                        Wolf.MoveTo(direction.Opposite(), delegate {
                            waitingForWolf = false;
                            if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
                        });
                    }
                    break;
                case MapObjectType.Wolf:
                    waitingForWolf = true;
                    Wolf.MoveTo(direction, delegate {
                        waitingForWolf = false;
                        if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
                    });
                    if (Fox != null)
                    {
                        waitingForFox = true;
                        Fox.MoveTo(direction.Opposite(), delegate {
                            waitingForFox = false;
                            if (!waitingForFox && !waitingForWolf && !waitingForKetsu) mapManager.CheckSolved();
                        });
                    }
                    break;
                case MapObjectType.Ketsu:
                    waitingForKetsu = true;
                    Ketsu.MoveTo(direction, delegate {
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