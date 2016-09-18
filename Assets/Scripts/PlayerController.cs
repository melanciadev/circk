using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Game{
	
	public class PlayerController : MonoBehaviour {

		// VALUES
		public float speed;
		public float delayAfterHit = 0.5f;

		// STATES
		protected bool moveLocked = false;

		// COMPONENTS
		private Transform tr;
		private Rigidbody2D rb;
		private Animator an;
		private SpriteRenderer sr;

		// OUTISDE
		private GameObject edgeWarning;
		private GameObject edgeDeath;

		private void Awake(){
			//Init Components
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody2D>();
			an = GetComponent<Animator> ();
			sr = GetComponent<SpriteRenderer> ();

			//Find the game borders
			edgeWarning = GameObject.FindGameObjectWithTag ("EdgeWarning");
			edgeDeath = GameObject.FindGameObjectWithTag ("EdgeDeath");
		}

		private void Update(){
			Move ();
		}

		public void Move (){
			
			//Dont move if player has just been pushed
			if(!moveLocked){
				//Get the input
				float horizontalValue = Input.GetAxis("Horizontal") * speed;
				float verticalValue = Input.GetAxis("Vertical") * speed * 0.75f;

				var scaleAux = tr.localScale;
				 
				/*
				if (!Mathf.Approximately (horizontalValue, 0f)) {
					
					scaleAux.x *= Mathf.Sign (horizontalValue);
					tr.localScale = scaleAux;

					//tr.localScale = new Vector2(scaleAux.x, scaleAux.y);
				}
				*/

				//Walk
				an.SetBool("Walking", (Mathf.Abs(verticalValue) > 0f) || Mathf.Abs(horizontalValue) > 0f);
				tr.Translate (horizontalValue * Time.deltaTime, verticalValue * Time.deltaTime, 0);
			}
		}

		private void OnCollisionEnter2D(Collision2D collision){
			//Gameover when collide to the border
			if (collision.gameObject.tag == "EdgeDeath")
				Debug.Log ("Fim de Jogo");
		}

		private void OnTriggerEnter2D(Collider2D collider){
			if (collider.gameObject.tag == "EdgeWarning") {
				an.SetBool ("Balance", true);
				Debug.Log ("Warning");
			}
		}

		private void OnTriggerExit2D(Collider2D collider){
			if (collider.gameObject.tag == "EdgeWarning") {
				an.SetBool ("Balance", false);
			}
		}

		//When the player is hit - called by enemy
		public void Impact(Vector3 orientation, float force){

			an.SetTrigger ("Hit");

			//Only works if the movementLoch is false
			if (!moveLocked) {
				rb.AddForce (force * orientation, ForceMode2D.Impulse);

				moveLocked = true;

				StartCoroutine (WaitAndCall (delayAfterHit, () => {
					moveLocked = false;
				}));
			}
		}

		//Wait to restart movement after hit
		private IEnumerator WaitAndCall(float waitTime, Action callback){
			
			//Set the moventLock to false after the delay
			yield return new WaitForSeconds(waitTime);
			if (callback != null)
				callback ();
		}
	}
}