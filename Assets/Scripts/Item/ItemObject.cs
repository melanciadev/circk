using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Circk{
	public class ItemObject : MonoBehaviour {

		[Header("Components")]
		private ItemManager itemManager;
		private Transform tr;

		[Header("Animation")]
		private Tweener tween;

		[Header("States")]
		public ItemManager.ItemType itemType;
		public bool ableToPick;

		[Header("Values")]
		private int maxUse;
		private int currentUse;
		private bool objTaken = false;

		private void Start(){
			//start the componets
			tr = GetComponent<Transform>();
			itemManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemManager>();

			//Set the animations
			Vector3 finalVector = new Vector3(1, 1, 1); 
			tween = tr.DOScale (finalVector, 0.5f);
		}

		//Called by the ItemManager
		private void InitiateItem(ItemManager.ItemType itemType){
			//Save the item type
			this.itemType = itemType;
			ableToPick = false;

			ShowAnimation();
		}

		private void OnTriggerEnter2D(Collider2D collider){
			if (ableToPick && collider.gameObject.tag == "Player") {
				if(!objTaken){
					objTaken = true;
					collider.gameObject.GetComponent<PlayerController>().TakeItem(itemType, itemManager.GetMaxUsesItem(itemType));
					PickAnimation();
				}

			}
		}

		private void ShowAnimation(){
			//Animation showing
			tween.Play().OnComplete(() => {
				ableToPick = true;
			});
		}

		private void PickAnimation(){
			tween = tr.DOScale(Vector3.zero, 0.2f).OnComplete(() => {
				Destroy(gameObject);
			});
		}
	}
}