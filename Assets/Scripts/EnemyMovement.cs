using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	Transform target;
	private GameController gameController;
	NavMeshAgent nav;
	public Vector3 spawnPosition;
	public float startWait;
	public int scoreValue;
	Vector3 targetPostDeath = Vector3.zero;
	Vector3 targetFlee = Vector3.zero;
	public Material[] materials;

	void Start(){
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
	
	void Awake () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
		nav = GetComponent <NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!gameController.gameOver){
			if(gameController.poweredUp == true){
				renderer.material = materials[1];
				if(targetFlee != Vector3.zero){
					do{
						targetFlee = new Vector3(Random.Range(-13, 13), Random.Range(-14, 15));
					}while(!Physics.Raycast(new Vector3(targetFlee.x, targetFlee.y+2, targetFlee.z), Vector3.down, 2));
					nav.SetDestination(targetFlee);
				}
			}
			else{
				renderer.material = materials[0];
				nav.SetDestination(target.position);
			}
		}
		else{
			if(targetPostDeath != Vector3.zero){
				do{
					targetPostDeath = new Vector3(Random.Range(-13, 13), Random.Range(-14, 15));
				}while(!Physics.Raycast(new Vector3(targetPostDeath.x, targetPostDeath.y+2, targetPostDeath.z), Vector3.down, 2));
				nav.SetDestination(targetPostDeath);
			}
		}
	}

	public void Spawn(){
		rigidbody.velocity = Vector3.zero;
		transform.position = spawnPosition;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			if(gameController.poweredUp == true){
				targetPostDeath = Vector3.zero;
				targetFlee = Vector3.zero;
				Spawn();
				gameController.AddScore(scoreValue);
			}
		}
		if(other.tag == "Enemy"){
			Physics.IgnoreCollision(other.collider, collider);
		}
	}

}
