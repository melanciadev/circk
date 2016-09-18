using UnityEngine;
using System.Collections;

namespace Circk{

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
		public int maxEnemy;
		public int enemiesUntilBoss;

		float currentTimeBetween;
		float timeCounter;
		int bossCounter;

		private void Awake() {
			gameManager = GetComponent<GameManager>();
			currentTimeBetween = maxTimeBetween;
			timeCounter = minTimeBetween;
			bossCounter = enemiesUntilBoss;
		}

		private void FixedUpdate() {
			//Only works if in the game
			if (gameManager.CurrentGameState == GameManager.GameState.GAME) {
				timeCounter -= Time.deltaTime;
				if (timeCounter <= 0 && (EnemyBase.enemies == null || EnemyBase.enemies.Count < maxEnemy)) {
					timeCounter = currentTimeBetween;
					currentTimeBetween -= incrementalTime;
					if (currentTimeBetween < minTimeBetween) {
						currentTimeBetween = minTimeBetween;
					}
					switch ((int)(Random.value*3)) {
						case 0: gameManager.SpawnEnemy(enemyZig); break;
						case 1: gameManager.SpawnEnemy(enemyChaser); break;
						default: gameManager.SpawnEnemy(enemyLine); break;
					}
					bossCounter--;
					if (bossCounter <= 0) {
						bossCounter = enemiesUntilBoss;
						gameManager.SpawnEnemy(enemieBoss);
					}
				}
			}
		}
	}
}