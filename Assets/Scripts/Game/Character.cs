using System;
using System.Collections;
using System.Collections.Generic;
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

		[Header("Tracks")]
		public int MaxTracks;
		public GameObject TrackPrefab;
		private Queue<Track> _tracks;

        bool stuckInMud;

	    Animator anim;

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
	        _tracks = new Queue<Track>();
        }

		private void OnDisable()
		{
			foreach (var track in _tracks) track.Remove();
			_tracks.Clear();
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

	            UpdateTracks(newPosition);
                StartCoroutine(RunMoveAnimation(newPosition, callback));
            }
		}

	    public bool InMud()
	    {
	        return stuckInMud;

	    }

		private void UpdateTracks(Vector3 newPos)
		{
			if (_tracks.Count >= MaxTracks) _tracks.Dequeue().Remove();
			var newTrack = Instantiate(TrackPrefab, transform.position, Quaternion.identity);
			newTrack.transform.LookAt(newPos);
			_tracks.Enqueue(newTrack.GetComponent<Track>());
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