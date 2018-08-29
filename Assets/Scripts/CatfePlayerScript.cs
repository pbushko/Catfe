using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum States {InsideRestaurant, CityMap, InventoryMenu, RecruitingMenu, InvToRestaurant, NewRestaurantMenu};

public class CatfePlayerScript : MonoBehaviour {

	public static CatfePlayerScript script;

	//used to make new restaurants
	public GameObject newRestaurantPrefab;
	public GameObject newRestaurantCanvas;
	public GameObject restaurantLocationsParent;
	private List<Vector3> restaurantLocations;

	//the city and everything in it
	public GameObject city;
	//the inside of a restaurant
	public GameObject insideRestaurant;
	//the buttons that will keep moving you in and out of the restaurant
	public GameObject buttons;

	//used to buy recipes and decor
	public GameObject storeConfirmation;
	public GameObject storeCanvas;
	public Recipe recipeToPurchase;
	public DecorationData decorToPurchase;
	public GameObject decorInfoPrefab;

	//the different canvases to manage cats
	public GameObject chefRecruitment;
	public GameObject waiterRecruitment;
	public GameObject catInventory;
	//to pull up the inventory of the restaurant
	public GameObject restaurantPanel;
	public RestaurantInventoryPanel invPanelScript;

	//the current state of the program
	public States currentState;

	//to load in the restaurant's objects when you go in it
	public GameObject waiterSpots;
	public GameObject chefSpot;

	public List<Sprite> restaurantSprites;
	public List<string> restaurantSpriteNames;

	private Vector3 location;

	public GameObject minigameItems;

	// Use this for initialization
	void Start () 
	{
		script = this;
		restaurantSprites = new List<Sprite>(Resources.LoadAll<Sprite>("RestaurantOutsides"));
		restaurantSprites = restaurantSprites.FindAll(FindFronts);
		restaurantSpriteNames = new List<string>();
		foreach (Sprite s in restaurantSprites)
		{
			restaurantSpriteNames.Add(s.name);
		}
		//getting all of the locations that the restaurant can be in
		restaurantLocations = new List<Vector3>();
		for (int i = 0; i < restaurantLocationsParent.transform.childCount; i++)
		{
			restaurantLocations.Add(restaurantLocationsParent.transform.GetChild(i).position);
		}
		SetUpRestaurants();
		currentState = States.CityMap;
	}

