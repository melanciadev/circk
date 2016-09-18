using UnityEngine;
using System.Collections;

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

		[Header("Sprite usingItens")]
		public Sprite[] itemUseBallSprite;
		public Sprite itemUseLionSprite;
		public Sprite itemUseGunSprite;

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
			//Instantiate
			GameObject itemInstantiate = (GameObject)Instantiate (itemGameObject, spawnArea.position, spawnArea.rotation);

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
			Instantiate(itemBall, origin.transform.position,origin.transform.rotation);
		}
		private void UseItemLion(Transform origin){
			print ("Lion -- " + origin.transform.rotation);
		}
		private void UseItemGun(Transform origin){
			print ("Gun -- " + origin.transform.rotation);
		}
	}
}