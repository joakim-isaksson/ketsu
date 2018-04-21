using Ketsu.Utils;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Animator))]
	public class Track : ExtendedMonoBehaviour
	{
		public SpriteRenderer TrackRenderer;
		public Sprite[] TracksSprites;
		public string EntryAnimTrigger;
		public string RemoveAnimTrigger;
		public float RemoveDelay;

		private Animator _anim;

		private void Awake()
		{
			_anim = GetComponent<Animator>();
		}

		private void Start()
		{
			var randomSprite = TracksSprites[Random.Range(0, TracksSprites.Length)];
			TrackRenderer.sprite = randomSprite;
			_anim.SetTrigger(EntryAnimTrigger);
		}

		public void Remove()
		{
			_anim.SetTrigger(RemoveAnimTrigger);
			DelayedAction(RemoveDelay, () => Destroy(gameObject));
		}
	}
}