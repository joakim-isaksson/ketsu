using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ketsu.Game;

public class PrefabRandomizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(gameObject.GetComponent<MapObject>().Type == MapObjectType.Tree ||
			gameObject.GetComponent<MapObject>().Type == MapObjectType.Bush){
			float rnd = Random.Range(0.8f, 1.2f);
			gameObject.transform.localScale = new Vector3(rnd, rnd, rnd);
			Vector3 pos = gameObject.transform.position;
			gameObject.transform.position = new Vector3(pos.x+Random.Range(0.0f, 0.2f), pos.y, pos.z+Random.Range(0.0f, 0.2f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
