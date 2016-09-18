using UnityEngine;
using System.Collections;

public class CameraManager:MonoBehaviour {
	int width;
	int height;
	const float idealProp = 16f/9f;

	void Start() {
		width = Screen.width;
		height = Screen.height;
		var prop = (float)width/height;
		Rect rect;
		if (prop > idealProp) {
			var propSize = idealProp/prop;
			var propMin = (1-propSize)/2f;
			rect = new Rect(propMin,0,propSize,1);
		} else if (prop < idealProp) {
			var propSize = prop/idealProp;
			var propMin = (1-propSize)/2f;
			rect = new Rect(0,propMin,1,propSize);
		} else {
			rect = new Rect(0,0,1,1);
		}
		GetComponent<Camera> ().rect = rect;
	}

	void Update() {
		if (width != Screen.width || height != Screen.height) {
			Start();
		}
	}
}