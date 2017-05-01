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
	    public string SfxSlideStart;
	    public string SfxSlideStop;
        public string SwitchGroupName;

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

                if (transform.position != newPosition)
                {
                    if (Type != MapObjectType.Ketsu) AkSoundEngine.SetSwitch(SwitchGroupName, targetGroundType.ToString(), gameObject);
                    if (targetGroundType != MapObjectType.Ice) AkSoundEngine.PostEvent(SfxMove, gameObject);
                }

                if (callback != null) callback();
            }
            else
            {
                if (targetGroundType == MapObjectType.Mud) stuckInMud = true;

                if (Type != MapObjectType.Ketsu) AkSoundEngine.SetSwitch(SwitchGroupName, targetGroundType.ToString(), gameObject);
                if (targetGroundType != MapObjectType.Ice) AkSoundEngine.PostEvent(SfxMove, gameObject);

                StartCoroutine(RunMoveAnimation(newPosition, callback));
            }
		}

	    public bool InMud()
	    {
	        return stuckInMud;

	    }

		IEnumerator RunMoveAnimation(Vector3 target, Action callback)
		{
		    Vector3 start = transform.position;

		    bool sliding = Vector3.Distance(start, target) > 1.5f;

		    if (sliding) AkSoundEngine.PostEvent(SfxSlideStart, gameObject);

            anim.SetBool("Walking", true);

            transform.LookAt(target);

		    float animTime = MoveAnimTime * Vector3.Distance(start, target);
            float timePassed = 0.0f;
			do
			{
				timePassed += Time.deltaTime;
				float progress = Mathf.Min(timePassed / animTime, 1.0f);
				transform.position = Vector3.Lerp(start, target, progress);

				yield return null;

			} while (timePassed < animTime);

            anim.SetBool("Walking", false);

		    if (sliding) AkSoundEngine.PostEvent(SfxSlideStop, gameObject);

            if (callback != null) callback();
		}
	}
}