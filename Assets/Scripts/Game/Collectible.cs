using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class Collectible : MonoBehaviour
	{
		public float KetsuPower;
		public float MaxKetsuPower;
		public string CollectSfx;

		CharController controller;

		void Awake()
		{

		}

		void Start()
		{
			controller = FindObjectOfType<CharController>();
		}

		void Update()
		{

		}

		private void OnTriggerEnter(Collider other)
		{
			Character character = other.GetComponent<Character>();
			if (character != null)
			{
				controller.KetsuPower += KetsuPower;
				controller.MaxKetsuPower += MaxKetsuPower;
				AkSoundEngine.PostEvent(CollectSfx, gameObject);

				// FIXME: this is the thing from my nightmare
				MapManager.LoadedMap.DynamicLayer.Remove(GetComponent<MapObject>());
				Destroy(gameObject);
			}
		}
	}
}