using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{

	public class Ball : MonoBehaviour {

		// VALUES
		public float speed = 1.0f;
		public float maxVelocity = 1.0f;
		protected Vector2 direction;

		// COMPONENTS
		private Transform tr;
		private Rigidbody2D rb;

		void Awake(){
			tr = GetComponent<Transform> ();
			rb = GetComponent<Rigidbody2D> ();
		}
			
		void Start () {
			
		}

		void LateUpdate(){
			rb.velocity = maxVelocity * (rb.velocity.normalized);
		}
			
		void Update () {
			rb.AddForce (direction, ForceMode2D.Impulse);
		}

		protected void OnCollisionEnter2D(Collision2D col){
			
		}

		public void SetDirection(Vector2 vec){
			direction = vec;
		}
	}
}