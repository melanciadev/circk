using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour {

	public Camera camera;

	public SpriteRenderer logo;
	public SpriteRenderer black;
	public SpriteRenderer black2; // PQ TAVA DANDO BUG AFF :(

	public Animator cortinaAn;

	// Use this for initialization
	void Start () {

		var color = new Color (logo.color.r, logo.color.g, logo.color.b, 0f);

		black.DOColor (color, 2f).OnComplete(() => { 
			logo.DOColor (color, 1f)
				.SetDelay (2f)
				.OnComplete(() => {
					cortinaAn.SetTrigger("Open");
					camera.DOOrthoSize(6f, 1.5f).OnComplete(() => {
						SceneManager.LoadScene("Main");
					});
				});
		});

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
