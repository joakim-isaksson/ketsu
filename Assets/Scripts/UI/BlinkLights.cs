using Game;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ketsu.Utils;

namespace Ketsu.UI
{
	public class BlinkLights : MonoBehaviour {
		Image[] Lights;
        public Sprite Off;
        public Sprite Blink;
		bool running;     
		// Use this for initialization
		void Start () {
			Lights = GetComponentsInChildren<Image>();
			StartCoroutine(Blinking());
		}
		
		IEnumerator Blinking(){
            while(true){
            	for (int i=0; i<Lights.Length; i++)
					Lights[i].sprite = Off;
                yield return new WaitForSeconds(1.0f);
                for (int i=0; i<Lights.Length; i++)
					Lights[i].sprite = Blink;
                yield return new WaitForSeconds(0.5f);
            }
        }
	}
}