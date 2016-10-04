using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Circk{

	public class EnemyBase : MonoBehaviour {

		public static List<EnemyBase> enemies = null;

		//VALUES
		public int scoreToGive = 1;
		public float power = 1.0f;
		public float weight = 1.0f;
		public float speed = 0.1f;
		public float entranceSpeed = 1.0f;
		public float delayAfterHit = 1f;
		protected float impactValue = 10f;
		protected Vector3 originalScale;

		//COMPONENTS
		protected Transform tr;
		protected Rigidbody2D rb;
		protected Collider2D cl;
		protected Animator an;

		//OUTSIDE COMPONENTS
		protected GameObject player;

		//SPAWN VALUES
		protected Transform originSpawn;
		protected Transform finalSpawn;
		protected Transform oppositeSpawn;

		//STATE
		protected bool entranceBehaviour = false;
		protected bool normalBehaviour = false;

		//POSITION
		private float lastPosition;


		private void Awake(){
			//Init Components
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody2D>();
			cl = GetComponent<Collider2D> ();
			an = GetComponent<Animator> ();

			//Find the player
			player = GameObject.FindGameObjectWithTag("Player");

			originalScale = tr.localScale;
			tr.localScale = new Vector3 (0.2f, 0.2f, 0.2f);

			if (enemies == null) {
				enemies = new List<EnemyBase>();
			}
			enemies.Add(this);

			//Set the first location
			lastPosition = tr.position.x;
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
				col.gameObject.GetComponent<PlayerController>().Impact (impactOrientation, impactValue * power);
			}

			if(col.gameObject.tag == "EdgeDeath"){
				Kill();
			}
		}

		//When the Enemy is hit - called by Ball and Lion
		public virtual void Impact(Vector3 orientation, float force){
			rb.AddForce (force * orientation, ForceMode2D.Impulse);
			AudioStuff.PlaySound("impact");
		}

		public void FixedUpdate(){
			if(lastPosition > tr.position.x){
				gameObject.GetComponent<SpriteRenderer>().flipX = true;
			}
			else if(lastPosition < tr.position.x){
				gameObject.GetComponent<SpriteRenderer>().flipX = false;
			}
			lastPosition = tr.position.x;
		}
			
		public void SetSpawnPoints(Transform originSpawn, Transform finalSpawn, Transform opositeSpawn){
			//Set the spawn points
			this.originSpawn = originSpawn;
			this.finalSpawn = finalSpawn;
			this.oppositeSpawn = opositeSpawn;
   		}
		public void StartEntrance(){
			
			entranceBehaviour = true;
			cl.enabled = false;

			tr.DOScale (originalScale, entranceSpeed/2f);

			tr.DOMove (finalSpawn.position, entranceSpeed)
				.OnComplete(() => {
					an.SetTrigger("Land");
					normalBehaviour = true;
					cl.enabled = true;
				})
				.SetEase(Ease.InBack);
		}

		protected IEnumerator WaitAndCall(float waitTime, Action callback){
			yield return new WaitForSeconds(waitTime);
			if (callback != null) {
				callback ();
			}
		}

		protected virtual void Kill(){
			tr.DOScale(new Vector3(0f, 0f, 0f), 0.3f).OnComplete(() => {
				GameManager.Instance.IncrementScore(scoreToGive);
				enemies.Remove(this);
				GameObject.Destroy(this.gameObject);
			});
				
		}
	}
}