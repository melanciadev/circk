using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Circk{
	
	public class ItemManager : MonoBehaviour {

		public enum ItemType{
			BALL,
			LION,
			GUN
		}

		[Header("GameManager")]
		private GameManager gameManager; 

		[Header("Itens")]
		public GameObject itemBall;
		public GameObject itemLion;
		public GameObject itemGun;

		[Header("Sprite Itens")]
		public Sprite itemBallSprite;
		public Sprite itemLionSprite;
		public Sprite itemGunSprite;

		[Header("UsingItens")]
		public GameObject[] itemUseBallSprite;
		public GameObject itemUseLionSprite;
		public GameObject itemUseGunSprite;

		[Header("Item Values")]
		public int maxUsesItemBall;
		public int maxUsesItemLion;
		public int maxUsesItemGun;

		[Header("Values")]
		public float timeBetweenspawn;
		public float maxItens;

		[Header("Spawn Time")]
		private float timeSinceLastSpawn;
		private int itemInGame;
		private GameObject[] enemyArray;

		[Header("Spawn Places")]
		public GameObject[] itemSpawnArea;


		//TODO - TEMP
		[Header("TEMP")]
		public bool spawn = false;
		public GameObject item;
		public Transform place;


		private void Awake(){
			gameManager = GetComponent<GameManager>();
		}

		private void FixedUpdate(){
			//Only works if in the game
			if(gameManager.CurrentGameState == GameManager.GameState.GAME){
				//TODO - Spawn Itens

				//TODO - TEMP
				if(spawn){
					spawn = false;
					SpawnItem (place, item);
				}
			}
		}

		private void SpawnItem(Transform spawnArea, GameObject itemGameObject){
			Vector2 pos;
			if (!RandomPosition(out pos)) return;

			//Instantiate
			GameObject itemInstantiate = (GameObject)Instantiate (itemGameObject, new Vector3(pos.x,pos.y,.1f), Quaternion.identity);

			//Get the ItemObject Component
			ItemObject itemScript = itemInstantiate.GetComponent<ItemObject>();
		}

		public Sprite GetItemSprite(ItemType itemType){
			if(itemType == ItemType.BALL){
				return itemBallSprite;
			}
			if(itemType == ItemType.GUN){
				return itemGunSprite;
			}
			if(itemType == ItemType.LION){
				return itemLionSprite;
			}
			return null;
		}
		public int GetMaxUsesItem(ItemType itemType){
			if(itemType == ItemType.BALL){
				return maxUsesItemBall;
			}
			if(itemType == ItemType.GUN){
				return maxUsesItemGun;
			}
			if(itemType == ItemType.LION){
				return maxUsesItemLion;
			}
			return 0;
		}
	
		public void UseItem(ItemType itemType, Transform origin){
			if(itemType == ItemType.BALL){
				UseItemBall(origin);
			}
			if(itemType == ItemType.GUN){
				UseItemLion(origin);
			}
			if(itemType == ItemType.LION){
				UseItemGun(origin);
			}
		}
		private void UseItemBall(Transform origin){
			print ("Ball -- " + origin.transform.rotation);

			//Instantiate the ball on the player origin
			Instantiate(itemUseBallSprite[0], origin.transform.position,origin.transform.rotation);
		}
		private void UseItemLion(Transform origin){
			print ("Lion -- " + origin.transform.rotation);

			//Instantiate the Lion on the player origin
			Instantiate(itemUseLionSprite, origin.transform.position,origin.transform.rotation);
		}
		private void UseItemGun(Transform origin){
			print ("Gun -- " + origin.transform.rotation);

			//Instantiate the Gun on the player origin
			Instantiate(itemUseGunSprite, origin.transform.position,origin.transform.rotation);
		}

		HashSet<int> set = null;
		List<int> list = null;
		const int stageWidth = 24;
		const int stageHeight = 9;
		const float stageProp = (float)stageWidth/stageHeight;
		const float stagePropInv = (float)stageHeight/stageWidth;
		const int stageRadius = stageWidth/2;
		const int stageSqrRadius = stageRadius*stageRadius;
		private bool RandomPosition(out Vector2 pos) {
			if (set == null) {
				set = new HashSet<int>();
				list = new List<int>();
			}
			for (int x = 0; x < stageWidth; x++) {
				for (int y = 0; y < stageWidth; y++) {
					int xx = x-stageRadius;
					int yy = y-stageRadius;
					if (xx*xx+yy*yy < stageSqrRadius) {
						set.Add(x+y*stageWidth);
						list.Add(x+y*stageWidth);
					}
				}
			}
			if (PlayerController.me != null) {
				RandomPositionCheckTransform(PlayerController.me.transform.position,true);
			}
			if (EnemyBase.enemies != null) {
				for (int a = 0; a < EnemyBase.enemies.Count; a++) {
					RandomPositionCheckTransform(EnemyBase.enemies[a].transform.position,false);
				}
			}
			if (ItemObject.items != null) {
				for (int a = 0; a < ItemObject.items.Count; a++) {
					RandomPositionCheckTransform(ItemObject.items[a].transform.position,false);
				}
			}
			if (set.Count > 0) {
				foreach (var i in set) {
					list.Add(i);
				}
				set.Clear();
			}
			if (list.Count == 0) {
				pos = Vector2.zero;
				return false;
			}
			int p = list[(int)(Random.value*list.Count)%list.Count];
			pos = new Vector2(p%stageWidth-stageRadius+.5f,(p/stageWidth-stageRadius)*stagePropInv+.5f);
			return true;
		}
		private void RandomPositionCheckTransform(Vector2 pos,bool isPlayer) {
			int x = (int)(pos.x+stageRadius);
			if (x < 0 || x >= stageWidth) return;
			int y = (int)((pos.y-.7f)*stageProp+stageRadius);
			if (y < 0 || y >= stageWidth) return;
			set.Remove(x+y*stageRadius);
			if (isPlayer) {
				RandomPositionRemoveFromList(x-1,y-1);
				RandomPositionRemoveFromList(x,y-1);
				RandomPositionRemoveFromList(x+1,y-1);
				RandomPositionRemoveFromList(x-1,y);
				RandomPositionRemoveFromList(x,y);
				RandomPositionRemoveFromList(x+1,y);
				RandomPositionRemoveFromList(x-1,y+1);
				RandomPositionRemoveFromList(x,y+1);
				RandomPositionRemoveFromList(x+1,y+1);
			}
		}
		private void RandomPositionRemoveFromList(int x,int y) {
			if (x >= 0 && x < stageWidth && y >= 0 && y < stageWidth) {
				list.Remove(x+y*stageRadius);
			}
		}
	}
}