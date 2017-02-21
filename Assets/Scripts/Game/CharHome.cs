using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class CharHome : MapObject
    {
        public CharType ForType;

        [HideInInspector]
        public Character Inside;

        MapManager manager;

        CharHome[] homes;

        void Awake()
        {
            manager = FindObjectOfType<MapManager>();
        }

        void Start()
        {
            homes = FindObjectsOfType<CharHome>();
        }

        void OnTriggerEnter(Collider other)
        {
            Character character = other.GetComponent<Character>(); 
            if (character != null && character.CharType == ForType)
            {
                Inside = character;
                foreach (CharHome home in homes)
                {
                    if (home.Inside == null) return;
                }
                manager.OnMapSolved();
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