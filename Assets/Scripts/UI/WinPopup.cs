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
        	//Settings.SetActive(false);
        	//WinPanel.SetActive(false);
			//ControlPanel.SetActive(true);
			WinPanel.transform.localScale = new Vector3(0,0,0);
        }

        void Start()
        {
            mapManger = FindObjectOfType<MapManager>();
        }

        void Update()
        {
			if (mapManger.Solved){
				//WinPanel.SetActive(true);
				//ControlPanel.SetActive(false);
				//Settings.SetActive(false);
				WinPanel.transform.localScale = new Vector3(1,1,1);
				ControlPanel.transform.localScale = new Vector3(0,0,0);

                StartCoroutine(TypeText());
                if(Time.time < StartTime+1)
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
            int level = SceneManager.GetActiveScene().buildIndex-1;
            if(level < 10)
            LevelNumber.text = "0"+level+"/28";
            else
            LevelNumber.text = level+"/28";
        }
    }
}