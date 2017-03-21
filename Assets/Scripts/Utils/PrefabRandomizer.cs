using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ketsu.Game;
using System.Linq;

public class PrefabRandomizer : MonoBehaviour {

	void Randomize(GameObject item, Vector3 pos, Quaternion rot){
		float rnd = Random.Range(0.8f, 1.2f);
		item.transform.localScale = new Vector3(rnd, rnd, rnd);
		item.transform.position = new Vector3(pos.x+Random.Range(0.0f, 0.1f), pos.y, pos.z+Random.Range(0.0f, 0.1f));
		item.transform.rotation = Quaternion.Euler(rot.x, rot.y+Random.Range(0,90), rot.z);
	}

	void RandomPrefab(GameObject orig, Vector3 pos, Quaternion rot, Transform parent, string name){
			GameObject[] list = Resources.LoadAll("Prefabs", typeof(GameObject)).Cast<GameObject>().ToArray();
			List<GameObject> prefabs = new List<GameObject>();
			foreach (GameObject p in list){
				if(p.transform.name.StartsWith(name)){
					prefabs.Add(p);
				}
			} 
			Destroy(orig);
			GameObject item = Instantiate(prefabs[Random.Range(0, prefabs.Count)], pos, rot);
			item.transform.parent = parent;
	}

	void Start () {
		string name = gameObject.transform.name;

		if(gameObject.GetComponent<MapObject>().Type == MapObjectType.Tree ||
			gameObject.GetComponent<MapObject>().Type == MapObjectType.Bush){
			Vector3 pos = gameObject.transform.position;
			Quaternion rot = gameObject.transform.rotation;
			Randomize(gameObject, pos, rot);
		}

		if(name.StartsWith("GroundEdge") || name.StartsWith("Bush") || name.StartsWith("TreeGroup") || name.StartsWith("Ice") || name.StartsWith("Island") || name.StartsWith("Mud") || name.StartsWith("Path")){
			name = name.Remove(name.IndexOf('('));
			Vector3 pos = gameObject.transform.position;
			Quaternion rot = gameObject.transform.rotation;
			Transform parent = gameObject.transform.parent;
			RandomPrefab(gameObject, pos, rot, parent, name);
		}
	}
}
