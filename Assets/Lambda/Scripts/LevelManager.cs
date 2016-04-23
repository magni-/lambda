using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	private int currentLevel;
	private int spawnFreq;
	private int spawnRadius;
	private Game gameScript;
		
	// Use this for initialization
	void Start () {
		spawnRadius = 300;
		gameScript = GameObject.Find("game").GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((int) Random.Range(0, 200) == 126) {
			gameScript.spawnEnemy(spawnRadius);
		}
	}
	
	
	void nextLevel() {
		loadLevel(++currentLevel);
	}
	
	void loadLevel(int level) {
		//Todo
	}
}
