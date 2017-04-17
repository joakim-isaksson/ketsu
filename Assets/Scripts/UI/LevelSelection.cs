using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelection : MonoBehaviour {
	public int level;
	public GameObject spaceship;
    Renderer rend;
    public bool important;
    Color StartColor = new Color(0.898f, 0.729f, 0.000f, 1.000f);
    Color EndColor = new Color(0.898f, 0.534f, 0.000f, 1.000f);

	void Start(){
		rend = GetComponent<Renderer>();
	}

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

  	void Update(){
  		if(important)
        	rend.material.color = Color.Lerp(StartColor, EndColor, Mathf.PingPong(Time.time, 1));
  	}
}
