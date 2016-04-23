using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	

	public  float   speed;
	public  float   gravityWeight;
	public  float   maxSpeed;
	private Game    gameScript;
	private Vector3 direction;
	private float   supernovaSize;
	public AudioClip explodeSound;
	public OTSprite star;
	private List<OTSprite> starLst;
	
	// Use this for initialization
	void Start () {
		maxSpeed = 1;
		if (tag == "enemy-b") {
			gravityWeight = 0.0001f;
		}
		
		gameScript = GameObject.Find("game").GetComponent<Game>();
		supernovaSize = 250; //TODO calibrate
			
		//Random direction
		float angle = Random.Range(0,360);
		direction = Quaternion.AngleAxis(angle,Vector3.back)* Vector3.up;
		GetComponent<OTSprite>().onEnter = onEnterCollision;
		starLst=new List<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!gameScript.GetPause()) {
			//this.GetComponent<OTSprite>().size += new Vector2(Mathf.Cos (Time.frameCount/10)*0.5f,Mathf.Sin (Time.frameCount/10)*0.5f);
			
			// Move
			speed = Mathf.Min(speed, maxSpeed);
			direction.Normalize();
			transform.position += direction*speed;//*Time.deltaTime;
			transform.Rotate(0,0,0.5f);
			applyGravity();
			
			if (gameScript.getBombCount() >= 3 && tag == "enemyBomb") {
				tag = "enemy-s";
				gameObject.GetComponent<OTSprite>().tintColor = gameScript.enemyPrefab.transform.GetChild(0).transform.GetChild(0).GetComponent<OTSprite>().tintColor;
			}

			//	starLst.Add ((OTSprite)Object.Instantiate(star));
			//	starLst[starLst.Count-1].position=this.GetComponent<OTSprite>().position;
		
			foreach(OTSprite currstar in starLst){
				currstar.alpha*=0.90f;	
			}
		}
	}
	
	
	void onEnterCollision(OTObject owner) {
		if(owner.tag.Contains("bullet")) {
			if (tag == "enemySupernova") {
				gameScript.displayPickup(transform.position, 0);
				gameScript.setBulletType(Game.bulletType.Supernova);
			} else if (tag == "enemyFreeze") {
				gameScript.setBulletType(Game.bulletType.Freeze);
			} else if (tag == "enemyShield") {
				gameScript.augmentShield();
			} else if (tag == "enemyFuel") {
				gameScript.augmentFuel();
			} else if (tag == "enemyBomb") {
				gameScript.incBombCount();	
			}
			if (owner.tag == "bulletSupernova") {
				// TODO supernova animation
				// TODO supernova sound
				Collider[] objs = Physics.OverlapSphere(transform.position, supernovaSize);
				foreach (Collider obj in objs) {
					if (obj.tag != "player" && obj != this) {
						gameScript.destroyEnemy(obj.gameObject.GetComponent<OTSprite>(), true);
					}
				}
				// TODO remove bullet • see ricochet
			} else if (owner.tag == "bulletFreeze") {
				// TODO freeze animation
				// TODO freeze sound
				gravityWeight = 0;
				speed         = 0;
				// TODO remove bullet • see ricochet
			}
			gameScript.destroyEnemy(gameObject.GetComponent<OTSprite>(), false);
		}
	}
	
	void applyGravity() {
		if (gravityWeight != 0) {
			float gravityMag = 1 / Mathf.Sqrt(2 * Mathf.PI)*Mathf.Exp(-1 / 2 * Mathf.Pow((gameScript.getCannonPosition() - transform.position).magnitude / 8, 2));
			//Ecrasement de la courbe
			gravityMag *= gravityWeight;
			
			Vector3 gravity = gameScript.getCannonPosition() - transform.position;
			gravity *= gravityMag;
			direction += gravity;
			
			speed = direction.magnitude;
			direction.Normalize();	
		}
	}

	public void setGravity(float weight) {
		gravityWeight = weight;
	}
	public void setMaxSpeed(float speed) {
		this.maxSpeed = speed;	
		this.speed=speed;
	}
	

}
