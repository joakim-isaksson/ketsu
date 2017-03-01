using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public void LoadOn(int level){
		SceneManager.LoadScene(level);
	}

	public void NextLevel(){
		 int level = SceneManager.GetActiveScene().buildIndex;
		 SceneManager.LoadScene(level+1);
	}
}