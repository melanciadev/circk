using UnityEngine;
using System.Collections;

namespace Game{

	public class WaveManager : MonoBehaviour {

		[Header("GameManager")]
		private GameManager gameManager; 

		[Header("Enemies")]
		public GameObject enemyZig;
		public GameObject enemyChaser;
		public GameObject enemyLine;
		public GameObject enemieBoss;

		[Header("Values")]
		public float minTimeBetween;
		public float maxTimeBetween;
		public float incrementalTime;
		public float maxEnemy;
		public float enemiesUntilBoss;

		[Header("Spawn Time")]
		private float timeSinceLastSpawn;
		private int enemiesInGame;
		private GameObject[] enemyArray;

		private void Awake(){
			gameManager = GetComponent<GameManager>();
		}

		private void FixedUpdate(){
			//Only works if the game is on
			if(gameManager.gameOn){
				//TODO
			}
		}

	}
}