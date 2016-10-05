using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Circk{
	public class ItemObject : MonoBehaviour {

		public static List<ItemObject> items = null;

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
			tr.localScale = Vector3.zero;
			tween = tr.DOScale (Vector3.one, 0.5f);

			if (items == null) {
				items = new List<ItemObject>();
			}
			items.Add(this);
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
					AudioStuff.PlaySound("item");
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
				items.Remove(this);
				Destroy(gameObject);
			});
		}
	}
}