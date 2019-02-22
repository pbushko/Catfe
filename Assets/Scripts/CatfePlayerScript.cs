using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
	public GameObject catInventory;
	//to pull up the inventory of the restaurant
	public GameObject restaurantPanel;
	public RestaurantInventoryPanel invPanelScript;

	//to load in the restaurant's objects when you go in it
	public GameObject waiterSpots;
	public GameObject chefSpot;

	public List<Sprite> restaurantSprites;
	public List<string> restaurantSpriteNames;

	public List<Sprite> decorationSprites;
	public List<string> decorationSpriteNames;
	public GameObject decorationLocations;

	private Vector3 location;

	public GameObject minigameItems;

	public Restaurant activeRestaurant;

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

		decorationSprites = new List<Sprite>(Resources.LoadAll<Sprite>("catToys"));
		decorationSpriteNames = new List<string>();
		foreach (Sprite s in decorationSprites)
		{
			decorationSpriteNames.Add(s.name);
		}

		//getting all of the locations that the restaurant can be in
		restaurantLocations = new List<Vector3>();
		for (int i = 0; i < restaurantLocationsParent.transform.childCount; i++)
		{
			restaurantLocations.Add(restaurantLocationsParent.transform.GetChild(i).position);
		}
		SetUpRestaurants();
	}

	private static bool FindFronts(Sprite s)
	{
		return s.name.Contains("Front_");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.Raycast(mousePos, Vector2.up).collider;
            if (hit)
            {
				if (hit.tag == "Restaurant")
                {
					//checking the inventory of the restaurant you are clicking on
					activeRestaurant = hit.gameObject.GetComponent<Restaurant>();
					//if you are collecting money, don't open the cnavas for the restaurant
					if (activeRestaurant.CollectMoney() == 0) 
					{
						restaurantPanel.SetActive(true);
						invPanelScript.SetChefs(activeRestaurant.data.chefs);
						invPanelScript.SetWaiters(activeRestaurant.data.waiters);
					}
					
                }
				else if (hit.tag == "RestaurantSpace")
                {
					//make a new restaurant when you click on an empty space
                    newRestaurantCanvas.SetActive(true);
					location = hit.transform.position;
					hit.enabled = false;
                }
				//start the minigame if you click on the chef
				else if (hit.tag == "Chef Cat")
				{
					minigameItems.SetActive(true);
					insideRestaurant.SetActive(false);
					buttons.SetActive(false);
				}
				else if (hit.tag == "Decoration Space")
				{
					Debug.Log(hit.GetComponent<Decoration>().data.ToString());
					catInventory.SetActive(true);
					CatInventory.catInv.ReadyAddToRestaurant();
				}
				else if (hit.tag == "Recipe Space")
				{

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
		List<WaiterData> ws = activeRestaurant.data.waiters;
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
		List<ChefData> c = activeRestaurant.data.chefs;
		if (c.Count > 0)
		{
			chefSpot.SetActive(true);
			chefSpot.GetComponent<Chef>().RefreshChef(c[0]);
		}
		else
		{
			chefSpot.SetActive(false);
		}
		SetDecorationSprites();
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
		string typeName = "";
		//only some of these have been implemented so far
		switch(r)
		{
			case RestaurantType.Catfe:
				typeName = "Catfe";
				break;
			case RestaurantType.Italian:
				typeName = "Pizza";
				break;
			case RestaurantType.Burger:
				typeName = "Burger";
				break;
			default:
				typeName = "Catfe";
				break;
		}
		return restaurantSprites[restaurantSpriteNames.IndexOf(Variables.RESTAURANT_SPRITE_STRING + typeName)];
	}

	//remove the cat from the inventory and into the restaurant's workers
	public void MoveCatToRestaurant(ChefData c, WaiterData w)
	{
		if (c != null)
		{
			activeRestaurant.data.chefs.Add(c);
			invPanelScript.AddChef(c);
		}
		if (w != null)
		{
			activeRestaurant.data.waiters.Add(w);
			invPanelScript.AddWaiter(w);
		}
		catInventory.SetActive(false);
	}

	public void MoveCatToInv(ChefData c, WaiterData w)
	{
		if (c != null)
		{
			activeRestaurant.data.chefs.Remove(c);
			PlayerData.playerData.chefs.Add(c);
		}
		if (w != null)
		{
			activeRestaurant.data.waiters.Remove(w);
			PlayerData.playerData.waiters.Add(w);
		}
		
	}

	public void ReadyPurchase(DecorationData d, Recipe r)
	{
		storeConfirmation.SetActive(true);
		if (d != null)
		{
			decorToPurchase = d;
			storeConfirmation.GetComponent<StorePurchaseConfirmer>().UpdateText(d, null);
		}
		if (r != null)
		{
			recipeToPurchase = r;
			storeConfirmation.GetComponent<StorePurchaseConfirmer>().UpdateText(null, r);
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
			if (recipeToPurchase != null)
			{

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

	public void SetDecorationSprites()
	{
		List<DecorationData> decorations = activeRestaurant.data.decor;
		for (int i = 0; i < decorations.Count; i++)
		{
			//find the sprite in the list
			for (int j = 0; j < decorationSpriteNames.Count; j++)
			{
				//if the sprite name is found, insert the sprite into the next slot
				if (decorationSpriteNames[j] == decorations[i].sprite)
				{
					decorationLocations.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = decorationSprites[j];
					j = decorationSpriteNames.Count;
				}
				//if the sprite was not found, just set it to a different one
				else if (j == decorationSpriteNames.Count - 1)
				{
					decorationLocations.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = decorationSprites[1];
				}
			}
		}
	}

	public Sprite GetDecorationSprite(string s)
	{
		return decorationSprites[decorationSpriteNames.IndexOf(s)];
	}

}
