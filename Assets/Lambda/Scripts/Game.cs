using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {	
	
	//Prefab settings
	public OTAnimatingSprite cannonPrefab;
	public OTSprite          enemyPrefab;
	public OTAnimatingSprite bulletPrefab;
	public GameObject        gridPrefab;
	public OTSprite starPrefab;
	public OTSprite supernovaPickupPrefab;
	
	//Textures
	public Texture bulletSupernova;
	public Texture bulletFreeze;
	
	//Sounds
	public AudioClip musicSound, fireSound,getBombSound, getFreezeSound,getSupernovaSound, fireSupernovaSound, fireFreezeSound, boundSound, explodeSound;
	public AudioClip Attraction, Beam, Charge, Magnetism, magatak;
	
	//Mode
	public bool isTutorial;

	private List<OTSprite> starLst;
	private List<OTSprite>          enemiesInGame;
	private List<OTAnimatingSprite> bulletsInGame;
	
	private OTSprite supernovaPickup;

	private OTAnimatingSprite cannon;
	private GameObject        grid;


	
	// Added TODO clean up
	public float gravityRadiusBullet = 20;

	//Clean hierarchy
	private GameObject enemies;
	private GameObject bullets;
	private int enemyCounter;
	private int bulletCounter;
	
	public enum bulletType {
		Standard,
		Supernova,
		Freeze
	}
	
	private bulletType currentBullet;
	
	private int bombCount;
	private int score;
	private GUIText scoreText;
	private GUIText shieldText;
	private GUIText fuelText;
	private GUIText bombsText;
	//private bool playing;
	private bool paused;
	//Bomb 
	private bool bombLaunched;
	private int numFrameAtLaunch;
	private bool isLocking;
	
	//Avoid sound saturation (limited play of firesound each 5 frame)
	private int lastExplodeFrame;
	
	// Use this for initialization
	void Start () {
		
		enemiesInGame = new List<OTSprite>();
		bulletsInGame = new List<OTAnimatingSprite>();
		starLst= new List<OTSprite>();
		
		enemies = new GameObject("Enemies");
		bullets = new GameObject("Bullets");
		
		enemyCounter = 0;
		bulletCounter = 0;
		
		currentBullet = bulletType.Standard;
			
		bombCount = 3;
		
		scoreText  = GameObject.Find("scoreGUI").GetComponent<GUIText>();
		shieldText = GameObject.Find("shieldGUI").GetComponent<GUIText>();
		fuelText   = GameObject.Find("fuelGUI").GetComponent<GUIText>();
		bombsText  = GameObject.Find("bombsGUI").GetComponent<GUIText>();
		
		InitializeObjects();
		
		score   = 0;
		//playing = true;
		unsetPause();
		bombLaunched = false;		
	}
	
	public void InitializeObjects() {
		
		cannon = (OTAnimatingSprite) GameObject.Instantiate(cannonPrefab);	
		cannon.name = "cannon";
		cannon.GetComponent<Cannon>().setBombs(3);
		cannon.GetComponent<Cannon>().setFuel(80);

		if(!isTutorial){
			//grid = (GameObject) Object.Instantiate(gridPrefab);
			//grid.name = "grid";	
		}
	}	
	
	// Update is called once per frame
	void Update () {
		
		//if (playing) {
		if (!paused) {
			scoreText.text  = "Score: " + score;
			shieldText.text = "Shield: " + cannon.GetComponent<Cannon>().getShield();
			if (cannon.GetComponent<Cannon>().getShield() == 100) {
				shieldText.color = Color.green;
			} else if (cannon.GetComponent<Cannon>().getShield() == 0) {
				shieldText.color = Color.red;
			} else {
				shieldText.color = Color.white;
			}
			fuelText.text   = "Fuel: " + cannon.GetComponent<Cannon>().getFuel();
			if (cannon.GetComponent<Cannon>().getFuel() == 60) {
				fuelText.color = Color.green;
			} else if (cannon.GetComponent<Cannon>().getFuel() == 0) {
				fuelText.color = Color.red;
			} else {
				fuelText.color = Color.white;
			}
			bombsText.text  = "Bombs: " + bombCount;
			if (bombCount == 3) {
				bombsText.color = Color.green;
			} else if (bombCount == 0) {
				bombsText.color = Color.red;
			} else {
				bombsText.color = Color.white;
			}
			
			if(Time.realtimeSinceStartup == 162) 
			{
				foreach(OTSprite enemyy in enemiesInGame )
				{
					enemyy.GetComponent<Enemy>().setMaxSpeed(2);
				}
			}
			if(Time.realtimeSinceStartup == 202) {
				foreach(OTSprite enemyy in enemiesInGame )
				{
					enemyy.GetComponent<Enemy>().setMaxSpeed(5);
				}	
			}
			/*} else {
				// game over here	
			}
			*/
			if(bombLaunched) {
			//	this.GetComponent<LevelManager>().enabled=false;
				if(Time.frameCount - numFrameAtLaunch <40) {
					if(!isLocking) {
						foreach(OTSprite enemy in enemiesInGame){
							enemy.GetComponent<Enemy>().setGravity(0.00001f);
							enemy.GetComponent<Enemy>().setMaxSpeed(0.01f);
							}
						isLocking=true;
					}
				cannon.GetComponent<Cannon>().setArmor((int)(Time.frameCount-numFrameAtLaunch)/5+1);
			
				}
				if(Time.frameCount-numFrameAtLaunch == 40) {
					this.audio.PlayOneShot(Charge);	
				}
				if(Time.frameCount-numFrameAtLaunch>40 && Time.frameCount-numFrameAtLaunch<80){
						cannon.transform.Translate(Mathf.Sin(Time.frameCount*20),0,0);
				
				}
				
				if(Time.frameCount-numFrameAtLaunch>80) {
						if(enemiesInGame.Count==0) {
						if(!isTutorial)
							this.GetComponent<LevelManager>().enabled=true;
							bombLaunched = false;
							isLocking = false;
							cannon.GetComponent<Cannon>().setArmor(0);
						}
						for(int i=0;i<enemiesInGame.Count;++i) {
						destroyEnemy(enemiesInGame[i],true);	
					}				
				}
			}
		}
	}

	


	public void spawnBullet() {
		// TODO bullet sound
		OTAnimatingSprite bullet;
		bullet = (OTAnimatingSprite) GameObject.Instantiate(bulletPrefab);
		bullet.transform.parent = bullets.transform;
		bulletsInGame.Add(bullet);
		bullet.name = "bullet-" + ++bulletCounter;

		if (currentBullet == bulletType.Supernova) {
			bullet.image = bulletSupernova;
			bullet.size.Set(16, 16);
			bullet.tag = "bulletSupernova";
			audio.PlayOneShot(fireSupernovaSound);
		} else if (currentBullet == bulletType.Freeze) {
			bullet.image = bulletFreeze;
			bullet.size.Set(16, 16);
			bullet.tag = "bulletFreeze";
			audio.PlayOneShot(fireFreezeSound);
		} else{
			audio.PlayOneShot(fireSound);	
		}
		
		currentBullet = bulletType.Standard;
		cannon.GetComponent<Cannon>().setSpecialBullet(0);
	}
	
	public void fireBomb() {
		// TODO bomb animation
		// TODO bomb sound
		
		//ADDED
		bombLaunched=true;
		numFrameAtLaunch=Time.frameCount;
		this.audio.clip=Attraction;
		this.audio.Play();		
	}
	
	public void augmentShield() {
		cannon.GetComponent<Cannon>().augmentShield();
	}
	
	public void augmentFuel() {
		cannon.GetComponent<Cannon>().augmentFuel();
	}
	
	public void spawnEnemy(int spawnRadius) {
		// TODO enemy spawn sound
		OTSprite enemy;
		enemy = (OTSprite) Object.Instantiate(enemyPrefab);
		enemy.transform.parent = enemies.transform;
		enemy.transform.position = new Vector3(
			(Random.Range(1, 3) >= 2 ? -1 : 1) * Random.Range(spawnRadius, OT.view._pixelPerfectResolution.x/2), 
			(Random.Range(1, 3) >= 2 ? -1 : 1) * Random.Range(spawnRadius, OT.view._pixelPerfectResolution.y/2),
			0);
		enemy.name = "enemy" + ++enemyCounter;
		enemy.tag  = "enemy-b";
		Debug.Log(OT.view.pixelPerfectResolution);
		
		// add powerup
		
		OTSprite medium;
		OTSprite small;
		for (int i = 0; i < enemy.transform.childCount; ++i) {
			medium = enemy.transform.GetChild(i).GetComponent<OTSprite>();
			medium.tag = "enemy-m";
			for (int j = 0; j < medium.transform.childCount; ++j) {
				small = medium.transform.GetChild(j).GetComponent<OTSprite>();
				switch (Random.Range(0, 50)) {//Random.Range(0, 50)) {
				case 0: // supernova
					small.tintColor = Color.red;
					small.tag = "enemySupernova";
					break;
				case 1: // freeze
					small.tintColor = Color.cyan;
					small.tag = "enemyFreeze";
					break;
				case 2: // shield
					if (cannon.GetComponent<Cannon>().getShield() < 100) {
						small.tintColor = Color.green;
						small.tag = "enemyShield";
					}
					break;
				case 3: // fuel
					if (cannon.GetComponent<Cannon>().getFuel() < 60) {
						small.tintColor = Color.grey;
						small.tag = "enemyFuel";
					}
					break;
				case 4: // bomb
					if (bombCount < 3) {
						small.tintColor = Color.blue;
						small.tag = "enemyBomb";
					}
					break;
				default:
					small.tag = "enemy-s";
					break;
				}
			}
		}
		
		enemiesInGame.Add(enemy);
	}
	
	public void destroyEnemy(OTSprite enemy, bool cascade) {
		for (int i = 0; i < enemy.transform.childCount; ++i) {
			if (cascade) {
				destroyEnemy(enemy.transform.GetChild(i).GetComponent<OTSprite>(), true); 
			} else {
				enemy.transform.GetChild(i).GetComponent<Enemy>().enabled = true;
				enemy.transform.GetChild(i).GetComponent<Enemy>().speed = enemy.transform.GetComponent<Enemy>().speed;
				enemy.transform.GetChild(i).GetComponent<Enemy>().gravityWeight = enemy.transform.GetComponent<Enemy>().gravityWeight;
				enemiesInGame.Add(enemy.transform.GetChild(i).GetComponent<OTSprite>());
			}
		}
		
		enemy.transform.DetachChildren();
		
		if (enemy.tag == "enemy-b") {
			++score;
		} else if (enemy.tag == "enemy-m") {
			score += 3;
		} else {
			score += 5;
		}
		if(Time.frameCount-lastExplodeFrame>5) {
			audio.PlayOneShot(explodeSound);
			lastExplodeFrame = Time.frameCount;
		}
		if(starLst.Count<60 && enemy.renderer.isVisible) {
				starLst.Add ((OTSprite) Object.Instantiate(starPrefab));
				starLst[starLst.Count-1].GetComponent<StarScript>().setCenter(enemy.transform.position);
				starLst[starLst.Count-1].transform.position=enemy.transform.position;
				starLst[starLst.Count-1].GetComponent<OTSprite>().tintColor = enemy.GetComponent<OTSprite>().tintColor;
		}
		enemiesInGame.Remove(enemy);
		Destroy(enemy.gameObject);	
	}

	public void gameOver() {
		//playing = false;
		Debug.Log ("You Lose !");	
		Destroy(cannon);
		Application.LoadLevel("Game Over");
	}
	
	public int getBombCount() {
		return bombCount;	
	}
	
	public void incBombCount() {
		cannon.GetComponent<Cannon>().setBombs(++bombCount);
		audio.PlayOneShot(getBombSound);
	}
	
	public void decBombCount() {
		cannon.GetComponent<Cannon>().setBombs(--bombCount);
		
	}
	
	// Added TODO clean up
	public List<Vector3> getNearEnemies(Vector3 position) {	
		List<Vector3> nearEnemies = new List<Vector3>();
		foreach(OTSprite sp in enemiesInGame) {
			if ((sp.transform.position-position).magnitude < gravityRadiusBullet) {
				nearEnemies.Add(sp.transform.position);
			}
		}	
		return nearEnemies;
	}
	
	// Added TODO clean up
	public Vector3 getCannonPosition() {
		return cannon.transform.position;
	}
	
	public void destroyStar(OTSprite starToDestroy) {
		starLst.Remove(starToDestroy);	
		Destroy(starToDestroy);
	}
	
	public void setBulletType(bulletType type) {
		if(type==bulletType.Supernova) {
			currentBullet = bulletType.Supernova;
			cannon.GetComponent<Cannon>().setSpecialBullet(1);
			audio.PlayOneShot(getSupernovaSound);
		}
		if(type==bulletType.Freeze) {
			currentBullet = bulletType.Freeze;
			cannon.GetComponent<Cannon>().setSpecialBullet(2);
			audio.PlayOneShot(getFreezeSound);
		}
	}
	
	public CannonTutorial getCannonTutorialScript() {
		return cannon.GetComponent<CannonTutorial>();	
	}
	
	public void spawnEnemyPosition(Vector3 pos, float speed) {
		OTSprite enemy;
		enemy = (OTSprite) Object.Instantiate(enemyPrefab);
		enemy.position = pos;
		enemy.GetComponent<Enemy>().speed = speed;
		enemiesInGame.Add(enemy);
	}
	
 	public void displayPickup(Vector3 position, int type) {
	
		supernovaPickup = (OTSprite) Object.Instantiate(supernovaPickupPrefab);
		supernovaPickup.position = new Vector2(position.x, position.y);
	}

	public void setPause() {
		paused = true;
		Time.timeScale = 0.0f;
		cannon.collider.enabled = false;
		bulletPrefab.GetComponent<Bullet>().enabled = false;
		//enemyPrefab.GetComponent<Enemy>().enabled = false;
		//cannonPrefab.GetComponent<Cannon>().enabled = false;
		this.GetComponent<LevelManager>().enabled = false;
	}

	public void unsetPause() {
		paused = false;
		Time.timeScale = 1.0f;
		cannon.collider.enabled = true;
		bulletPrefab.GetComponent<Bullet>().enabled = true;
		//enemyPrefab.GetComponent<Enemy>().enabled = true;
		cannonPrefab.GetComponent<Cannon>().enabled = true;
		this.GetComponent<LevelManager>().enabled = true;
	}

	public bool GetPause() {
		return paused;
	}
}
