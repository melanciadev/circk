using UnityEngine;
using System.Collections;
namespace Circk{
	
	public class RotationArrow : MonoBehaviour {

		private Transform tr;

		public enum Angle{
			UP,
			UPLEFT,
			UPRIGHT,
			DOWN,
			DOWNRIGHT,
			DOWNLEFT,
			LEFT,
			RIGHT
		} 
		public Angle currentAngle;

		private void Awake(){
			
			tr = GetComponent<Transform>();
		}

		private void Update(){
			if(GameManager.Instance.CurrentGameState == GameManager.GameState.GAME){

				if (Input.GetAxis ("Vertical") > 0) {
					//upRight
					if (Input.GetAxis ("Horizontal") > 0) {
						currentAngle = Angle.UPRIGHT;
					}
					//upLeft
					else if (Input.GetAxis ("Horizontal") < 0) {
						currentAngle = Angle.UPLEFT;
					}
					//up
					else {
						currentAngle = Angle.UP;
					}
				} else if (Input.GetAxis ("Vertical") < 0) {
					//downRight
					if (Input.GetAxis ("Horizontal") > 0) {
						currentAngle = Angle.DOWNRIGHT;
					}
					//downLeft
					else if (Input.GetAxis ("Horizontal") < 0) {
						currentAngle = Angle.DOWNLEFT;
					}
					//down
					else {
						currentAngle = Angle.DOWN;
					}
				} else {
					//Right
					if (Input.GetAxis ("Horizontal") > 0) {
						currentAngle = Angle.RIGHT;
					}
					//Left
					else if (Input.GetAxis ("Horizontal") < 0) {
						currentAngle = Angle.LEFT;
					}
				}

				if(Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0){
					//Todo - sumir com a setinha
				}
			}
		}

		private void FixedUpdate(){
			switch (currentAngle) {
			case Angle.UP: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 0);
				break;
			case Angle.UPLEFT: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 315);
				break;
			case Angle.UPRIGHT: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 45);
				break;
			case Angle.DOWN: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 180);
				break;
			case Angle.DOWNRIGHT: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 135);
				break;
			case Angle.DOWNLEFT: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 195);
				break;
			case Angle.LEFT: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 270);
				break;
			case Angle.RIGHT: 
				tr.eulerAngles = new Vector3 (tr.eulerAngles.x, tr.eulerAngles.y, 90);
				break;
			}
		}
	}
}