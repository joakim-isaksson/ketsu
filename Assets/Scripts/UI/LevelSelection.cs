using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelection : MonoBehaviour {
	public int level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
        // this object was clicked - do something
        SceneManager.LoadScene(level+1);
  	}  
}
