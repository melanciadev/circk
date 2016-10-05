using UnityEngine;
using System.Collections;

namespace Circk{

	public class EnemyChaser : EnemyBase {

		//ENEMY STATE
		private bool chasing = true;

		// COMPONENTS
		public Transform orientationTr;

		public bool isBoss = false;

		public override void FixedUpdate() {
			base.FixedUpdate();

			if(normalBehaviour && GameManager.Instance.CurrentGameState == GameManager.GameState.GAME){
				Chase ();

				//Always look to the player
				orientationTr.LookAt(player.transform.position);
				//orientationTr.rotation = Quaternion.LookRotation(player.transform.position);
			}
		}

		protected override void OnCollisionEnter2D(Collision2D col){

			base.OnCollisionEnter2D (col);

			//If collides with the player
			if(col.gameObject.tag == "Player"){
				//Stop chasing
				chasing = false;

				an.SetTrigger ("Attack");
				GameManager.Instance.CrowdCheerAnimation();

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

		public override void Impact(Vector3 orientation,float force) {
			base.Impact(orientation,force);

			AudioStuff.PlaySound(isBoss ? "boss" : "urso",AudioStuff.volumeVoice);
		}

		protected override void Kill() {
			base.Kill();

			AudioStuff.PlaySound(isBoss ? "bossdead" : "ursodead",AudioStuff.volumeVoice);
		}
	}

}