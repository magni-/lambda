using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
	
	public  float rotationSpeed;
	public  float maxRotationSpeed;
	public OTSprite armorSprite;
	public OTSprite bombSprite;
	public OTSprite shieldSprite;
	public OTSprite fuelSprite;
	public OTSprite specialBulletSprite;

	private Game  gameScript;
	private int   shield = 100;
	private int   fuel = 0;
	private float speed = 8;

	private float width;
	private float height;
	
	private int currentFrameCount;

	// Use this for initialization
	void Start () {
		rotationSpeed    = 0;
		maxRotationSpeed = 15;
		transform.rotation = Quaternion.identity;
		gameScript = GameObject.Find ("game").GetComponent<Game>();
		GetComponent<OTAnimatingSprite>().onEnter = OnEnterCollision;
		
		width  = gameObject.GetComponent<OTAnimatingSprite>().worldSize.x;
		height = gameObject.GetComponent<OTAnimatingSprite>().worldSize.y;
		
		//ADDED
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameScript.GetPause()) {
			// Left Rotation
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				currentFrameCount = Time.frameCount;
			}
			if(Input.GetKey(KeyCode.LeftArrow)) {
				rotationSpeed = Mathf.Min(Mathf.Max(Mathf.Sqrt((Time.frameCount - currentFrameCount) / 5) * 5, 2), maxRotationSpeed);
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow)) {
				rotationSpeed = 0;
			}
			
			// Right Rotation
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				currentFrameCount = Time.frameCount;
			}
			if(Input.GetKey(KeyCode.RightArrow)) {
				rotationSpeed =- Mathf.Min(Mathf.Max(Mathf.Sqrt((Time.frameCount - currentFrameCount) / 5) * 5, 2), maxRotationSpeed);
			}
			if (Input.GetKeyUp(KeyCode.RightArrow)) {
				rotationSpeed = 0;
			}
			if(Input.GetKey (KeyCode.UpArrow) && fuel > 0 ){
				this.transform.Translate(0,5,0);	
				if (Time.frameCount % 10 == 0) {
				setFuel(--fuel);
			}
			}
			if(Input.GetKey (KeyCode.DownArrow) && fuel > 0 ){
				this.transform.Translate(0,-5,0);	
				if (Time.frameCount % 10 == 0) {
				setFuel(--fuel);
			}
			}
			
			// Fire
			if (Input.GetKeyDown(KeyCode.Space)) {
				gameScript.spawnBullet();
				GetComponent<OTAnimatingSprite>().PlayOnce("fire");
			} /*else if (Input.GetKey(KeyCode.Space)) {
				if (Time.frameCount % 10 == 0) {
					fire();
				}
			}*/
			if (Input.GetKeyDown(KeyCode.LeftShift) && gameScript.getBombCount() > 0) {
				gameScript.fireBomb();
			}
			transform.Rotate(0, 0, rotationSpeed / 2);
			
				if(shield < 0) {
					gameScript.gameOver();	
				}
		}
	}
	
	private void fire() {
		gameScript.spawnBullet();
	}	
	
	public void dealDamages(string tag) {
			// TODO damage sound
			if (tag == "enemy-b") {
			shield -= 40;
			} else if (tag == "enemy-m") {
				shield -= 10;
			} else {
				shield -= 3;
			}
			setShield(shield);

	}
	
	public void OnEnterCollision(OTObject target) {
		if (target.tag.Contains("enemy")) {
			dealDamages(target.tag);
			gameScript.destroyEnemy(target.gameObject.GetComponent<OTSprite>(), true);
		}
	}
	
	public int getShield() {
		return shield;	
	}
	
	public void augmentShield() {
		// TODO augment shield sound
		shield = Mathf.Min(shield + 10, 100);
	}
	
	public int getFuel() {
		return fuel;	
	}
	
	public void augmentFuel() {
		// TODO augment fuel sound
		setFuel((int)Mathf.Min(fuel + 10, 60));
	}
				
	public void move(float x, float y) {
		transform.position = new Vector3(
			bounded(OT.view.worldRect.xMin + (width/2), transform.position.x + (speed * x), OT.view.worldRect.xMax - (width/2)),
			bounded(OT.view.worldRect.yMin + (height/2), transform.position.y + (speed * y), OT.view.worldRect.yMax - (height/2)),
			0
		);
		if (Time.frameCount % 10 == 0) {
			setFuel(--fuel);
		}
	}
	
	public float bounded(float min, float val, float max) {
		return Mathf.Max(Mathf.Min(val, max), min);	
	}
	
	public void setArmor(int level) {
		armorSprite.GetComponent<CannonArmor>().setArmor(level);
	}	
	
	public void setBombs(int nb) {
		//bombSprite.GetComponent<CannonBombs>().setBombs(nb);
	}	
	
	public void setShield(int shield) {
		shieldSprite.GetComponent<CannonShield>().setShield(shield);
	}
	
	public void setFuel(int sfuel) {
		fuel = sfuel;
		fuelSprite.GetComponent<CannonFuel>().setFuel(sfuel);	
	}
	
	public void setSpecialBullet(int bulletType) {
		specialBulletSprite.GetComponent<CannonBullet>().setBullet(bulletType);	
	}
	
}	
