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
                yield return new WaitForSeconds(Random.Range(5.0f, 20.0f));
                for (int i=0; i<Lights.Length; i++){
                	int n = Random.Range(0, Lights.Length);
                	if(Random.Range(1,3)%2==0){
                		Lights[n].sprite = Blink;
                		break;
                	}
				}
                yield return new WaitForSeconds(0.5f);
            }
        }
	}
}