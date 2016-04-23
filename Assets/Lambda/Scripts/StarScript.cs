using UnityEngine;
using System.Collections;

public class StarScript : MonoBehaviour {
	
	public float speed;
	public float angularSpeed;
	private Vector3 direction;
	private Vector3 center;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	//	direction=Quaternion.AngleAxis(angularSpeed,Vector3.forward)*direction;
		direction.Normalize();
		speed*=1/this.GetComponent<OTSprite>().size.x;
		transform.position += direction *speed;
		this.GetComponent<OTSprite>().size *=1.02f;
		//this.transform.Rotate(0,0,10);
		//transform.RotateAround(center,Vector3.forward,angularSpeed);
		this.GetComponent<OTSprite>().alpha*=0.95f;
		if(this.GetComponent<OTSprite>().alpha < 0.05f)
			GameObject.Find ("game").GetComponent<Game>().destroyStar(this.GetComponent<OTSprite>());
	}
	
	public void setDirection(Vector3 _direction) {
		direction=_direction;
	}
	public void setCenter(Vector3 position) {
		center=position;	
	}
}
