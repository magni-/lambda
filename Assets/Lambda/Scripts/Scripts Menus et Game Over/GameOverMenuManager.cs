using UnityEngine;
using System.Collections;

public class GameOverMenuManager : MonoBehaviour {
	
	public AudioClip selectSound;
	
	private GameOverMenuEffects menuScript;
	private int counter;
	private GameObject selected;
	private bool isFirst;
	private bool mouse;
	private Vector2 size;
	
	// Use this for initialization
	void Start () {
		counter = 0;
		isFirst = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("down")){
			if (!isFirst) {
				menuScript.setRowNumber(0);
				selected.GetComponent<OTSprite>().size = size;
				if (counter != 1)
					counter++;
				else
					counter = 0;
			} else
				isFirst = false;
			
			select ();
		}
		
		if (Input.GetKeyDown("up")){
			if (!isFirst) {
				menuScript.setRowNumber(0);
				selected.GetComponent<OTSprite>().size = size;
				if (counter != 0)
					counter--;
				else
					counter = 1;
			} else
				isFirst = false;
			
			select ();
		}
		
		if (Input.GetKeyDown(KeyCode.Return)) {
			if (!isFirst) {
				switch (counter) {
				case 0:
					Application.LoadLevel("game");	
					break;
				case 1:
					Application.LoadLevel("Welcome Menu");
					break;
				default:
					break;
				}
			}
		}
	}
	
	void setSelected(int i) {
		switch (i) {
		case 0:
			selected = GameObject.Find("Try Again");					
			break;
		case 1:
			selected = GameObject.Find("Main Menu");
			break;
		default:
			Debug.Log ("problem");
			break;
		}
		size = selected.GetComponent<OTSprite>().size;
	}
	
	void select() {
		resetMouse();
		setSelected(counter);
		menuScript = selected.GetComponent<GameOverMenuEffects>();
		menuScript.setRowNumber(1);
		selected.GetComponent<OTSprite>().size	= size + new Vector2(100, 100);;
		audio.PlayOneShot(selectSound);
	}
	
	public void reset() {//si on sélectionne avec la souris
		if (!isFirst) {
			isFirst = true;
			counter = 0;
			menuScript.setRowNumber(0);
			selected.GetComponent<OTSprite>().size = size;
		}
	}
	
	void resetMouse() {
		if (mouse) { 
			for (int i = 0; i < 3 ; i++) {
				setSelected(i);
				menuScript = selected.GetComponent<GameOverMenuEffects>();
				menuScript.setRowNumber(0);
				selected.GetComponent<OTSprite>().size = size;
			}
			mouse = false;
		}
	}
	
	public void setMouse(bool b) {
		mouse = b;
	}
	
	public bool getMouse() {
		return mouse;
	}
}