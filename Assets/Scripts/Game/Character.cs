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
        
        Flasher flasher;

        bool stuckInMud;

        void Awake()
        {
            flasher = GetComponent<Flasher>();
        }

        void Start()
		{

		}

		public void MoveTo(Vector3 newPosition, MapObjectType targetGroundType, Action callback)
		{
            if (stuckInMud || transform.position == newPosition)
            {
                stuckInMud = false;
                if (callback != null) callback();
            }
            else
            {
                if (targetGroundType == MapObjectType.Mud) stuckInMud = true;

                AkSoundEngine.PostEvent(SfxMove, gameObject);
                MoveAnimation(newPosition, callback);
            }
		}

        public void TakeDamage(float amount)
		{
            flasher.Flash(Color.white, 0.05f, 0.05f, 3, null);
        }

		void MoveAnimation(Vector3 target, Action callback)
		{
			StartCoroutine(RunMoveAnimation(target, callback));
		}

		IEnumerator RunMoveAnimation(Vector3 target, Action callback)
		{
			Vector3 start = transform.position;

            float animTime = MoveAnimTime * Vector3.Distance(start, target);

            transform.LookAt(target);

			float timePassed = 0.0f;
			do
			{
				timePassed += Time.deltaTime;
				float progress = Mathf.Min(timePassed / animTime, 1.0f);
				transform.position = Vector3.Lerp(start, target, progress);

				yield return null;

			} while (timePassed < animTime);

			if (callback != null) callback();
		}
	}
}