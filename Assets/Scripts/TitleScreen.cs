using UnityEngine;
using System.Collections;
using DG.Tweening;


namespace Circk{
	public class TitleScreen : MonoBehaviour {

		[Header("UI")]
		public GameObject logo;
		public GameObject logoFinalPos;
		public GameObject credits;
		public GameObject creditsFinalPos;
		public GameObject message;
		public GameObject messageFinalPos;

		protected Vector3 logoBeganPos;
		protected Vector3 creditsBeganPos;
		protected Vector3 messageBeganPos;

		public float introSpeed = 1.0f;
		protected bool onIntro = true;

		void Awake(){
			logoBeganPos = logo.transform.position;
			creditsBeganPos = credits.transform.position;
			messageBeganPos = message.transform.position;
		}

		// Use this for initialization
		void Start () {
			Intro ();
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown(KeyCode.Space)) {
				Outro ();
				Debug.Log ("OK");
			}
		}


		public void Intro(){
			logo.transform.DOMove (logoFinalPos.transform.position, introSpeed)
				.OnComplete(() => { 
					credits.transform.DOMove(creditsFinalPos.transform.position, introSpeed)
						.OnComplete(() => { 
							message.transform.DOMove(messageFinalPos.transform.position, introSpeed)
								.OnComplete(() => { onIntro = false; });
						});
				});
		}

		public void Outro(){
			//intro.PlayBackwards (); TEM COMO MAS IA DEMORAR

			if (onIntro)
				return;

			message.transform.DOMove (messageBeganPos, introSpeed);
			credits.transform.DOMove (creditsBeganPos, introSpeed);
			logo.transform.DOMove(logoBeganPos, introSpeed);
				
		}
	}
}
