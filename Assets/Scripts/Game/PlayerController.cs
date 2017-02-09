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

        Character targetCharacter;
        Vector2 touchStartPos;
        int waitingForCallbacks;

        void Awake()
        {
            
        }

        void Start()
        {
            // Find characters to control
            foreach (MapObject obj in MapManager.Instance.CurrentMap.DynamicLayer)
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
            targetCharacter = Fox;
        }

        void Update()
        {
            HandleKeyInputs();
            HandleTouchInputs();
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
                            if ((touch.position.x > touchStartPos.x)) MoveAction(Direction.Right);
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

            Debug.Log("Tap Point: " + tapPoint + ", Selected Tile: " + selectedTilePos + ", Target Char Point: " + targetCharacter.transform.position);

            // Character selection
            if (!CharacterSelectionAction(selectedTilePos))
            {
                // Move action
                if (Mathf.Abs(targetCharacter.Position.X - selectedTilePos.X) >
                    Mathf.Abs(targetCharacter.Position.Y - selectedTilePos.Y))
                {
                    if (targetCharacter.Position.X < selectedTilePos.X) MoveAction(Direction.Right);
                    else MoveAction(Direction.Left);
                }
                else
                {
                    if (targetCharacter.Position.Y < selectedTilePos.Y) MoveAction(Direction.Forward);
                    else MoveAction(Direction.Back);
                }
            }
        }

        void MoveAction(Direction direction)
        {
            if (waitingForCallbacks > 0) return;

            Debug.Log("Move Action: " + direction.ToString());

            switch(targetCharacter.Type)
            {
                case MapObjectType.Fox:
                    waitingForCallbacks += 2;
                    Fox.MoveTo(direction, delegate { waitingForCallbacks--; });
                    Wolf.MoveTo(direction.Opposite(), delegate { waitingForCallbacks--; });
                    break;
                case MapObjectType.Wolf:
                    waitingForCallbacks += 2;
                    Fox.MoveTo(direction.Opposite(), delegate { waitingForCallbacks--; });
                    Wolf.MoveTo(direction, delegate { waitingForCallbacks--; });
                    break;
                case MapObjectType.Ketsu:
                    waitingForCallbacks += 1;
                    Ketsu.MoveTo(direction, delegate { waitingForCallbacks--; });
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
                targetCharacter = Fox;
                return true;
            }
            else if (Wolf.Position.Equals(selectedTilePos))
            {
                Debug.Log("Wolf Selected");
                targetCharacter = Wolf;
                return true;
            }

            return false;
        }
    }
}