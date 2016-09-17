using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Game{

	public class EnemyBase : MonoBehaviour {

		//VALUES
		public float speed = 0.1f;
		public float entranceSpeed = 1.0f;
		public float delayAfterHit = 1f;
		protected float impactValue = 10f;

		//COMPONENTS
		protected Transform tr;
		protected Rigidbody2D rb;
		protected Collider2D cl;

		//OUTSIDE COMPONENTS
		protected GameObject player;

		//SPAWN VALUES
		protected Transform originSpawn;
		protected Transform finalSpawn;
		protected Transform opositeSpawn;

		//STATE
		protected bool entranceBehaviour = false;
		protected bool normalBehaviour = false;


		private void Awake(){
			//Init Components
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody2D>();
			cl = GetComponent<Collider2D> ();

			//Find the player
			player = GameObject.FindGameObjectWithTag("Player");
		}
			
		protected void OnCollisionEnter2D(Collision2D col){

			// If collides with another fellow enemy - Ignore
			if (col.gameObject.tag == "Enemy") {
				Physics2D.IgnoreCollision (col.collider, cl);
			}

			//If collides with the player - Push
			if (col.gameObject.tag == "Player") {
				//Set the orientation and intensity of the impact that will cause to the player
				Vector3 impactOrientation = -col.contacts[0].normal; 
				col.gameObject.GetComponent<PlayerController>().Impact (impactOrientation, impactValue);
			}

			if(col.gameObject.tag == "EdgeDeath"){
				Kill();
			}
		}

		public void FixedUpdate(){
			
		}
			
		public void SetSpawnPoints(Transform originSpawn, Transform finalSpawn, Transform opositeSpawn){
			//Set the spawn points
			this.originSpawn = originSpawn;
			this.finalSpawn = finalSpawn;
			this.opositeSpawn = opositeSpawn;
   		}
		public void StartEntrance(){
			
			entranceBehaviour = true;
			cl.enabled = false;

			tr.DOMove (finalSpawn.position, entranceSpeed)
				.OnComplete(() => {
					normalBehaviour = true;
					cl.enabled = true;
				})
				.SetEase(Ease.InOutBack);
		}

		protected IEnumerator WaitAndCall(float waitTime, Action callback){
			//Set the chasing to true after the delay
			yield return new WaitForSeconds(waitTime);
			if (callback != null) {
				callback ();
			}
		}

		protected void Kill(){
			//Debug.Log ("Morri");
			GameObject.Destroy (this.gameObject);
		}
	}
}