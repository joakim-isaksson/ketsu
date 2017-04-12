using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ketsu.UI
{
public class PlanetRotate : MonoBehaviour {
	float rotationSpeed = 0.1f;
 
 	Vector3 speed = new Vector3();
 	bool dragging = false;
 	static Quaternion rot;

	public GameObject spaceship;
 	static Quaternion rotSpaceship;
 	static bool rotated = false;

 	float widthRatio = Screen.width / 1920.0f;
	float heightRatio = Screen.height / 1080.0f;
 
 	void Start(){
 		transform.rotation = rot;
 		if(rotated)
 			spaceship.transform.rotation = rotSpaceship;

 		Debug.Log(widthRatio+ " " + widthRatio*rotationSpeed+" "+ heightRatio + " "+heightRatio*rotationSpeed);
 	}

	void OnMouseOver() 
	{
	    dragging = true;
	}
 
 	void Update () 
 	{
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		if (Input.GetMouseButton(0) && dragging) {
         	speed = new Vector3(-Input.GetAxis ("Mouse X"), Input.GetAxis("Mouse Y"), 0);
     	}
#else
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            speed = new Vector3(-touchDeltaPosition.x, touchDeltaPosition.y, 0);
        }
#endif
     	 else {
         	if (dragging) {
             	speed = Vector3.zero;
             	dragging = false;
         	}
    	}
    	transform.Rotate(speed.y * rotationSpeed / widthRatio, speed.x * rotationSpeed / heightRatio, 0, Space.World);
    	rot = transform.rotation;
    	rotSpaceship = spaceship.transform.rotation;
    	rotated = true;
 	}
}

}