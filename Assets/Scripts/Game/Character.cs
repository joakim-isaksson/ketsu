using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class Character : MonoBehaviour
    {
        public float MovementTime;

        void Awake()
        {

        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void MoveTo(Vector3 direction, Action callback)
        {
            Vector3 target = transform.position + direction;
            StartCoroutine(AnimateTo(target, callback));
        }

        IEnumerator AnimateTo(Vector3 target, Action callback)
        {
            Vector3 start = transform.position;
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