using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ketsu.UI
{
    public class WinPopup : MonoBehaviour
    {
        public GameObject WinPanel;
        public GameObject ControlPanel;
        public GameObject Settings;
        public Text LevelNumber;
        MapManager mapManger;
        float StartTime;
        bool blink=false;

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
                StartCoroutine(TypeText());
                Debug.Log(StartTime + " " + Time.time);
                if(Time.time < StartTime+4)
                    LevelNumber.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 0.8f));
                else
                    LevelNumber.color = Color.white;

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

        IEnumerator TypeText () {
            if(!blink){
                StartTime = Time.time;
                blink = true;
            }
            yield return new WaitForSeconds (0.5f);
            LevelNumber.text = "0"+(SceneManager.GetActiveScene().buildIndex-1)+"/30";

        }
    }
}