using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ketsu.Game;
using System.Linq;

public class PrefabRandomizer : MonoBehaviour {
	void Start () {
		if(gameObject.GetComponent<MapObject>().Type == MapObjectType.Tree ||
			gameObject.GetComponent<MapObject>().Type == MapObjectType.Bush){
			float rnd = Random.Range(0.8f, 1.2f);
			gameObject.transform.localScale = new Vector3(rnd, rnd, rnd);
			Vector3 pos = gameObject.transform.position;
			gameObject.transform.position = new Vector3(pos.x+Random.Range(0.0f, 0.2f), pos.y, pos.z+Random.Range(0.0f, 0.2f));
		}
		if(gameObject.transform.name.Substring(0,10) == "GroundEdge"){
			GameObject[] prefabs = Resources.LoadAll("Prefabs", typeof(GameObject)).Cast<GameObject>().ToArray();
			List<GameObject> edges = new List<GameObject>();
			foreach (GameObject p in prefabs){
				if(p.transform.name.StartsWith("GroundEdge")){
					Debug.Log(p.transform.name);
					edges.Add(p);
				}
			} 
			Vector3 pos = gameObject.transform.position;
			Quaternion rot = gameObject.transform.rotation;
			Transform parent = gameObject.transform.parent;
			Destroy(gameObject);
			GameObject item = Instantiate(edges[Random.Range(0, edges.Count)], pos, rot);
			item.transform.parent = parent;

		}
	}
}
