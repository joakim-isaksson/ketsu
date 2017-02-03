﻿using Ketsu.Utils;
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
        Vector3 touchStartPos;
        int waitingForCallbacks;

        void Awake()
        {
            Fox = GameObject.FindGameObjectWithTag("Fox").GetComponent<Character>();
            Wolf = GameObject.FindGameObjectWithTag("Wolf").GetComponent<Character>();
            targetCharacter = Fox;
        }

        void Start()
        {

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
                // TODO: not working
                CharacterSelectionAction(Camera.main.ScreenPointToRay(Input.mousePosition).origin);
            }
        }

        void HandleTouchInputs()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                // Touch started
                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("Touch Started");
                    touchStartPos = touch.position;
                }

                // Touch ended
                else if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log("Touch Ended");

                    // SWIPE
                    if (Vector3.Distance(touchStartPos, touch.position) > Screen.height * DragDistance)
                    {
                        if (Mathf.Abs(touch.position.x - touchStartPos.x) > Mathf.Abs(touch.position.y - touchStartPos.y))
                        { 
                            if ((touch.position.x > touchStartPos.x))
                            {
                                Debug.Log("Swipe Right");
                                MoveAction(Direction.Right);
                            }
                            else
                            {
                                Debug.Log("Swipe Left");
                                MoveAction(Direction.Left);
                            }
                        }
                        else
                        { 
                            if (touch.position.y > touchStartPos.y)
                            {
                                Debug.Log("Swipe Forward");
                                MoveAction(Direction.Forward);
                            }
                            else
                            {
                                Debug.Log("Swipe Back");
                                MoveAction(Direction.Back);
                            }
                        }
                    }

                    // TAP
                    else
                    {
                        // Character selection
                        if (!CharacterSelectionAction(touchStartPos))
                        {
                            // Move action
                            if (Mathf.Abs(targetCharacter.transform.position.x - touchStartPos.x) <
                                Mathf.Abs(targetCharacter.transform.position.y - touchStartPos.y))
                            {
                                if (touchStartPos.x > targetCharacter.transform.position.x)
                                {
                                    Debug.Log("Tap Right");
                                    MoveAction(Direction.Right);
                                }
                                else
                                {
                                    Debug.Log("Tap Left");
                                    MoveAction(Direction.Left);
                                }
                            }
                            else
                            {
                                if (touchStartPos.y > targetCharacter.transform.position.y)
                                {
                                    Debug.Log("Tap Forward");
                                    MoveAction(Direction.Forward);
                                }
                                else
                                {
                                    Debug.Log("Tap Back");
                                    MoveAction(Direction.Back);
                                }
                            }
                        }
                    }
                }
            }
        }

        void MoveAction(Direction direction)
        {
            if (waitingForCallbacks > 0) return;

            switch(targetCharacter.Type)
            {
                case CharacterType.Fox:
                    waitingForCallbacks += 2;
                    Fox.MoveTo(direction, delegate { waitingForCallbacks--; });
                    Wolf.MoveTo(direction.Opposite(), delegate { waitingForCallbacks--; });
                    break;
                case CharacterType.Wolf:
                    waitingForCallbacks += 2;
                    Fox.MoveTo(direction.Opposite(), delegate { waitingForCallbacks--; });
                    Wolf.MoveTo(direction, delegate { waitingForCallbacks--; });
                    break;
                case CharacterType.Ketsu:
                    waitingForCallbacks += 1;
                    Ketsu.MoveTo(direction, delegate { waitingForCallbacks--; });
                    break;
                default:
                    break;
            }
        }

        // Return true if character selected
        bool CharacterSelectionAction(Vector3 targetPos)
        {
            IntVector2 selectedTilePos = new IntVector2(
                (int)Mathf.Round(targetPos.x),
                (int)Mathf.Round(targetPos.z)
            );

            Debug.Log(selectedTilePos);

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