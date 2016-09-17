using UnityEngine;
using System.Collections;

namespace Game{

	public class GameManager : MonoBehaviour {

		static protected GameManager _instance = null;
		static public GameManager Instance { get { return GameManager._instance; } }

		public bool spawn = false; //TODO - TEMP
		public GameObject enemy;


		// POSITION SPAWN OBJECTS
		[Header("Spawn")]
		public GameObject leftSpawn;
		public GameObject rightSpawn;
		public GameObject topSpawn;
		public GameObject bottomSpawn;

		[Header("Destinations")]
		public GameObject trueLeftSpawn;
		public GameObject trueRightSpawn;
		public GameObject trueTopSpawn;
		public GameObject trueBottomSpawn;


		private void Awake (){
			//Only one GameManager
			if (GameManager.Instance == null)
			{
				GameManager._instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				GameObject.Destroy(this.gameObject);
				return;
			}
		}

		//TODO - TEMP
		private void Update(){
			if(spawn){
				SpawnEnemy ();
				spawn = false;
			}
		}

		private void SpawnEnemy(){
			GameObject startPosition = leftSpawn;
			GameObject trueStartPosition = trueLeftSpawn;
			GameObject opositePosition = trueRightSpawn;

			//Randomize the StartPoint
			int randomSpawnPoint = Random.Range(0, 4);
			switch(randomSpawnPoint){
			case 0:
				startPosition = leftSpawn;
				trueStartPosition = trueLeftSpawn;
				break;
			case 1:
				startPosition = rightSpawn;
				trueStartPosition = trueRightSpawn;
				break;
			case 2:
				startPosition = topSpawn;
				trueStartPosition = trueTopSpawn;
				break;
			case 3:
				startPosition = bottomSpawn;
				trueStartPosition = trueBottomSpawn;
				break;
			}

			//Randomize the OpositePoint
			int randomOpositePoint = Random.Range(0, 4);
			if(randomOpositePoint == randomSpawnPoint){
				if (randomOpositePoint == 3) {
					randomOpositePoint = 0;
				} else {
					randomOpositePoint++;
				}
			}
			switch(randomOpositePoint){
			case 0:
				opositePosition = trueLeftSpawn;
				break;
			case 1:
				opositePosition = trueRightSpawn;
				break;
			case 2:
				opositePosition = trueTopSpawn;
				break;
			case 3:
				opositePosition = trueBottomSpawn;
				break;
			}
		
			//Instantiate
			GameObject enemyInstantiated = (GameObject)Instantiate (enemy, startPosition.transform.position, startPosition.transform.rotation);

			//Get the EnemyBase Component
			EnemyBase enemyScript = enemyInstantiated.GetComponent<EnemyBase>();

			//Set the SpawnPoints
			enemyScript.SetSpawnPoints(startPosition.transform, trueStartPosition.transform, opositePosition.transform);

			//Start the entrance movement
			enemyScript.StartEntrance();
		}
	}
}