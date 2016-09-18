using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {

	private Transform tr;

	private void Awake(){
		tr = GetComponent<Transform>();
	}

	private void Start(){
		tr.eulerAngles = Vector3.zero;
	}
}
