using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {

	public bool isQuit = false;
	
	void OnMouseEnter(){
		//change text color
		gameObject.guiText.color = Color.red;
	}
	
	void OnMouseExit(){
		//change text color
		gameObject.guiText.color = Color.white;
	}
	
	void OnMouseUp(){
		//is this quit
		if (isQuit == true) {
			//quit the game
			Application.Quit();
		}
		else {
			//load level
			Application.LoadLevel("Level1");
		}
	}
	
	void Update(){
		//quit game if escape key is pressed
		if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit();
		}
	}
}
