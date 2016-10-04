using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{

	public class Ball : MonoBehaviour {

		// VALUES
		public float speed = 1.0f;
		public float maxVelocity = 1.0f;
		protected Vector2 direction;
		public float impactValue = 10.0f;
		private float lastPosition;

		// COMPONENTS
		private Transform tr;
		private Rigidbody2D rb;
		public SpriteRenderer spriteRenderer;


		void Awake(){
			tr = GetComponent<Transform> ();
			rb = GetComponent<Rigidbody2D> ();
		
			Physics2D.IgnoreCollision(PlayerController.me.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
		}

		public void Start(){
			lastPosition = tr.position.x;
		}

		public void SetSpeed(float speed){
			this.speed = speed * 2;
			rb.AddForce (tr.up * speed, ForceMode2D.Impulse);
			print(tr.forward);
		}

		protected void OnCollisionEnter2D(Collision2D col){
			//If collides with the player - Push
			if (col.gameObject.tag == "Enemy") {
				//Set the orientation and intensity of the impact that will cause to the player
				Vector3 impactOrientation = -col.contacts[0].normal; 
				col.gameObject.GetComponent<EnemyBase>().Impact(impactOrientation, impactValue * speed);
				Destroy(gameObject, 0.1f);
			}

			if(col.gameObject.tag == "EdgeDeath"){
				Destroy(gameObject, 0.1f);
			}
		}

		public void FixedUpdate(){
			if(lastPosition > tr.position.x){
				spriteRenderer.flipX = false;
			}
			else if(lastPosition < tr.position.x){
				spriteRenderer.flipX = true;
			}
			lastPosition = tr.position.x;
		}
	}
}