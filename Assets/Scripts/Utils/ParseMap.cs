using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System;

/* Map parser executes once when you open the scene,
 * if the scene is new, i.e. has not been initialized before.
 * 
 * If the scene has been initialized before, the easiest way to run 
 * Map parser again is to uncheck Initialized and duplicate the 
 * GameObject holding the Map parser.
 *
 * The Map file used must be in .json format,
 * and is loaded based on user's selection.
 */

[ExecuteInEditMode]
public class ParseMap : MonoBehaviour {

	//map structure to hold information
	public class Map {
		public int height;
    	public int width;
    	public int amountOfTiles;
    	//dictionary of <layer name, layer data>
    	public Dictionary<string, long[,]> tiles;
	}

	#if UNITY_EDITOR
    [SerializeField]
    private bool initialized = false;
 
    void Awake() { Initialize(); }
 
    void Initialize()
    {
        if (initialized)
        	return;
 
		//init new map
		Map map = new Map();
		map.tiles = new Dictionary<string, long[,]>();
		string tileString = "";
		string layername = "";
		

		//read json file
		string line = null;

		//which map to read, ask user
		string path = EditorUtility.OpenFilePanel("Map file to load", "Assets/Maps/", "json");
		StreamReader reader = new StreamReader(path);
		while ((line = reader.ReadLine()) != null) {	        	
			int start = line.IndexOf(":", 0)+1;
        	int end = line.IndexOf(",", start);

	        if(line.Contains("data")){
	        	tileString = line.Substring(start+1, line.LastIndexOf(',')-2-start);

				while ((line = reader.ReadLine()) != null) {
			        //parse the variables
					start = line.IndexOf(":", 0)+1;
		        	end = line.IndexOf(",", start);

		        	//layer name
					if(line.Contains("name"))
			        	layername = line.Substring(start+1, line.LastIndexOf(',')-2-start);
			   		//height & width for the map
			        if(line.Contains("\"height") && map.height == 0)
			        	int.TryParse(line.Substring(start, end - start), out map.height);
			        if(line.Contains("\"width")){
			        	if(map.width == 0){
			        		int.TryParse(line.Substring(start, end - start), out map.width);
			        		break;
			        	}
			        	else
			        		break;
			        }
			        
				}

				//parse the string of tiles to an actual array
			    long[,] layerdata = parseTiles(tileString, map.width, map.height);
				map.tiles.Add(layername, layerdata);
	        }
	        if(line.Contains("tilecount"))
	        	int.TryParse(line.Substring(start, end - start), out map.amountOfTiles);
	        
	    }
	    //instantiate GameObjects via prefabs
	    createObjects(map.tiles, map.width, map.height);

        initialized = true;
	}

	long[,] parseTiles(string tileString, int x, int y){
		tileString = Regex.Replace(tileString, @"\s+", "");
		string[] substrings = tileString.Split(',');

		long[,] tiles = new long[y, x];
		for(int i=0; i<y; i++)
			for(int j=0; j<x; j++)
				long.TryParse(substrings[i*x+j], out tiles[i,j]);
		
		return tiles;
	}

	Dictionary<int, GameObject> loadPrefabsFromFile(){
		Dictionary<int, GameObject> prefabs = new Dictionary<int, GameObject>();

		StreamReader reader = new StreamReader("Assets/Maps/tiles.txt");
		string line = null;

		while ((line = reader.ReadLine()) != null) {
			int n = 0;
			int.TryParse(line.Substring(0, line.IndexOf(' ')), out n);
			string name = line.Substring(line.LastIndexOf(' ')+1);

			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Maps/Prefabs/"+name+".prefab", typeof(GameObject));
			prefabs.Add(n, prefab);
		}	

		return prefabs;
	}

	void createObjects(Dictionary<string, long[,]> tiles, int x, int y){

		Dictionary<int, GameObject> prefabs = loadPrefabsFromFile();
		
		//Create correct objects
        foreach(KeyValuePair<string, long[,]> entry in tiles){

        	//Instantiate ground things
			if(entry.Key=="Ground"){
				GameObject GroundParent = new GameObject();
				GroundParent.name = "Ground";
				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						for(int k=1; k<=30; k++){
							int rotation, val;
							getRotation(entry.Value[y-i-1,x-j-1], out rotation, out val);

							if(prefabs.ContainsKey(k) && k==val){
								Debug.Log(rotation + " " + k);
								GameObject item = Instantiate(prefabs[k], new Vector3(-j+x-1, 0, i), Quaternion.Euler(0, rotation, 0));
								item.transform.parent = ObjectParent.transform;
							}
						}
					}
				}
			}
			 
			//Instantiate obstacles
			if(entry.Key=="Obstacles"){
				GameObject ObstacleParent = new GameObject();
				ObstacleParent.name = "Obstacles";
				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						for(int k=31; k<=80; k++){
							int rotation, val;
							getRotation(entry.Value[y-i-1,x-j-1], out rotation, out val);

							if(prefabs.ContainsKey(k) && k==val){
								Debug.Log(rotation + " " + k);
								GameObject item = Instantiate(prefabs[k], new Vector3(-j+x-1, 0, i), Quaternion.Euler(0, rotation, 0));
								item.transform.parent = ObjectParent.transform;
							}
						}
					}
				}

			}
			//Instantiate objects = spawn points etc.
			if(entry.Key=="Objects"){
				GameObject ObjectParent = new GameObject();
				ObjectParent.name = "Objects";
				//GameObject item;
				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						for(int k=140; k<=200; k++){
							int rotation, val;
							getRotation(entry.Value[y-i-1,x-j-1], out rotation, out val);

							if(prefabs.ContainsKey(k) && k==val){
								Debug.Log(rotation + " " + k);
								GameObject item = Instantiate(prefabs[k], new Vector3(-j+x-1, 0, i), Quaternion.Euler(0, rotation, 0));
								item.transform.parent = ObjectParent.transform;
							}
						}
					}
				}
			}
		}
	}

	void getRotation(long key, out int rotation, out int val){
		//Rotations 2684354560 == 90deg (0xA), 3221225472 == 180 deg (0xC), 1610612736 == 270 (0x6)
		long deg90 = 2684354560;
		long deg180 = 3221225472;
		long deg270 = 1610612736;
		rotation=0;
		val=0;

		if(key < deg270){
			rotation = 0;
			val = Convert.ToInt32(key);
		}
		else if(key > deg90 && key < deg180){
			rotation = 90;
			val = Convert.ToInt32(key-deg90);
		}
		else if(key > deg180){
			rotation = 180;
			val = Convert.ToInt32(key-deg180);
		}
		else if(key > deg270 && key < deg90){
			rotation = 270;
			val = Convert.ToInt32(key-deg270);
		}
	}
	#endif

}

