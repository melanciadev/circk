using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;


namespace Circk{
	public class TitleScreen : MonoBehaviour {

		[Header("UI")]
		public GameObject logo;
		public GameObject logoFinalPos;
		public GameObject credits;
		public GameObject creditsFinalPos;
		public GameObject message;
		public GameObject messageFinalPos;
		public Image messageImage;
		public Sprite messageImageMenu;
		public Sprite messageImageRetry;

		protected Vector3 logoBeganPos;
		protected Vector3 creditsBeganPos;
		protected Vector3 messageBeganPos;
		protected Vector3 gameOverBeganPos;

		public GameObject gameOver;
		public Text finalScoreValue;
		public Text maxScoreValue;

		public float introTime = 1.0f;
		protected bool onIntro = true;

		void Awake(){
			logoBeganPos = logo.transform.position;
			creditsBeganPos = credits.transform.position;
			messageBeganPos = message.transform.position;
			gameOverBeganPos = gameOver.transform.position;

		}

		// Use this for initialization
		void Start () {
			//maxScoreValue.text = PlayerPrefs.GetInt("MaxScore").ToString();
			maxScoreValue.text = "oi2";
			Intro ();
		}
		
		// Update is called once per frame
		void Update () {
			if(GameManager.Instance.CurrentGameState == GameManager.GameState.GAME){
				if(GameManager.Instance.currentScore > PlayerPrefs.GetInt("MaxScore")){
					PlayerPrefs.SetInt("MaxScore", GameManager.Instance.currentScore);
					maxScoreValue.text = PlayerPrefs.GetInt("MaxScore").ToString();
				}

			}

			if (Input.GetKeyDown(KeyCode.Space)){
				switch (GameManager.Instance.CurrentGameState) {
				case GameManager.GameState.TITLE:
					IntroOut();
					break;

				case GameManager.GameState.RETRY:
					
					GameOverOutro ();
					break;

				default:
					break;
				}

			}
		}

		public void Intro(){

			onIntro = true;

			logo.transform.DOMove (logoFinalPos.transform.position, introTime)
				.OnComplete(() => { 
					credits.transform.DOMove(creditsFinalPos.transform.position, introTime)
						.OnComplete(() => { 
							message.transform.DOMove(messageFinalPos.transform.position, introTime / 2)
								.OnComplete(() => { onIntro = false; });
						});
				});
		}

		public void IntroOut(){
			if (onIntro)
				return;

			message.transform.DOMove (messageBeganPos, introTime / 2);
			credits.transform.DOMove (creditsBeganPos, introTime / 2);
			logo.transform.DOMove(logoBeganPos, introTime / 2);

			GameManager.Instance.CurrentGameState = GameManager.GameState.GAME;
			GameManager.Instance.StartFillEnergyBar ();
		}

		public void CallGameOver(){
			
			onIntro = true;

			GameManager.Instance.CurrentGameState = GameManager.GameState.RETRY;

			messageImage.sprite = messageImageRetry;

			finalScoreValue.text = GameManager.Instance.currentScore.ToString();
			//maxScoreValue.text = GameManager.Instance.maxScore.ToString();
		
			gameOver.transform.DOMoveX (0f, introTime);
			credits.transform.DOMove (creditsFinalPos.transform.position, introTime);
			message.transform.DOMove (messageFinalPos.transform.position, introTime).OnComplete(() => { onIntro = false; });

		}

		public void GameOverOutro(){
			
			if (onIntro)
				return;
			

			PlayerPrefs.SetInt("MaxScore", GameManager.Instance.currentScore);

			gameOver.transform.DOMove (gameOverBeganPos, introTime / 2);
			credits.transform.DOMove (creditsBeganPos, introTime / 2 );
			message.transform.DOMove (messageBeganPos, introTime / 2).OnComplete(() => {
				Application.LoadLevelAsync ("Main");
				EnemyBase.enemies.Clear();
				ItemObject.items.Clear();
			});

		}
	}
}
