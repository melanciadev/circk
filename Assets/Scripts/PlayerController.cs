using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{
	
	public class PlayerController : MonoBehaviour {

		[Header("Values")]
		public float speed;
		public float delayAfterHit = 0.5f;

		[Header("States")]
		protected bool moveLocked = false;

		[Header("Components")]
		private Transform tr;
		private Rigidbody2D rb;
		private Animator an;
		private SpriteRenderer sr;
		private GameManager gm;
		private ItemManager im;

		[Header("Direction Vector")]
		public Transform directionTransform;
		public Transform originPosition;

		[Header("Item")]
		public GameObject itemHolderSprite;
		public GameObject itemSprite;
		private Tweener tween;
		private ItemManager.ItemType currentItem;
		private bool haveItem;
		private bool canUseItem;
		private int maxUsesOfItem;
		private int currentUseOfItem;

		private void Awake(){
			//Init Components
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody2D>();
			an = GetComponent<Animator>();
			sr = GetComponent<SpriteRenderer>();
			gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
			im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemManager>();
		}

		private void Update(){
			//Only moves if the game is on
			if(gm.CurrentGameState == GameManager.GameState.GAME){
				Move ();

				if(haveItem && canUseItem && Input.GetKeyDown(KeyCode.Space)){
					UseItem();
				}
			}
		}

		public void Move (){
			//Dont move if player has just been pushed
			if(!moveLocked){
				//Get the input
				float horizontalValue = Input.GetAxis("Horizontal") * speed;
				float verticalValue = Input.GetAxis("Vertical") * speed * 0.75f;

				var scaleAux = tr.localScale;

				//Walk
				an.SetBool("Walking", (Mathf.Abs(verticalValue) > 0f) || Mathf.Abs(horizontalValue) > 0f);
				tr.Translate (horizontalValue * Time.deltaTime, verticalValue * Time.deltaTime, 0);
			}
		}
   	
		private void OnCollisionEnter2D(Collision2D collision){
			//Gameover when collide to the border
			if (collision.gameObject.tag == "EdgeDeath")
				Debug.Log ("Fim de Jogo");
		}
		private void OnTriggerEnter2D(Collider2D collider){
			if (collider.gameObject.tag == "EdgeWarning") {
				an.SetBool ("Balance", true);
				Debug.Log ("Warning");
				GameManager.Instance.CrowdCheerAnimation ();
			}
		}
		private void OnTriggerExit2D(Collider2D collider){
			if (collider.gameObject.tag == "EdgeWarning") {
				an.SetBool ("Balance", false);
			}
		}

		//When the player is hit - called by enemy
		public void Impact(Vector3 orientation, float force){

			an.SetTrigger ("Hit");

			//Only works if the movementLoch is false
			if (!moveLocked) {
				rb.AddForce (force * orientation, ForceMode2D.Impulse);

				moveLocked = true;

				StartCoroutine (WaitAndCall (delayAfterHit, () => {
					moveLocked = false;
				}));
			}
		}

		//Wait to restart movement after hit
		private IEnumerator WaitAndCall(float waitTime, Action callback){
			//Set the moventLock to false after the delay
			yield return new WaitForSeconds(waitTime);
			if (callback != null)
				callback ();
		}

		//Called by the ItemObject - Show on the playerHead
		public void TakeItem(ItemManager.ItemType itemType, int maxUses){
			//Set the item visiable and with the right sprite
			if(!itemHolderSprite.activeSelf){
				itemHolderSprite.SetActive(true);
			}
			itemSprite.GetComponent<SpriteRenderer>().sprite = im.GetItemSprite(itemType);

				
			//Animate it to show and them to keep moving slightly
			itemHolderSprite.transform.DOScale(new Vector3(2.4f, 2.4f, 2.4f), 0.25f).OnComplete(() => {
				itemSprite.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f).OnComplete(() => {
					//TODO - keep moving slightly

				});
			});

			//set the currentItem
			currentItem = itemType;
			maxUsesOfItem = maxUses;
			currentUseOfItem = 0;

			//Set it is usign one
			haveItem = true;
			canUseItem = true;

		}

		private void UseItem(){
			canUseItem = false;

			//If still have the item
			if(currentUseOfItem < maxUsesOfItem){

				//Use it
				im.UseItem(currentItem, originPosition);
				currentUseOfItem++;
			}
		}
	}
}