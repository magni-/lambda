using UnityEngine;
using System.Collections;

public class pickupScript : MonoBehaviour {
	
	float frames;
	// Use this for initialization
	void Start () {
		frames = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<OTSprite>().alpha  = Mathf.Log(3000f - Time.time - frames, 3000f);
		
		if(GetComponent<OTSprite>().alpha <0.05) {
			Destroy(this);
		}
	
	}
}
