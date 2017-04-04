using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelection : MonoBehaviour {
	public int level;
	public GameObject spaceship;

	void OnMouseDown(){
		if(spaceship.transform.rotation == transform.rotation)
			StartCoroutine(Load());
		else
        	StartCoroutine(Fly());
  	}  

  	IEnumerator Fly(){
  		AkSoundEngine.PostEvent("LevelMenu_UnlockedLevel_Select", spaceship);
		for(float t = 0f; t < 1; t += Time.deltaTime) {
            spaceship.transform.rotation = Quaternion.Lerp(spaceship.transform.rotation, transform.rotation, t);
            yield return null;
         }
  	}

  	IEnumerator Load(){
  		AkSoundEngine.PostEvent("LevelMenu_StartLevel_Select", spaceship);
		for(float t = 0f; t < 1; t += Time.deltaTime) {
            spaceship.transform.position = Vector3.Lerp(spaceship.transform.position, new Vector3(spaceship.transform.position.x, spaceship.transform.position.y, spaceship.transform.position.z+0.03f), t);
            yield return null;
        }
  		  SceneManager.LoadScene(level+1);
  		  yield return null;
  	}
}
