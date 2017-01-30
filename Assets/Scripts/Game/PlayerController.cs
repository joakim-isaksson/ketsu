using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class PlayerController : MonoBehaviour
    {
        Character Fox;
        Character Wolf;

        int waitingForActions;

        void Awake()
        {
            Fox = GameObject.FindGameObjectWithTag("Fox").GetComponent<Character>();
            Wolf = GameObject.FindGameObjectWithTag("Wolf").GetComponent<Character>();
        }

        void Start()
        {

        }

        void Update()
        {
            #if UNITY_STANDALONE
                HandleKeyInputs();
            #endif

            #if UNITY_ANDROID || UNITY_IOS
                HandleTouchInputs();
            #endif
        }

        void HandleKeyInputs()
        {
            if (Input.GetAxis("Left") > 0) MoveAction(Direction.Left);
            else if (Input.GetAxis("Right") > 0) MoveAction(Direction.Right);
            else if (Input.GetAxis("Up") > 0) MoveAction(Direction.Up);
            else if (Input.GetAxis("Down") > 0) MoveAction(Direction.Down);
        }

        void HandleTouchInputs()
        {
            // TODO
        }

        void MoveAction(Direction direction)
        {
            if (waitingForActions > 0) return;

            waitingForActions = 2;
            Fox.MoveTo(direction, delegate { waitingForActions--; });
            Wolf.MoveTo(Opposite(direction), delegate { waitingForActions--; });
        }

        Direction Opposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
            }

            return direction;
        }
    }
}