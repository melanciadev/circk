using UnityEngine;
using System.Collections;

namespace Circk {
	public class SceneManager:MonoBehaviour {
		public static SceneManager me = null;

		public Sprite[] cortinas;
		
		Transform cortinaTr;
		SpriteRenderer cortinaSprite;

		static float sceneTempo;
		static string nextScene;
		static bool loadingScene;
		
		void Awake() {
			if (me != null) {
				Destroy(gameObject);
				return;
			}
			me = this;
			DontDestroyOnLoad(gameObject);
			
			cortinaTr = transform.Find("cortina");
			cortinaSprite = cortinaTr.GetComponent<SpriteRenderer>();
			cortinaSprite.enabled = false;
			loadingScene = false;
		}

		void Update() {
			const float vel = 1.25f;
			if (loadingScene) {
				sceneTempo += Time.deltaTime*vel;
				if (sceneTempo >= 1) {
					sceneTempo = 1;
					loadingScene = false;
					UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
				}
				AnimCortina(true);
			} else if (sceneTempo > 0) {
				sceneTempo -= Time.deltaTime*vel;
				if (sceneTempo <= 0) {
					sceneTempo = 0;
					AnimCortina(false);
				} else {
					AnimCortina(true);
				}
			}
		}

		void AnimCortina(bool show) {
			if (show) {
				float t = Mathf.Clamp01(sceneTempo*1.25f);
				cortinaSprite.enabled = true;
				int index = (int)(t*1.25f*(cortinas.Length-1));
				if (index > cortinas.Length-1) index = cortinas.Length-1;
				cortinaSprite.sprite = cortinas[cortinas.Length-1-index];
				cortinaTr.localScale = Vector3.Lerp(new Vector3(1.3f,1.3f,1),new Vector3(.72f,.72f,1),(2-t)*t);
			} else {
				cortinaSprite.enabled = false;
			}
		}

		public static void LoadScene(string name) {
			if (sceneTempo > 0) return;
			loadingScene = true;
			nextScene = name;
		}
	}
}