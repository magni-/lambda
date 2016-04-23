using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

	//Prefab settings
	public OTSprite mainMenuPrefab;
	public OTSprite titlePrefab;
	public OTSprite resumePrefab;

	public AudioClip selectSound;

	private OTSprite mainMenu;
	private OTSprite title;
	private OTSprite resume;
	private Game gameScript;
	private bool paused;
	private int counter;
	private OTSprite selected;
	private bool isFirst;
	private bool mouse;
	private PauseMenuEffects menuScript;

	// Use this for initialization
	void Start () {
		paused = false;
		counter = 0;
		mouse = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			gameScript = GameObject.Find("game").GetComponent<Game>();
			if (!paused) {
				paused = true;
				isFirst = true;
				gameScript.setPause();
				title = (OTSprite) Object.Instantiate(titlePrefab);
				title.position = new Vector2(0f, 300f);
				resume = (OTSprite) Object.Instantiate(resumePrefab);
				resume.position = new Vector2(0f, 0f);
				mainMenu = (OTSprite) Object.Instantiate(mainMenuPrefab);
				mainMenu.position = new Vector2(0f, -250f);
			} else {
				resumeFunc();
			}
		}

		if (paused) {
			if (Input.GetKeyDown("down")){
				if (!isFirst) {
					menuScript.setRowNumber(0);
					selected.GetComponent<OTSprite>().size = new Vector2(400, 150);
					updateCounter();
				} else
					isFirst = false;
				
				select ();
			}
			
			if (Input.GetKeyDown("up")){
				if (!isFirst) {
					menuScript.setRowNumber(0);
					selected.GetComponent<OTSprite>().size = new Vector2(400, 150);
					updateCounter();
				} else
					isFirst = false;
				
				select ();
			}
			
			if (Input.GetKeyDown(KeyCode.Return)) {
				if (!isFirst) {
					switch (counter) {
					case 0:
						resumeFunc();
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
		//if (Input.GetKey(KeyCode.M))
			//Couper le son : Creer une fonction et l'appeler.

	}

	void updateCounter() {
		if (counter == 1)
			counter = 0;
		else 
			counter = 1;
	}

	void setSelected(int i) {
		switch (i) {
		case 0:
			selected = resume;					
			break;
		case 1:
			selected = mainMenu;
			break;
		default:
			Debug.Log ("problem");
			break;
		}
	}

	void select() {
		resetMouse();
		setSelected(counter);
		menuScript = selected.GetComponent<PauseMenuEffects>();
		menuScript.setRowNumber(1);
		selected.GetComponent<OTSprite>().size	= new Vector2(500, 250);
		audio.PlayOneShot(selectSound);
	}

	public void reset() {//si on sélectionne avec la souris
		if (!isFirst) {
			isFirst = true;
			counter = 0;
			menuScript.setRowNumber(0);
			selected.GetComponent<OTSprite>().size = new Vector2(400, 150);
		}
	}

	void resetMouse() {
		if (mouse) { 
			for (int i = 0; i < 3 ; i++) {
				setSelected(i);
				menuScript = selected.GetComponent<PauseMenuEffects>();
				menuScript.setRowNumber(0);
				selected.GetComponent<OTSprite>().size = new Vector2(400, 150);
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

	public void resumeFunc () {
		paused = false;
		gameScript.unsetPause();
		Destroy (title);
		Destroy (resume);
		Destroy (mainMenu);
	}
}
