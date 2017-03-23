using Ketsu.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class WinPopup : MonoBehaviour
    {
        public GameObject WinPanel;
        MapManager mapManger;

        void Awake()
        {
        }

        void Start()
        {
            mapManger = FindObjectOfType<MapManager>();
        }

        void Update()
        {
			if (mapManger.Solved) WinPanel.SetActive(true);
			else WinPanel.SetActive(false);
        }
    }
}