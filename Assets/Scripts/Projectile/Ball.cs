using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{

	public class Ball : MonoBehaviour {

		public Transform orientation;

		// VALUES
		public float speed = 1.0f;
		public float maxVelocity = 1.0f;
		protected Vector2 direction;
		public float impactValue = 5.0f;
		public bool rotate = false;
		private bool dead = false;

		// COMPONENTS
		private Transform tr;
		private Rigidbody2D rb;
		public SpriteRenderer spriteRenderer;
		private Transform spriteTr;


		void Awake(){
			tr = GetComponent<Transform> ();
			rb = GetComponent<Rigidbody2D> ();
			orientation = tr.Find("orientation");
			spriteTr = spriteRenderer.transform;

			Physics2D.IgnoreCollision(PlayerController.me.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
		}

		public void Start(){
			tr.localRotation = Quaternion.identity;
		}

		public void SetSpeed(float speed){
			this.speed = speed;
			var or = orientation.up;
			spriteRenderer.flipX = or.x >= 0;
			rb.AddForce (or * speed, ForceMode2D.Impulse);
		}
		
		protected void OnCollisionEnter2D(Collision2D col){
			if (dead) return;
			//If collides with the player - Push
			if (col.gameObject.tag == "Enemy") {
				//Set the orientation and intensity of the impact that will cause to the player
				Vector3 impactOrientation = -col.contacts[0].normal; 
				col.gameObject.GetComponent<EnemyBase>().Impact(impactOrientation, impactValue * speed);
				dead = true;
				tr.DOScale(Vector3.zero,.1f).OnComplete(() => {
					Destroy(gameObject);
				});
				ParticleManager.Hit(col.contacts[0].point,true);
			}

			if(col.gameObject.tag == "EdgeDeath"){
				dead = true;
				tr.DOScale(Vector3.zero,.1f).OnComplete(() => {
					Destroy(gameObject);
				});
			}
		}

		void Update() {
			if (rotate) {
				spriteTr.localRotation = Quaternion.Euler(0,0,speed*Time.time*30);
			}
		}
	}
}