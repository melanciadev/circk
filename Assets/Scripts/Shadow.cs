using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk {
	public class Shadow:MonoBehaviour {
		public bool glow = false;

		Transform tr;
		SpriteRenderer rend;

		void Awake() {
			tr = transform;
			rend = tr.parent.GetComponent<SpriteRenderer>();
			if (rend == null) {
				rend = tr.parent.Find("sprite").GetComponent<SpriteRenderer>();
			}
		}

		void Start() {
			GetComponent<SpriteRenderer>().sortingOrder = -3;
			rend.sortingLayerName = "StageStuff";
			if (glow) {
				tr.DOScale(tr.localScale*1.25f,.92f)
					.SetLoops(-1,LoopType.Yoyo)
					.SetEase(Ease.InOutSine);
			}
			Update();
		}

		void Update() {
			rend.sortingOrder = (int)((10-tr.position.y)*16);
		}
	}
}