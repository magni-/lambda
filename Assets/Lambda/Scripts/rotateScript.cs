using UnityEngine;
using System.Collections;

public class rotateScript : MonoBehaviour {
	
	public float angle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(float.IsNaN(angle)) 
			angle=0;
			this.transform.Rotate(0,0,angle);
	}
}
