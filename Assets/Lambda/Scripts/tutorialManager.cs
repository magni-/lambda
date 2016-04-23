using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tutorialManager : MonoBehaviour {
	
	public GUIText guiTextObject;
	public List<string> tutoText;
	public List<float> textTime;
	public OTSprite keySprite;
	public OTSprite enemyPRefab;
	public List<Texture> keysTextures;
	public List<float> timeIndexTextures;
	public Game gameScript;
	private float elapsedTimeInTable;
	private bool initialized=false;
	public int currentIndex;
	private List<OTSprite>enemyList;
	private CannonTutorial cannonScript;
	bool bulletSpawned=false;
	bool enemiesSpawned=false;
	bool bombLaunched = false;
	
	// Use this for initialization
	void Start () {
		elapsedTimeInTable=2;
		enemyList = new List<OTSprite>();
		Application.targetFrameRate = 30;
	}
	
	// Update is called once per frame
	void Update () {
		if(!initialized && Time.timeSinceLevelLoad>2) {
				guiTextObject.text  = tutoText[0];
				elapsedTimeInTable += textTime[0];
			currentIndex = 0;
			initialized=true;
		}
		
		if(initialized) {
			
			if(Time.timeSinceLevelLoad>elapsedTimeInTable && currentIndex < tutoText.Count-1) {
				guiTextObject.text = tutoText [++currentIndex];
				elapsedTimeInTable += textTime[currentIndex];
				
				if(timeIndexTextures.Contains(currentIndex)) {
					keySprite.texture = keysTextures[timeIndexTextures.IndexOf(currentIndex)];
				}
			}
		}
		
		
		if(currentIndex == 9) {
			GameObject.Find("CannonFuel").GetComponent<CannonFuel>().setFuel(10);
		}	
		
		if(currentIndex == 3) {
			if(enemyList.Count == 0 && !enemiesSpawned) {

				gameScript.spawnEnemyPosition(new Vector3(-300,0,0),0f);
				gameScript.spawnEnemyPosition(new Vector3(300,0,0),0f);
				gameScript.spawnEnemyPosition(new Vector3(0,300,0),0f);
				enemiesSpawned = true;
			}
		}
		
		if(currentIndex == 5){
			cannonScript = gameScript.getCannonTutorialScript();
			cannonScript.rotationSpeed =( 12 / textTime[5]);
		}
		
		if(currentIndex ==6)
			{				
				cannonScript.rotationSpeed =(-12/ textTime[6]);
			}
		
		if(currentIndex>6)
			{
				cannonScript.transform.rotation = Quaternion.identity;
				cannonScript.rotationSpeed = 0;		
			}
		
		if(currentIndex == 7) {
			cannonScript.transform.Translate(0,Time.deltaTime*150f/textTime[7],0);
			cannonScript.setFuel(60 + 10*(int)(elapsedTimeInTable - Time.timeSinceLevelLoad));
		}
		if(currentIndex == 8) {
			cannonScript.transform.Translate(0,Time.deltaTime*-cannonScript.transform.position.y,0);
			cannonScript.setFuel(30 + 10*(int)(elapsedTimeInTable - Time.timeSinceLevelLoad));
		}
		if(currentIndex == 10) {
			if(Time.timeSinceLevelLoad > elapsedTimeInTable - textTime [10]/2 && !bulletSpawned) {
				gameScript.spawnBullet();
				bulletSpawned=true;
				enemiesSpawned = false;
			}	
		}
		
		if(currentIndex == 11 && !enemiesSpawned) {
			
			gameScript.spawnEnemyPosition(new Vector3(270,374,000),0.2f);
			gameScript.spawnEnemyPosition(new Vector3(264,-243,000),0.2f);
			gameScript.spawnEnemyPosition(new Vector3(-240,244,000),0.2f);
			gameScript.spawnEnemyPosition(new Vector3(-200,-221,000),0.2f);	
			gameScript.spawnEnemyPosition(new Vector3(-339,457,000),0.2f);
			gameScript.spawnEnemyPosition(new Vector3(150,150,000),0.2f);
			gameScript.spawnEnemyPosition(new Vector3(0,-297,000),0.2f);
			gameScript.spawnEnemyPosition(new Vector3(200,100,000),0.2f);
			enemiesSpawned = true;
		}
		
		if(currentIndex == 12 && !bombLaunched ) {
			gameScript.fireBomb();
			bombLaunched = true;
			keySprite.enabled = false;
		}
		
		if(currentIndex == tutoText.Count-1) {
			keySprite.enabled = true;
			
			if(Input.GetKeyDown(KeyCode.KeypadEnter)) {
					Application.LoadLevel("game");
			}
		}
		
		keySprite.alpha = 0.7f + Mathf.Sin (Time.frameCount/5);
		
	}
}
