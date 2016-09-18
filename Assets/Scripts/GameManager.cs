using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

namespace Circk{

	public class GameManager : MonoBehaviour {

		public enum GameState
		{
			TITLE,
			GAME,
			RETRY
		}

		public GameState CurrentGameState = GameState.TITLE;

		//TODO - TEMP
		public bool spawn = false;
		public GameObject enemy;

		public TitleScreen titleScreen;

		[Header("Energy Bar")]
		public GameObject energyBar;
		public GameObject energyBarFill;
		public GameObject energyBarSpecialFrame;
		public Animator energyBarAnimator;
		public float energyBarMax = 100f;
		public float energyBarCurrentPoints = 0f;
		public float energyBarTime = 10f;
		public Tweener barTween;

		[Header("Score")]
		public Text scoreValueText;
		public int currentScore = 0;
		public int maxScore = 0;

		[Header("Crowd")]
		protected float crowdOriginalYPos;
		public GameObject crowd;

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

		//ONLY INSTANCE
		static protected GameManager _instance = null;
		static public GameManager Instance { get { return GameManager._instance; } }

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

		void Start(){

			ResetEnergyBar ();
			ResetCurrentScore ();

			crowdOriginalYPos = crowd.transform.position.y;
			SetCrowdMove (true);
		}

		//TODO - TEMP
		private void Update(){
			if(spawn){
				SpawnEnemy(enemy);
				spawn = false;
			}
		}

		public void SpawnEnemy(GameObject enemyObject){
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
			GameObject enemyInstantiated = (GameObject)Instantiate (enemyObject, startPosition.transform.position, startPosition.transform.rotation);

			//Get the EnemyBase Component
			EnemyBase enemyScript = enemyInstantiated.GetComponent<EnemyBase>();

			//Set the SpawnPoints
			enemyScript.SetSpawnPoints(startPosition.transform, trueStartPosition.transform, opositePosition.transform);

			//Start the entrance movement
			enemyScript.StartEntrance();
		}

		// Realiza o Tween da variavel de Pontos da barra de especial
		public void StartFillEnergyBar(){
			barTween = DOTween.To (() => energyBarCurrentPoints, x => energyBarCurrentPoints = x, energyBarMax, energyBarTime)
				.OnUpdate (() => {
					SetEnergyBarFillYScale (energyBarCurrentPoints / energyBarMax);
				})
				.OnComplete (() => {
					energyBarAnimator.SetBool ("SpecialReady", true); 
					energyBarSpecialFrame.SetActive (true);
				})
				.SetEase (Ease.Linear);

		}

		public void ResetEnergyBar(){

			// CALL THIS AFTER SHOOTING A PROJECTILE

			barTween.Pause ();

			SetEnergyBarFillYScale (0f);

			energyBarCurrentPoints = 0f;
			energyBarSpecialFrame.SetActive (false);
			energyBarAnimator.SetBool ("SpecialReady", false);

			if (GameManager.Instance.CurrentGameState == GameState.GAME)
				StartFillEnergyBar ();
		}

		protected void SetEnergyBarFillYScale(float newY){
			var vAux = new Vector2(energyBarFill.transform.localScale.x, newY);
			energyBarFill.transform.localScale = vAux;
		}

		public void IncrementScore(int increment){
			currentScore += increment;
			UpdateScoreText ();
		}

		public void ResetCurrentScore(){
			currentScore = 0;
			UpdateScoreText ();
		}

		protected void UpdateScoreText(){
			scoreValueText.text = currentScore.ToString ();
		}

		public void CrowdCheerAnimation(){

			crowd.transform.DOMoveY (crowd.transform.position.y + 0.3f, 0.2f)
				.SetLoops(4, LoopType.Yoyo)
				.SetEase(Ease.Linear)
				.OnComplete(() => { crowd.transform.DOMoveY(crowdOriginalYPos, 0.1f); });
		}

		public void SetCrowdMove(bool move){
			crowd.GetComponent<Animator> ().SetBool("Move", move);
		}
	}
}