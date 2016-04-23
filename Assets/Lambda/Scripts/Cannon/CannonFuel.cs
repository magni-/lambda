using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonFuel : MonoBehaviour {
	
	public  List<Texture> fuelTextures;
	public AudioClip fuelSound;
	private int nbFuel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setFuel(int nb) {
		if(nb != nbFuel) {
			this.GetComponent<OTSprite>().texture = fuelTextures[nb/10-1];
			nbFuel = nb;
		}
	}
}
