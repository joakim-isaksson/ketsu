using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetsuSplitUI : MonoBehaviour {

	public GameObject leader;
	public Renderer leadvisible;

    void Start()
    {
    	leadvisible = GetComponentsInChildren<Renderer>()[0];
    	leadvisible.enabled = false;
    }

    void LateUpdate () 
    {
    	if(leader == null){
    	    leader = GameObject.Find("Ketsu(Clone)");
    	  	if(leader != null){
    	  		leadvisible.enabled = true;
		        Vector3 targetPosition = leader.transform.position;
		        targetPosition.y = transform.position.y; 
		        transform.position = targetPosition;
    	  	}
    	  	else
    			leadvisible.enabled = false;
    	}
    	else if(leader.activeSelf == false)
    	   	leadvisible.enabled = false;
    	else {
    		Vector3 targetPosition = leader.transform.position;
		    targetPosition.y = transform.position.y;
		    transform.position = targetPosition;
    	}
    }
}
