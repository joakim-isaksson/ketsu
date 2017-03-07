using System;
using System.Collections;
using UnityEngine;

namespace Ketsu.Utils
{
	public class ExtendedMonoBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Invoke an action after given delay
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

        /// <summary>
        /// Flash this object in and out of given color
        /// </summary>
        /// <param name="color">Color to be flashed</param>
        /// <param name="fadeTime">Fade in/out time</param>
        /// <param name="keepTime">Time to keep the color before fading out</param>
        /// <param name="repeat">How many times to repeat the flash</param>
        /// <param name="callback">invoked after the task is completed</param>
        public void Flash(Color color, float fadeTime, float keepTime, int repeat, Action callback)
        {
            StartCoroutine(RunFlash(color, fadeTime, keepTime, repeat, callback));
        }

        IEnumerator RunFlash(Color color, float fadeTime, float keepTime, int repeat, Action callback)
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            Material[] materials = renderer.materials;
            Color[] startingColors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                startingColors[i] = materials[i].color;
            }

            for (int i = 0; i < repeat; i++)
            {
                float timePassed = 0.0f;
                while (timePassed < fadeTime)
                {
                    timePassed += Time.deltaTime;
                    float progress = Mathf.Min(timePassed / fadeTime, 1.0f);
                    for (int ii = 0; ii < materials.Length; ii++)
                    {
                        materials[ii].color = Color.Lerp(startingColors[ii], color, progress);
                    }
                    yield return null;
                }

                yield return new WaitForSeconds(keepTime);

                timePassed = 0.0f;
                while (timePassed < fadeTime)
                {
                    timePassed += Time.deltaTime;
                    float progress = Mathf.Min(timePassed / fadeTime, 1.0f);
                    for (int ii = 0; ii < materials.Length; ii++)
                    {
                        materials[ii].color = Color.Lerp(color, startingColors[ii], progress);
                    }
                    yield return null;
                }
            }

            if (callback != null) callback();
        }
    }
}