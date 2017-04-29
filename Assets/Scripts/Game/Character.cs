using System;
using System.Collections;
using Ketsu.Utils;
using UnityEngine;

namespace Game
{
	public class Character : MapObject
	{
		[Header("Animations")]
		public float MoveAnimTime;

		[Header("Sounds")]
		public string SfxMove;

        bool stuckInMud;

	    Animator anim;

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
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
            // do nothing
        }

		void MoveAnimation(Vector3 target, Action callback)
		{
			StartCoroutine(RunMoveAnimation(target, callback));
		}

		IEnumerator RunMoveAnimation(Vector3 target, Action callback)
		{
		    anim.SetBool("Walking", true);

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

            anim.SetBool("Walking", false);

            if (callback != null) callback();
		}
	}
}