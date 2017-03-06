using System;
using System.Collections;
using UnityEngine;

namespace Ketsu.Utils
{
	public class ExtendedMonoBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Invoke action after delay
		/// </summary>
		/// <param name="seconds">Delay time in seconds</param>
		/// <param name="action">Action to invoke</param>
		public void DelayedAction(float seconds, Action action)
		{
			StartCoroutine(RunDelayedAction(seconds, action));
		}

		IEnumerator RunDelayedAction(float seconds, Action action)
		{
			yield return new WaitForSeconds(seconds);
			if (action != null) action();
		}
	}
}