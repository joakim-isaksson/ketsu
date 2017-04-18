using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class WinPopup : MonoBehaviour
    {
        public GameObject WinPanel;
        public GameObject ControlPanel;
        public GameObject Settings;
        MapManager mapManger;

        void Awake()
        {
        	Settings.SetActive(false);
        	WinPanel.SetActive(false);
			ControlPanel.SetActive(true);
        }

        void Start()
        {
            mapManger = FindObjectOfType<MapManager>();
        }

        void Update()
        {
			if (mapManger.Solved){
				WinPanel.SetActive(true);
				ControlPanel.SetActive(false);
				Settings.SetActive(false);
			} 
			else {
				if(Input.GetKeyDown(KeyCode.Tab)){
					ControlPanel.SetActive(false);
					Settings.SetActive(true);
				}
				if(Input.GetKeyDown(KeyCode.Escape)){
					ControlPanel.SetActive(true);
					Settings.SetActive(false);
				}
			}
        }
    }
}