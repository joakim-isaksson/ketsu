using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("Percentage of the screens height")]
        public float DragDistance;

        [HideInInspector]
        public Character Fox;
        [HideInInspector]
        public Character Wolf;
        [HideInInspector]
        public Character Ketsu;
        [HideInInspector]
        public Character SelectedCharacter;

        Vector2 touchStartPos;
        int waitingForActions;

        void Awake()
        {
            
        }

        void Start()
        {
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

            // Set starting character
            Ketsu.gameObject.SetActive(false);
            SelectedCharacter = Fox;
        }

        void Update()
        {
#if UNITY_EDITOR
            HandleKeyInputs();
#else
            HandleTouchInputs();
#endif
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
                        if (Mathf.Abs(touch.position.x - touchStartPos.x) > Mathf.Abs(touch.position.y - touchStartPos.y))
                        { 
                            if ((touch.position.x < touchStartPos.x)) MoveAction(Direction.Right);
                            else MoveAction(Direction.Left);
                        }
                        else
                        { 
                            if (touch.position.y < touchStartPos.y) MoveAction(Direction.Forward);
                            else MoveAction(Direction.Back);
                        }
                    }

                    // It's a TAP
                    else
                    {
                        TapAction(Camera.main.ScreenToWorldPoint(touchStartPos));
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

        void MoveAction(Direction direction)
        {
            if (waitingForActions > 0 || SelectedCharacter.HasMoved == true) return;

            Debug.Log("Move Action: " + direction.ToString());

            switch(SelectedCharacter.Type)
            {
                case MapObjectType.Fox:
                    waitingForActions += 2;
                    Fox.MoveTo(direction, delegate { waitingForActions--; });
                    Wolf.MoveTo(direction.Opposite(), delegate { waitingForActions--; });
                    break;
                case MapObjectType.Wolf:
                    waitingForActions += 2;
                    Fox.MoveTo(direction.Opposite(), delegate { waitingForActions--; });
                    Wolf.MoveTo(direction, delegate { waitingForActions--; });
                    break;
                case MapObjectType.Ketsu:
                    waitingForActions += 1;
                    Ketsu.MoveTo(direction, delegate { waitingForActions--; });
                    break;
                default:
                    break;
            }
        }

        // Return true if character selected
        bool CharacterSelectionAction(IntVector2 selectedTilePos)
        {
            // Character selection
            if (Fox.Position.Equals(selectedTilePos))
            {
                Debug.Log("Fox Selected");
                SelectedCharacter = Fox;
                return true;
            }
            else if (Wolf.Position.Equals(selectedTilePos))
            {
                Debug.Log("Wolf Selected");
                SelectedCharacter = Wolf;
                return true;
            }

            return false;
        }
    }
}