using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonArmor : MonoBehaviour {
	
	public  List<Texture> armorTextures;
	public AudioClip shieldSound;
	public AudioClip magnetismSound;
	private int currentLevel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void setArmor(int level) {
		if(level != currentLevel) {
			this.GetComponent<OTSprite>().texture = armorTextures[level];
			if(level!=0) {
				this.audio.Play();
			}
			else{
				audio.PlayOneShot(shieldSound);	
			}
			currentLevel = level;
		}
	}
}