	private static bool FindFronts(Sprite s)
	{
		return s.name.Contains("Front_");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!IsCanvasActive() && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.Raycast(mousePos, Vector2.up).collider;
            if (hit)
            {
				if (currentState == States.CityMap && hit.tag == "Restaurant")
                {
					//checking the inventory of the restaurant you are clicking on
					PlayerData.playerData.activeRestaurant = hit.gameObject;
					restaurantPanel.SetActive(true);
					RestaurantData r = PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().data;
					invPanelScript.SetChefs(r.chefs);
					invPanelScript.SetWaiters(r.waiters);
					PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().CollectMoney();
                }
				else if (currentState == States.CityMap && hit.tag == "RestaurantSpace")
                {
					//make a new restaurant when you click on an empty space
                    newRestaurantCanvas.SetActive(true);
					location = hit.transform.position;
					hit.enabled = false;
                }
				else if (hit.tag == "Recruit Chefs")
				{
					chefRecruitment.SetActive(true);
				}
				else if (hit.tag == "Recruit Waiters")
				{
					waiterRecruitment.SetActive(true);
				}
				else if (hit.tag == "Cat Inventory")
				{
					catInventory.SetActive(true);
				}
				else if (hit.tag == "Location Switch")
				{
					city.SetActive(!city.activeSelf);
					insideRestaurant.SetActive(!insideRestaurant.activeSelf);
					currentState = States.CityMap;
				}
				//start the minigame if you click on the chef
				else if (hit.tag == "Chef Cat")
				{
					minigameItems.SetActive(true);
					insideRestaurant.SetActive(false);
					buttons.SetActive(false);
				}
				//opening the store for decor and recipes
				else if (hit.tag == "Store")
				{
					storeCanvas.SetActive(true);
				}
				else if (hit.tag == "Decoration Space")
				{
					catInventory.SetActive(true);
					CatInventory.catInv.ReadyAddToRestaurant();
				}
			}
		}
	}

	public void EnterRestaurant()
	{
		city.SetActive(false);
		insideRestaurant.SetActive(true);
		buttons.SetActive(true);
		//loading in the waiters in the back
		List<WaiterData> ws = PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().data.waiters;
		for (int i = 0; i < waiterSpots.transform.childCount; i++)
		{
			GameObject child = waiterSpots.transform.GetChild(i).gameObject;
			if (i >= ws.Count)
			{
				child.SetActive(false);
			}
			else
			{
				child.SetActive(true);
				child.GetComponent<Waiter>().RefreshWaiter(ws[i]);
			}
		}
		//loading in a chef
		List<ChefData> c = PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().data.chefs;
		if (c.Count > 0)
		{
			chefSpot.SetActive(true);
			chefSpot.GetComponent<Chef>().RefreshChef(c[0]);
		}
		else
		{
			chefSpot.SetActive(false);
		}
		currentState = States.InsideRestaurant;
	}

	public bool IsCanvasActive()
	{
		return chefRecruitment.activeSelf || waiterRecruitment.activeSelf || catInventory.activeSelf || restaurantPanel.activeSelf || storeCanvas.activeSelf;
	}

	public void MakeNewRestaurant(RestaurantType r)
	{
		newRestaurantCanvas.SetActive(false);
		Debug.Log("You've made a new restaurant of type " + r);
		GameObject newRest = (GameObject)Instantiate(newRestaurantPrefab);
		newRest.transform.position = location;
		//getting the correct sprite
		newRest.GetComponent<Restaurant>().title.sprite = GetRestaurantOutside(r);
		//saving the location that the restaurant is in
		int loc = -1;
		int i = 0;
		while (loc == -1)
		{
			if (restaurantLocations[i] == location)
			{
				loc = i;
			}
			i++;
		}
		newRest.GetComponent<Restaurant>().data.location = loc;
		newRest.GetComponent<Restaurant>().data.type = r;
		newRest.transform.SetParent(restaurantLocationsParent.transform);
		PlayerData.playerData.restaurants.Add(newRest.GetComponent<Restaurant>().data);
		currentState = States.CityMap;
	}

	//loading in all the restaurants we own
	private void SetUpRestaurants()
	{
		foreach(RestaurantData r in PlayerData.playerData.restaurants)
		{
			GameObject newRest = (GameObject)Instantiate(newRestaurantPrefab);
			newRest.GetComponent<Restaurant>().title.sprite = GetRestaurantOutside(r.type);
			//setting the data of this restaurant to be the saved data.
			newRest.GetComponent<Restaurant>().data = r;
			newRest.transform.SetParent(city.transform);
			newRest.transform.position = new Vector3(restaurantLocations[r.location].x, restaurantLocations[r.location].y, restaurantLocations[r.location].z - 0.2f);
			//r.PrintAll();
		}
	}

	public Sprite GetRestaurantOutside(RestaurantType r)
	{
		switch(r)
		{
			case RestaurantType.Catfe:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Catfe")];
				break;
			case RestaurantType.Italian:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Pizza")];
				break;
			case RestaurantType.Sandwich:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Catfe")];
				break;
			case RestaurantType.Burger:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Burger")];
				break;
			case RestaurantType.Asian:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Catfe")];
				break;
			case RestaurantType.Indian:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Catfe")];
				break;
			case RestaurantType.Bakery:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Catfe")];
				break;
			default:
				return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + "Catfe")];
				break;
		}
	}

	//remove the cat from the inventory and into the restaurant's workers
	public void MoveCatToRestaurant(ChefData c, WaiterData w)
	{
		Restaurant r = PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>();
		if (c != null)
		{
			r.data.chefs.Add(c);
			invPanelScript.AddChef(c);
		}
		if (w != null)
		{
			r.data.waiters.Add(w);
			invPanelScript.AddWaiter(w);
		}
		catInventory.SetActive(false);
	}

	public void MoveCatToInv(ChefData c, WaiterData w)
	{
		Restaurant r = PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>();
		if (c != null)
		{
			r.data.chefs.Remove(c);
			PlayerData.playerData.chefs.Add(c);
		}
		if (w != null)
		{
			r.data.waiters.Remove(w);
			PlayerData.playerData.waiters.Add(w);
		}
		
	}

	public void ReadyPurchase(DecorationData d, Recipe r)
	{
		storeConfirmation.SetActive(true);
		if (d != null)
		{
			decorToPurchase = d;
			storeConfirmation.GetComponent<StorePurchaseConfirmer>().UpdateText(d);
		}
		
	}

	//will be called by the storPurchaseConfirmer
	public void PurchaseItem(bool b)
	{
		if (b)
		{
			//add the decor to the user's inventory
			if (decorToPurchase != null)
			{
				int dup = FindDuplicateIndex();
				if (dup != -1)
				{
					//Debug.Log("duplicate purchased");
					CatInventory.catInv.AddToDecorCount(dup);
				}
				//if the decoration is not in your inventory yet
				else
				{
					PlayerData.playerData.purchasedDecor.Add(decorToPurchase);
					CatInventory.catInv.AddDecor(decorToPurchase);
				}
			}
		}
		//clear the possible items it's purchasing; this is done whether or not the item is purchased
		decorToPurchase = null;
		recipeToPurchase = null;
	}

	//helper function to find duplicate decorations for a purchase; returns the index of the duplicate
	private int FindDuplicateIndex()
	{
		List<DecorationData> pur = PlayerData.playerData.purchasedDecor;
		for (int i = 0; i < pur.Count; i++)
		{
			if(pur[i].id == decorToPurchase.id)
			{
				return i;
			}
		}
		//if this is reached, there was no match
		return -1;
	}

}
