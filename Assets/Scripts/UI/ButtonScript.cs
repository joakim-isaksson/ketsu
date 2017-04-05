using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	[Header("Sounds")]
	public string ButtonSound;

	// Use this for initialization
	void Start () {
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		AkSoundEngine.PostEvent(ButtonSound, gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
