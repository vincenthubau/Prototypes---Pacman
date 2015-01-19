using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public bool poweredUp = false;
	private GameController gameController;
	private Vector3 spawnPosition = new Vector3( 0, 0, -2 );
	public float startWait;
	public float powerUpTime;
	private float currentPowerUpTime;

	public float moveDist = 0.1f;

	//Load the GameController
	void Start ()
	{
		currentPowerUpTime = powerUpTime;
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

	//Get the keys and change the player movement
	void Update(){
		/*
		Vector3 pos = transform.position;
		pos.x =  Mathf.Clamp(transform.position.x, -0.5f, 0.5f);
		transform.position = pos;
		*/

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(Vector3.left * speed);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(Vector3.right * speed);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(0,0,1 * speed);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(0, 0, -1 * speed);
		}

		if(poweredUp){
			currentPowerUpTime -= Time.deltaTime;
			if(currentPowerUpTime <= 0){
				gameController.StopSiren();
				poweredUp = false;
				currentPowerUpTime = powerUpTime;
			}
		}
	
		//Need to see how to clamp the player to 0 or .5 values to get him on the middle of each passage (less collisions)

		//Limiter les rattachements à des zones par .5 pour éviter les accrochages avec les murs
		//Verifier la possibilité de tourner avec un capsuleCast ?
	}

	//Used to teleport the player on the side "doors" and activate the power-up state
	void OnTriggerEnter(Collider other){
		if(other.tag == "Teleport"){
			//Simply get the player on the other side of the map
			if(transform.position.x<0){
				transform.position = new Vector3(-(transform.position.x + 0.5f), transform.position.y, transform.position.z);
			}
			else{
				transform.position = new Vector3(-(transform.position.x - 0.5f), transform.position.y, transform.position.z);
			}
		}
		else if(other.tag == "Power Up"){
			poweredUp = true;
			StartCoroutine(PowerUpTime());
			gameController.LaunchSiren();
			//A régler pour que la Siren reste en place meme avec d'autres sons
		}
	}

	IEnumerator PowerUpTime(){
		yield return new WaitForSeconds (powerUpTime);
	}

	//Wait for the player to be respawned
	IEnumerator Respawn(){
		rigidbody.velocity = Vector3.zero;
		transform.position = spawnPosition;
		yield return new WaitForSeconds (startWait);
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Enemy") {
			if(!poweredUp){
				gameController.LoseLife();
				if(gameController.lifeNumber > 0){
					StartCoroutine(Respawn());
				}
				else{
					Destroy (gameObject);
					gameController.GameOver();
				}
			}
		}
	}

}
