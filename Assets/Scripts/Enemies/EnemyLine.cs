using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Game{

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

			//If collides with the player
			if(col.gameObject.tag == "Player"){
				//Pause the movement
				tweenLineMove.Pause();

				//Start the delay and unpause again after
				StartCoroutine(WaitAndCall(delayAfterHit, () => { 
					tweenLineMove.Play();
				}));
			}
		}

		void LineMove(){
			if (tweenLineMove == null) {
				tweenLineMove = tr.DOMove (opositeSpawn.position, speed)
					.SetLoops (-1, LoopType.Yoyo)
					.SetEase (Ease.Linear);
			}
		}
	}
}