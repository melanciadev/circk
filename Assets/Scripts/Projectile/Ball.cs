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

		// COMPONENTS
		private Transform tr;
		private Rigidbody2D rb;

		void Awake(){
			tr = GetComponent<Transform> ();
			rb = GetComponent<Rigidbody2D> ();
		
			Physics2D.IgnoreCollision(PlayerController.me.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
		}

		public void SetSpeed(float speed){
			this.speed = speed;
			rb.AddForce (tr.up * speed, ForceMode2D.Impulse);
			print(tr.forward);
		}

		protected void OnCollisionEnter2D(Collision2D col){
			//If collides with the player - Push
			if (col.gameObject.tag == "Enemy") {
				//Set the orientation and intensity of the impact that will cause to the player
				Vector3 impactOrientation = -col.contacts[0].normal; 
				col.gameObject.GetComponent<EnemyBase>().Impact(impactOrientation, impactValue * speed);
				Destroy(gameObject, 0.3f);
			}
		}
	}
}