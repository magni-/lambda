using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonBullet : MonoBehaviour {
	
	public  List<Texture> bulletTexture;
	private int currentBulletType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(currentBulletType == 1) {
			this.GetComponent<OTSprite>().tintColor = new Color(1,0.5f+Mathf.Sin (Time.frameCount/2),0.5f+Mathf.Sin (Time.frameCount/2));//0.5f+Mathf.Sin (Time.frameCount/2),1f,1f);	
		}
		if(currentBulletType == 2) {
			this.GetComponent<OTSprite>().tintColor = new Color(0.1f+Mathf.Sin (Time.frameCount/2)/2,0.9f+Mathf.Sin (Time.frameCount/2)/2,1f);
		}
		if(currentBulletType == 0) {
			this.GetComponent<OTSprite>().tintColor = new Color(1,1,1);	
		}
	
	}
	
	public void setBullet(int bulletType) {
			this.GetComponent<OTSprite>().texture = bulletTexture[bulletType];
			currentBulletType=bulletType;
	}
}
