using UnityEngine;
using System.Collections;

namespace Game{

	public class EnemyChaser : EnemyBase {

		//ENEMY STATE
		private bool chasing = true;

		// COMPONENTS
		public Transform orientationTr;

		void FixedUpdate () {
			base.FixedUpdate();

			if(normalBehaviour){
				Chase ();

				//Always look to the player
				orientationTr.rotation = Quaternion.LookRotation(player.transform.position);
			}
		}

		private void OnCollisionEnter2D(Collision2D col){

			base.OnCollisionEnter2D (col);

			//If collides with the player
			if(col.gameObject.tag == "Player"){
				//Stop chasing
				chasing = false;

				//Start the delay and chase again after
				StartCoroutine(WaitAndCall(delayAfterHit, () => { 
					chasing = true; 
				}));
			}
		}

		void Chase(){
			//Seek the Player
			if(chasing){
				tr.position = Vector2.MoveTowards(tr.position, player.transform.position, speed * Time.deltaTime);
			}
				
		}
	}

}