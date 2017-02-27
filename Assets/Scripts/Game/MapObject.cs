using Ketsu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class MapObject : MonoBehaviour
	{
        public MapObjectType Type = MapObjectType.Undefined;
        public MapLayer Layer = MapLayer.Dynamic;

        [HideInInspector]
        public IntVector2 Position;

        /// <summary>
        /// Update object's position from the object's world position
        /// </summary>
        public void UpdatePositionFromWorld()
        {
            Position = IntVector2.FromXZ(transform.position);
        }

        public void DelayedAction(float time, Action action)
        {
            StartCoroutine(RunDelayedAction(time, action));
        }

        IEnumerator RunDelayedAction(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            if (action != null) action();
        }
    }
}