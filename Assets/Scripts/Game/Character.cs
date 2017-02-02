using Ketsu.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Character : MonoBehaviour
    {
        public float MovementTime;

		public CharacterType Type;

		[HideInInspector]
		public IntVector2 Position;

		MapManager map;

        void Awake()
        {

        }

        void Start()
        {
			map = MapManager.Instance;
        }

        void Update()
        {

        }

        public void MoveTo(Direction direction, Action callback)
        {
			if (!CanMoveTo(direction)) return;
            StartCoroutine(AnimateTo(direction, callback));
        }

		bool CanMoveTo(Direction direction)
		{

			map.CurrentMap.Obstacles.
            return false;
		}

        IEnumerator AnimateTo(Direction direction, Action callback)
        {
			Vector3 start = transform.position;
			Vector3 target = start + direction.ToVector3();
            
            float timePassed = 0.0f;
            do
            {
                yield return null;
                timePassed += Time.deltaTime;
                transform.position = Vector3.Lerp(start, target, Mathf.Min(timePassed / MovementTime, 1.0f));
            } while (timePassed < MovementTime);

            callback();
        }
    }
}