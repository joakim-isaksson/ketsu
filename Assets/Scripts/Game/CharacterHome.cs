using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class CharacterHome : MapObject
    {
        public CharacterType For;

        [HideInInspector]
        public Character Inside;

        MapManager manager;

        CharacterHome[] homes;

        void Awake()
        {
            manager = FindObjectOfType<MapManager>();
        }

        void Start()
        {
            homes = FindObjectsOfType<CharacterHome>();
        }

        void OnTriggerEnter(Collider other)
        {
            Character character = other.GetComponent<Character>(); 
            if (character != null && character.CharType == For)
            {
                Inside = character;
                foreach (CharacterHome home in homes)
                {
                    if (home.Inside == null) return;
                }
                manager.OnMapSolved();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Character character = other.GetComponent<Character>();
            if (character != null && character.CharType == For)
            {
                Inside = null;
            }
        }
    }
}