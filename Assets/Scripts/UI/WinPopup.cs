using Ketsu.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class WinPopup : MonoBehaviour
    {
        Text winText;
        MapManager mapManger;

        void Awake()
        {
			winText = GetComponent<Text>();
			mapManger = FindObjectOfType<MapManager>();
        }

        void Start()
        {

        }

        void Update()
        {
			if (mapManger.Solved) winText.enabled = true;
			else winText.enabled = false;
        }
    }
}