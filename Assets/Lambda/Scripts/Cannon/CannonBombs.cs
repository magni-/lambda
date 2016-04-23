using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonBombs : MonoBehaviour {
	
	public  List<Texture> bombTextures;
	public AudioClip bombSound;
	private int nbBombs;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setBombs(int nb) {
		if(nb != nbBombs) {
			this.GetComponent<OTSprite>().texture = bombTextures[nb];
			nbBombs = nb;
		}
	}
}
