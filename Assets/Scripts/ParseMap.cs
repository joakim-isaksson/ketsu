using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

/* Map parser executes once when you open the scene,
 * if the scene is new, i.e. has not been initialized before.
 * 
 * If the scene has been initialized before, the easiest way to run 
 * Map parser again is to duplicate the GameObject holding it.
 *
 * The Map file used must be in .json format,
 * and for now is only specified in code, will be changed later.
 */

[ExecuteInEditMode]
public class ParseMap : MonoBehaviour {
	//map structure to hold information
	public class Map {
		public int height;
    	public int width;
    	public int amountOfTiles;
    	public int[,] tiles;
    	public List<string> layers;
	}

	public Transform GroundTest;
	public Transform WaterTest;
	public Transform SandTest;
	public Transform RockTest;

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
		map.layers = new List<string>();
		string tileString = "";

		//read json file
		string line = null;
		int n = 0;
		//want to save height, width and the tile numbers
		List<string> save = new List<string> {"\"height", "\"width", "data"};

		//which map to read, make this smarter later
		StreamReader reader = new StreamReader("./Assets/Maps/ParserTestMap/TestMap.json");
		while ((line = reader.ReadLine()) != null) {
	        //if we want to check the line or not
	       	for(int i=0; i<save.Count; i++)
	       		if(!line.Contains(save[i]))
	            	continue;
	        	
	        //parse the variables
			int start = line.IndexOf(":", 0)+1;
        	int end = line.IndexOf(",", start);

	        if(line.Contains("\"height") && map.height == 0)
	        	int.TryParse(line.Substring(start, end - start), out map.height);
	        if(line.Contains("\"width") && map.width == 0)
	        	int.TryParse(line.Substring(start, end - start), out map.width);
	        if(line.Contains("data")){
	        	tileString = line.Substring(start+1, line.LastIndexOf(',')-2-start);
	        	n++;
	        }
	        
	    }
	    Debug.Log(map.width + " "+ map.height + " " + tileString);
	    map.amountOfTiles = map.width * map.height;

		//parse the string of tiles to an actual array
	    map.tiles = parseTiles(tileString, map.width, map.height);

	    //instantiate GameObjects via prefabs
	    createObjects(map.tiles, map.width, map.height);

        initialized = true;
	}

	int[,] parseTiles(string tileString, int x, int y){
		tileString = Regex.Replace(tileString, @"\s+", "");
		string[] substrings = tileString.Split(',');

		int[,] tiles = new int[y, x];
		for(int i=0; i<y; i++) {
			for(int j=0; j<x; j++){
				int.TryParse(substrings[i*y+j], out tiles[i,j]);
			}
		}
		return tiles;
	}

	void createObjects(int[,] tiles, int x, int y){
		for(int i=0; i<y; i++){
			for(int j=0; j<x; j++){
				if(tiles[i,j]==2)
					Instantiate(GroundTest, new Vector3(-j+x-1, 0, i), Quaternion.identity);
				if(tiles[i,j]==1)
					Instantiate(RockTest, new Vector3(-j+x-1, 0, i), Quaternion.identity);
				if(tiles[i,j]==3)
					Instantiate(SandTest, new Vector3(-j+x-1, 0, i), Quaternion.identity);
				if(tiles[i,j]==4)
					Instantiate(WaterTest, new Vector3(-j+x-1, 0, i), Quaternion.identity);
			}
		}
	}
	#endif

}

