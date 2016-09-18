using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{

	public class EnemyLine : EnemyBase{

		//TWEENERS
		private Tweener tweenLineMove;

		private void FixedUpdate(){
			base.FixedUpdate();

			if(normalBehaviour){
				LineMove ();
			}
		}

		private void OnCollisionEnter2D(Collision2D col){
			base.OnCollisionEnter2D (col);

			//Pause the movement
			tweenLineMove.Pause();

			//Start the delay and unpause again after
			StartCoroutine(WaitAndCall(delayAfterHit, () => { 
				tweenLineMove.Play();
			}));
		}

		void LineMove(){
			if (tweenLineMove == null) {
				tweenLineMove = tr.DOMove (oppositeSpawn.position, speed)
					.SetLoops (-1, LoopType.Yoyo)
					.SetEase (Ease.Linear);
			}
		}

		public override void Impact(Vector3 orientation,float force) {
			base.Impact(orientation,force);

			AudioStuff.PlaySound("bailarina");
		}

		protected override void Kill() {
			base.Kill();

			AudioStuff.PlaySound("bailarinadead");
		}
	}
}