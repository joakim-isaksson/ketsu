using Game;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ketsu.Utils;


namespace Ketsu.UI
{
    public class KetsuPowerMeter : MonoBehaviour
    {
        CharacterHandler charHandler;

        [Header("PowerBar")]
        public Component BG;
        Image[] Colors;

        public Sprite Blink;
        Sprite Orig;
        public GameObject Ketsu;  
        bool running; 

        void Awake()
        {

        }

        void Start()
        {
            charHandler = FindObjectOfType<CharacterHandler>();
            Colors = BG.GetComponentsInChildren<Image>();
            for (int i=0; i<Colors.Length-1; i++)
            {
                if(i > charHandler.KetsuPower)
                    Colors[i].enabled = false;
            }

            GameObject parent = GameObject.Find("Objects");
            if(parent.transform.Find("Ketsu(Clone)").gameObject != null)
                Ketsu = parent.transform.Find("Ketsu(Clone)").gameObject;
            running = false;
            Orig = gameObject.GetComponent<Image>().sprite;
        }

        void Update()
        {
            for (int i=0; i<Colors.Length-1; i++)
            {
                if(i > charHandler.KetsuPower)
                    Colors[i].enabled = false;
                else
                    Colors[i].enabled = true;
            }

            if(Ketsu){
                if(Ketsu.GetComponent<Flasher>().flashing){
                    if(!running){
                        running = true;
                        StartCoroutine(Blinking());
                    }
                }
                else {
                    running = false;
                    gameObject.GetComponent<Image>().sprite = Orig;
                }
            }
        }

        IEnumerator Blinking(){
            while(running){
                gameObject.GetComponent<Image>().sprite = Orig;
                yield return new WaitForSeconds(0.8f);
                gameObject.GetComponent<Image>().sprite = Blink;
                yield return new WaitForSeconds(0.8f);
            }
        }
    }
}