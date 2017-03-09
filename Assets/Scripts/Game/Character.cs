using Ketsu.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Ketsu.Game
{
	public class Character : MapObject
	{
		[Header("Animations")]
		public float MoveAnimTime;

		[Header("Sounds")]
		public string SfxMove;

		void Start()
		{

		}

		void Awake()
		{

		}

		public void MoveTo(Vector3 position, Action callback)
		{
            AkSoundEngine.PostEvent(SfxMove, gameObject);
            MoveAnimation(position, callback);
		}

        public void TakeDamage(float amount)
		{
            Flash(Color.white, 0.05f, 0.05f, 3, null);
        }

		void MoveAnimation(Vector3 target, Action callback)
		{
			StartCoroutine(RunMoveAnimation(target, callback));
		}

		IEnumerator RunMoveAnimation(Vector3 target, Action callback)
		{
			Vector3 start = transform.position;

			transform.LookAt(target);

			float timePassed = 0.0f;
			do
			{
				timePassed += Time.deltaTime;
				float progress = Mathf.Min(timePassed / MoveAnimTime, 1.0f);
				transform.position = Vector3.Lerp(start, target, progress);

				yield return null;

			} while (timePassed < MoveAnimTime);

			if (callback != null) callback();
		}
	}
}