using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject paletPrefab;
	public GameObject powerUpPrefab;
	public GameObject bonusPrefab;
	private List<GameObject> palets = new List<GameObject>();

	public int lifeNumber = 3;
	public int score = 0;
	public int scoreToLife = 10000;
	public int lastScore = 0;
	public bool poweredUp = false;
	private bool bonusUp = false;
	public float bonusWaitTime = 20;
	public float bonusTime = 10;
	private float currentBonusTime;
	private float currentBonusWaitTime;
	private GameObject bonusObject;

	public GUIText scoreText;
	public GUIText lifeText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText menuText;

	public bool gameOver;
	private bool restart;
	private bool nextLevel;

	public AudioClip oneUp;
	public AudioClip playerDies;
	public AudioClip pointsUp;
	public AudioClip siren;
	
	//Initiate the displays and spawns the pucks
	void Start () {
		SpawnPalets();
		UpdateScore();
		UpdateLife();
		restart = false;
		gameOver = false;
		restartText.text = "";
		gameOverText.text = "";
		menuText.text = "";
		currentBonusWaitTime = bonusWaitTime;
		currentBonusTime = bonusTime;
	}

	//Spawn pucks on a grid, spaced by 1
	void SpawnPalets(){
		Vector3 spawnPos = new Vector3();
		int i,j;
		for(i = -13; i < 13; i++){
			for(j = -14 ; j < 15; j++){
				if ((i >- 6 && i < 5) && (j > -5 && j < 7)){}
				else if ((i< -8 || i > 7) && (j > -5 && j < 7)){}
				else{
					//Align on a "grid"
					spawnPos.x = i + 0.5f;
					spawnPos.z = j;
					//Hard coding the position of the power-ups
					if((i == -13 && j == 12)||(i == -13 && j == -8)||(i == 12 && j == 12)||(i == 12 && j == -8)){
						GameObject g = Instantiate(powerUpPrefab, spawnPos, Quaternion.identity) as GameObject;
						palets.Add(g);
					}
					else{
						//Test if there is a block. If not, don't Instantiate a puck
						if (!Physics.Raycast(new Vector3(spawnPos.x, spawnPos.y+2, spawnPos.z), Vector3.down, 2)){
							GameObject g = Instantiate(paletPrefab, spawnPos, Quaternion.identity) as GameObject;
							palets.Add(g);
						}
					}
				}
			}
		}
	}

	void Update(){
		Vector3 spawnPos = new Vector3(0, 0, -2);

		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R)){
				Application.LoadLevel (Application.loadedLevel);
			}
			if(Input.GetKeyDown (KeyCode.M)){
				Application.LoadLevel("Menu");
			}
		}
		if(lifeNumber <=0){
			GameOver ();
		}
		if(palets.Count == 0){
			EndGame();
		}

		if(!bonusUp){
			currentBonusWaitTime -= Time.deltaTime;
			//Bonus generator
			if(currentBonusWaitTime <= 0){
				bonusUp = true;
				currentBonusTime = bonusTime;
				bonusObject = Instantiate(bonusPrefab, spawnPos, Quaternion.identity) as GameObject;
			}
		}
		else{
			currentBonusTime -= Time.deltaTime;
			if(currentBonusTime <= 0){
				Destroy(bonusObject);
				bonusUp = false;
				currentBonusWaitTime = bonusWaitTime;
			}
		}

	}

	public void GameOver(){
		restartText.text = "Press 'R' for Restart";
		restart = true;
		gameOverText.text = "Game Over!";
		gameOver = true;
		menuText.text = "Press 'M' for Menu";
		GameObject[] enemyListObject = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject g in enemyListObject){
			Destroy(g);
		}
	}

	public void EndGame(){
		restartText.text = "Press 'R' for Restart";
		restart = true;
		gameOverText.text = "You win!";
		gameOver = true;
		menuText.text = "Press 'M' for Menu";
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		Destroy(playerObject);
		GameObject[] enemyListObject = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject g in enemyListObject){
			Destroy(g);
		}
	}

	//Add score to the total and Update the display
	public void AddScore(int newScoreValue){
		score += newScoreValue;
		if(score >= lastScore + scoreToLife){
			lastScore = score;
			AddLife();
		}
		UpdateScore();
	}

	public void AddScore(int newScoreValue, GameObject paletEaten){
		if(paletEaten.tag == "Bonus"){
			bonusUp = false;
		}
		else{
			palets.Remove (paletEaten);
		}

		audio.clip = pointsUp;
		if (audio.clip == pointsUp && !audio.isPlaying){
			audio.Play();
		}
		score += newScoreValue;
		//Give a life if the player has scored scoreToLife since it's last death
		if(score >= lastScore + scoreToLife){
			lastScore = score;
			AddLife();
		}
		UpdateScore();
	}

	//Update the GUIText display
	void UpdateScore(){
		scoreText.text = "Score\n" + score;
	}

	//Lose a life and update the display
	public void LoseLife(){
		audio.clip = playerDies;
		audio.Play();
		lifeNumber--;
		lastScore = score;
		UpdateLife();
	}

	//Gain a life and update the display
	public void AddLife(){
		audio.clip = oneUp;
		audio.Play();
		if(lifeNumber <=4){
			lifeNumber++;
		}
		UpdateLife();
	}

	//Update the life number display
	void UpdateLife(){
		lifeText.text = "Life\n" + lifeNumber;
	}

	public void LaunchSiren(){
		poweredUp = true;
		audio.clip = siren;
		audio.loop = true;
		audio.Play();
	}

	public void StopSiren(){
		poweredUp = false;
		audio.clip = siren;
		audio.Stop();
		audio.loop = false;
	}
}
