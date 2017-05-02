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

        Image[] Colors;
        public Sprite Locked;
        public Sprite Power;
        public Sprite Off;
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
            Colors = GetComponentsInChildren<Image>();
            for (int i=1; i<Colors.Length; i++)
        	{
        		if(i > charHandler.MaxKetsuPower)
        			Colors[i].sprite = Locked;
                else if (i > charHandler.KetsuPower)
                    Colors[i].sprite = Off;
                else
                    Colors[i].sprite = Power;
        	}
            GameObject parent = GameObject.Find("Objects");
            Ketsu = parent.transform.Find("Ketsu(Clone)").gameObject;
            running = false;
            Orig = Colors[0].sprite;
        }

        void Update()
        {
            for (int i=1; i<Colors.Length; i++)
        	{
        		if(i > charHandler.KetsuPower && Colors[i].sprite != Locked)
        			Colors[i].sprite = Off;
                else if(Colors[i].sprite == Locked)
                    break;
        		else
        			Colors[i].sprite = Power;
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
                    Colors[0].sprite = Orig;
                }
            }
        }

        IEnumerator Blinking(){
            while(running){
                Colors[0].sprite = Orig;
                yield return new WaitForSeconds(0.8f);
                Colors[0].sprite = Blink;
                yield return new WaitForSeconds(0.8f);
            }
        }
    }
}