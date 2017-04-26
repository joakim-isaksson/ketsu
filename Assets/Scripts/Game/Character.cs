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
        
        Flasher flasher;

        public bool StuckInMud;

        void Awake()
        {
            flasher = GetComponent<Flasher>();
        }

        void Start()
		{

		}

	    public void MoveTo(Vector3 newPosition, bool backwards, Action callback)
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
                MoveAnimation(newPosition, backwards, callback);
            }
		}

        public void TakeDamage(float amount)
		{
            flasher.Flash(Color.white, 0.05f, 0.05f, 3, null);
        }

		void MoveAnimation(Vector3 target, bool backwards, Action callback)
		{
			StartCoroutine(RunMoveAnimation(target, backwards, callback));
		}

		IEnumerator RunMoveAnimation(Vector3 target, bool backwards, Action callback)
		{
			Vector3 start = transform.position;

            float animTime = MoveAnimTime * Vector3.Distance(start, target);

            transform.LookAt(target);
            if (backwards) transform.Rotate(Vector3.up, 180.0f);

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