using UnityEngine;
using System.Collections;

namespace Circk {
	public class ParticleManager:MonoBehaviour {
		public static ParticleManager me;

		Transform tr;
		ParticleSystem ps;
		ParticleSystem ps2;

		void Awake() {
			me = this;
			tr = transform;
			ps = tr.Find("hit").GetComponent<ParticleSystem>();
			ps2 = tr.Find("hit2").GetComponent<ParticleSystem>();
		}
		
		public static void Hit(Vector2 pos,bool goodHit) {
			me.tr.position = pos;
			(goodHit ? me.ps : me.ps2).Emit(Random.Range(8,10));
		}
	}
}