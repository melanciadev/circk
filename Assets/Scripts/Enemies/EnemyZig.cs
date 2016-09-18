using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Game{
	
	public class EnemyZig : EnemyBase  {

		private bool zigMovementStarted;
		public float desiredVelocity;
		public bool touchedPlayer;

		private void FixedUpdate(){
			base.FixedUpdate();

			if(normalBehaviour){
				ZigMove();
			}
		}

		private void LateUpdate(){
			if(normalBehaviour){
				rb.velocity = desiredVelocity * (rb.velocity.normalized);
			}
		}

		private void OnCollisionEnter2D(Collision2D col){
			base.OnCollisionEnter2D (col);

			//If collides with the player
			if(col.gameObject.tag == "Player"){ //TODO - colidir com outras 
				gameObject.layer = 0;

				an.SetTrigger ("Hit");

				StartCoroutine(WaitAndCall(delayAfterHit, () => { 
					gameObject.layer = 8;
				}));
			}
		}
			
		void ZigMove(){	
			if (!zigMovementStarted) {
				zigMovementStarted = true;
				rb.AddForce(new Vector3(speed, speed, 0));
			} 
		}
	}
}