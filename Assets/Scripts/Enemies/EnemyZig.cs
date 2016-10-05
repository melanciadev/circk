using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{
	
	public class EnemyZig : EnemyBase  {

		private bool zigMovementStarted;
		public float desiredVelocity;
		public bool touchedPlayer;

		public override void FixedUpdate(){
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

		protected override void OnCollisionEnter2D(Collision2D col){
			base.OnCollisionEnter2D (col);

			//If collides with the player
			if(col.gameObject.tag == "Player"){
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

		public override void Impact(Vector3 orientation,float force) {
			base.Impact(orientation,force);

			AudioStuff.PlaySound("bruno",AudioStuff.volumeVoice);
		}

		protected override void Kill() {
			base.Kill();

			AudioStuff.PlaySound("brunodead",AudioStuff.volumeVoice);
		}
	}
}