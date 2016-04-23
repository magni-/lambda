using UnityEngine;
using System.Collections;

public class WelcomeMenuEffects : MonoBehaviour {

	public AudioClip selectSound;
	private WelcomeMenuManager menuManagerScript;
	private Vector2 size;
	//vars for the whole sheet
	public int colCount =  1;
	public int rowCount =  2;
		
	//vars for animation
	public int  rowNumber  =  0; //Zero Indexed
	public int colNumber = 0; //Zero Indexed
	public int totalCells = 1;
	public int  fps     = 1;
	//Maybe this should be a private var
	private Vector2 offset;

	void Start () {
		menuManagerScript = GameObject.Find("Main Camera").GetComponent<WelcomeMenuManager>();
	}

	//Update
	void Update () { 
		SetSpriteAnimation(colCount,rowCount,rowNumber,colNumber,totalCells,fps);  
	}
		
	//SetSpriteAnimation
	void SetSpriteAnimation(int colCount ,int rowCount ,int rowNumber ,int colNumber,int totalCells,int fps ){
			
		// Calculate index
		int index  = (int)(Time.time * fps);
		// Repeat when exhausting all cells
		index = index % totalCells;
			
		// Size of every cell
		float sizeX = 1.0f / colCount;
		float sizeY = 1.0f / rowCount;
		Vector2 size =  new Vector2(sizeX,sizeY);
			
		// split into horizontal and vertical index
		var uIndex = index % colCount;
		var vIndex = index / colCount;
			
		// build offset
		// v coordinate is the bottom of the image in opengl so we need to invert.
		float offsetX = (uIndex+colNumber) * size.x;
		float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
		Vector2 offset = new Vector2(offsetX,offsetY);
			
		renderer.material.SetTextureOffset ("_MainTex", offset);
		renderer.material.SetTextureScale  ("_MainTex", size);
	}

	public void setRowNumber(int x) {
		if (x == 0 || x == 1)
			rowNumber = x;	
	}


	void OnMouseEnter(){
		menuManagerScript.setMouse(true);
		menuManagerScript.reset();
		rowNumber = 1;
		size = this.GetComponent<OTSprite>().size;
		this.GetComponent<OTSprite>().size	= size + new Vector2(100, 100)	;
		audio.PlayOneShot(selectSound);
	}

	void OnMouseUp () {
		if (this.tag == "play") {
			Application.LoadLevel("game");
		} else if (this.tag == "options") {
			Application.LoadLevel ("Options Menu");
		} 
		else if (this.tag == "exit") {
			Application.Quit ();
		}
	}

	void OnMouseExit() {
		if (menuManagerScript.getMouse()) { //si la souris n'a pas été resetée
			menuManagerScript.setMouse(false);
			rowNumber = 0;	
			this.GetComponent<OTSprite>().size = size;
		}

	}
	

}
