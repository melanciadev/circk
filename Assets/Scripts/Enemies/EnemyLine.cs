using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{

	public class EnemyLine : EnemyBase{

		private Vector2[] line;
		private int pointIndex;

		//TWEENERS
		
		private Tweener tweenLineMove;

		public override void FixedUpdate(){
			base.FixedUpdate();
		}

		protected override void OnCollisionEnter2D(Collision2D col){
			base.OnCollisionEnter2D (col);
		}

		void Update() {
			if (normalBehaviour) {
				if (line == null) {
					line = new Vector2[2] { transform.position,oppositeSpawn.position };
					pointIndex = 1;
				}
				const float yr = .75f;
				Vector2 pos = tr.position;
				Vector2 end = line[pointIndex];
				Vector2 dir = new Vector2(end.x-pos.x,(end.y-pos.y)/yr).normalized;
				float horizontalValue = dir.x * speed * Time.deltaTime;
				float verticalValue = dir.y * speed * yr * Time.deltaTime;
				if (Mathf.Sign(pos.x-end.x) != Mathf.Sign(pos.x+horizontalValue-end.x) || Mathf.Sign(pos.y-end.y) != Mathf.Sign(pos.y+verticalValue-end.y)) {
					tr.position = end;
					pointIndex = (pointIndex == 1) ? 0 : 1;
				} else {
					tr.Translate(horizontalValue,verticalValue,0);
				}

				tr.Translate(horizontalValue * Time.deltaTime,verticalValue * Time.deltaTime,0);
			}
		}

		public override void Impact(Vector3 orientation,float force) {
			base.Impact(orientation,force);

			AudioStuff.PlaySound("bailarina",AudioStuff.volumeVoice);
		}

		protected override void Kill() {
			base.Kill();

			AudioStuff.PlaySound("bailarinadead",AudioStuff.volumeVoice);
		}
	}
}