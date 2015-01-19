using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public int scoreValue;
	private GameController gameController;

	//Load the GameController
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	//Used to trigger a score change
	void OnTriggerEnter(Collider other){
		if(gameObject.tag == "Points"){
			if(other.tag == "Player"){
				Destroy(gameObject);
				gameController.AddScore(scoreValue, gameObject);
			}
		}
		else if(gameObject.tag == "Power Up"){
			if(other.tag == "Player"){
				gameController.AddScore(scoreValue, gameObject);
				Destroy(gameObject);
			}
		}
		else if(gameObject.tag == "Bonus"){
			if(other.tag == "Player"){
				gameController.AddScore(scoreValue, gameObject);
				Destroy(gameObject);
			}
		}
	}
}
