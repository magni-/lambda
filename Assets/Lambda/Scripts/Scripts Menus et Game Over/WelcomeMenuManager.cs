using UnityEngine;
using System.Collections;

public class WelcomeMenuManager : MonoBehaviour {

	public AudioClip selectSound;

	private WelcomeMenuEffects menuScript;
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
				if (counter != 3)
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
					counter = 3;
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
					//Application.LoadLevel("highsocres");
					break;
				case 2:
					Application.LoadLevel("Options Menu");
					break;
				case 3:
					Application.Quit();
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
			selected = GameObject.Find("Play");					
			break;
		case 1:
			selected = GameObject.Find("HighScores");
			break;
		case 2:
			selected = GameObject.Find("Options");
			break;
		case 3:
			selected = GameObject.Find("Exit");
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
		menuScript = selected.GetComponent<WelcomeMenuEffects>();
		menuScript.setRowNumber(1);
		selected.GetComponent<OTSprite>().size	= size + new Vector2 (100, 100);
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
				menuScript = selected.GetComponent<WelcomeMenuEffects>();
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