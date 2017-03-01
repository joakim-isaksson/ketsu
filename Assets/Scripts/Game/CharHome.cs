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

        CharHome[] homes;

        void Awake()
        {

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