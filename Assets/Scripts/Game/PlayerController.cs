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

        Character fox;
        Character wolf;

        Vector3 touchStartPos;

        int waitingForActions;

        void Awake()
        {
            fox = GameObject.FindGameObjectWithTag("Fox").GetComponent<Character>();
            wolf = GameObject.FindGameObjectWithTag("Wolf").GetComponent<Character>();
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
            if (Input.GetAxis("Left") > 0) MoveAction(Vector3.left);
            else if (Input.GetAxis("Right") > 0) MoveAction(Vector3.right);
            else if (Input.GetAxis("Forward") > 0) MoveAction(Vector3.forward);
            else if (Input.GetAxis("Back") > 0) MoveAction(Vector3.back);
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

                    // Check if it was a swipe
                    if (Vector3.Distance(touchStartPos, touch.position) > Screen.height * DragDistance)
                    {
                        // Horiontal swipe
                        if (Mathf.Abs(touch.position.x - touchStartPos.x) > Mathf.Abs(touch.position.y - touchStartPos.y))
                        { 
                            if ((touch.position.x > touchStartPos.x))
                            {
                                Debug.Log("Right Swipe");
                                MoveAction(Vector3.right);
                            }
                            else
                            {
                                Debug.Log("Left Swipe");
                                MoveAction(Vector3.left);
                            }
                        }

                        // Vertical swipe
                        else
                        { 
                            if (touch.position.y > touchStartPos.y)
                            {
                                Debug.Log("Forward Swipe");
                                MoveAction(Vector3.forward);
                            }
                            else
                            {
                                Debug.Log("Backward Swipe");
                                MoveAction(Vector3.back);
                            }
                        }
                    }

                    // It is a tap
                    else
                    {
                        Debug.Log("Tap");
                    }
                }
            }
        }

        void MoveAction(Vector3 direction)
        {
            if (waitingForActions > 0) return;

            waitingForActions += 2;

            fox.MoveTo(direction, delegate { waitingForActions--; });
            wolf.MoveTo(-direction, delegate { waitingForActions--; });
        }
    }
}