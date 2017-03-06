using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.Utils
{
	public class ScreenFaider : MonoBehaviour
	{
		[HideInInspector]
		public static ScreenFaider Instance = null;

		public RawImage Foreground;
		public Color DefaultColor;

		/// <summary>
		/// Destroy this singleton instance
		/// </summary>
		public static void DestroySingleton()
		{
			Destroy(Instance.gameObject);
			Instance = null;
		}

		void Awake()
		{
			// Make this an indestructible singleton
			if (Instance == null) Instance = this;
			else if (!Instance.Equals(this)) Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
		}

		public void FadeIn(float seconds, Action callback)
		{
			StartCoroutine(StartFadeIn(DefaultColor, seconds, callback));
		}

		public void FadeOut(float seconds, Action callback)
		{
			StartCoroutine(StartFadeOut(DefaultColor, seconds, callback));
		}

		public void FadeIn(Color color, float seconds, Action callback)
		{
			StartCoroutine(StartFadeIn(color, seconds, callback));
		}

		public void FadeOut(Color color, float seconds, Action callback)
		{
			StartCoroutine(StartFadeOut(color, seconds, callback));
		}

		IEnumerator StartFadeIn(Color color, float seconds, Action callback)
		{
			Foreground.gameObject.SetActive(true);
			Foreground.color = new Color(color.r, color.g, color.b, 0);

			float progress = 0;
			while (Foreground.color.a < 0.99f)
			{
				progress += (Time.deltaTime / seconds);
				Foreground.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0.0f, 1.0f, progress));
				yield return null;
			}
			Foreground.color = new Color(color.r, color.g, color.b, 1.0f);

			if (callback != null) callback();
		}

		IEnumerator StartFadeOut(Color color, float seconds, Action callback)
		{
			Foreground.gameObject.SetActive(true);
			Foreground.color = new Color(color.r, color.g, color.b, 1);

			float progress = 0;
			while (Foreground.color.a > 0.01f)
			{
				progress += (Time.deltaTime / seconds);
				Foreground.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1.0f, 0.0f, progress));
				yield return null;
			}
			Foreground.color = new Color(color.r, color.g, color.b, 0.0f);
			Foreground.gameObject.SetActive(false);

			if (callback != null) callback();
		}
	}
}