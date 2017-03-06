using UnityEngine;

namespace Ketsu.Game
{
	public class Home : MapObject
	{
		public MapObjectType ForType;

		[HideInInspector]
		public Character Inside;

		void Awake()
		{

		}

		void Start()
		{

		}

		void OnTriggerEnter(Collider other)
		{
			Character character = other.GetComponent<Character>();
			if (character != null && character.CharType == ForType)
			{
				Inside = character;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			Character character = other.GetComponent<Character>();
			if (character != null && character.CharType == ForType)
			{
				Inside = null;
			}
		}
	}
}