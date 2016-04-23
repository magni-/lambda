using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

	public  float    speed;
	public  float    maxSpeed;
	public  float    gravityWeight;
	private Vector3  direction;
	private OTSprite bulletSprite;
	private Game     gameScript;
	
	// Use this for initialization
	void Start () {
		speed         = 20;
		maxSpeed      = 20;
		gravityWeight = 0.1f;
		
		bulletSprite = GetComponent<OTSprite>();
		bulletSprite.onEnter = onEnterCollision;
		
		transform.position = GameObject.Find("cannon").transform.position;
		direction = GameObject.Find("cannon").transform.rotation * Vector3.up;
		
		gameScript = GameObject.Find("game").GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update () {
		speed = Mathf.Min(speed, maxSpeed);
		direction.Normalize();
		transform.position += direction * speed;
		if (!renderer.isVisible) {
			Destroy(gameObject);	
		}
		// applyGravity();
	}

	private void applyGravity(){

		List<Vector3> nearEnemies = gameScript.getNearEnemies(transform.position);
		Vector3 sumVector = new Vector3();
		
		for(int i = 0; i < nearEnemies.Count ; ++i) {
			sumVector += nearEnemies[i] - transform.position;
		}
		
		sumVector *= gravityWeight;
		direction += sumVector;
		speed = direction.magnitude;
		
		direction.Normalize();
	}
	
	public void onEnterCollision(OTObject owner) {
		if(owner.tag.Contains("enemy")) {
			//Rebond
			Vector2 tangent = bulletSprite.position-owner.position;
			direction.Normalize();
			direction *= -1;
			Vector2 direct = new Vector2(direction.x, direction.y);
			direct.Normalize();
			tangent.Normalize();
			
			float angle = -2 * Mathf.Sign(Vector2.Dot(direct, new Vector2(-tangent.y, tangent.x))) * Mathf.Acos(Vector2.Dot(direct, tangent));
	
			//Perform Rotation
			direction = new Vector3(
				direct.x * Mathf.Cos(angle) - direct.y * Mathf.Sin(angle),
				direct.x * Mathf.Sin (angle) + direct.y * Mathf.Cos (angle) );
		}
	
	}
	
	public void SetDirection(Vector3 direction) {
		this.direction = direction;	
	}
}
