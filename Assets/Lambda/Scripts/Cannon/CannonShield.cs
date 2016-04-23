using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonShield : MonoBehaviour {
	
	public  List<Texture> shieldTextures;
	public AudioClip shieldSound;
	private int nbShield;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setShield(int nb) {
		if(nb != nbShield) {
			this.GetComponent<OTSprite>().texture = shieldTextures[nb/10-1];
			nbShield = nb;
		}
	}
}
