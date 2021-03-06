﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

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

	//used to buy recipes and decor
	public GameObject storeConfirmation;
	public GameObject storeCanvas;
	public GameObject restWallDecorPanel;
	public GameObject restTableDecorPanel;
	public GameObject restFloorDecorPanel;
	public GameObject restDecorPanel;
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

	private int fingerID = -1;

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
	}
	
	private void Awake()
	{
		#if !UNITY_EDITOR
			fingerID = 0; 
		#endif
	}

	private static bool FindFronts(Sprite s)
	{
		return s.name.Contains("Front_");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!EventSystem.current.IsPointerOverGameObject(fingerID) && Input.GetMouseButtonDown(0))
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
				//make a new restaurant when you click on an empty space
				else if (hit.tag == "RestaurantSpace")
                {
                    newRestaurantCanvas.SetActive(true);
					location = hit.transform.position;
					hit.enabled = false;
                }
				//start the minigame if you click on the chef
				else if (hit.tag == "Chef Cat")
				{
					minigameItems.SetActive(true);
					insideRestaurant.SetActive(false);
				}
				// let player place a decoration if they click on a decor space
				else if (hit.tag == "Wall Decoration Space")
				{
					Debug.Log("wall space");
					Debug.Log(hit.GetComponent<Decoration>().data.ToString());
					restWallDecorPanel.SetActive(true);
					restTableDecorPanel.SetActive(false);
					restFloorDecorPanel.SetActive(false);
					restDecorPanel.SetActive(true);
					CatInventory.catInv.ReadyAddToRestaurant();
				}
				else if (hit.tag == "Table Decoration Space")
				{
					Debug.Log("Table space");
					Debug.Log(hit.GetComponent<Decoration>().data.ToString());
					restWallDecorPanel.SetActive(false);
					restTableDecorPanel.SetActive(true);
					restFloorDecorPanel.SetActive(false);
					restDecorPanel.SetActive(true);
					CatInventory.catInv.ReadyAddToRestaurant();
					//CatInventory.catInv.StartDecorSpacePurchased();
				}
				else if (hit.tag == "Floor Decoration Space")
				{
					Debug.Log("floor space");
					Debug.Log(hit.GetComponent<Decoration>().data.ToString());
					restWallDecorPanel.SetActive(false);
					restTableDecorPanel.SetActive(false);
					restFloorDecorPanel.SetActive(true);
					restDecorPanel.SetActive(true);
					CatInventory.catInv.ReadyAddToRestaurant();
					//CatInventory.catInv.StartDecorSpacePurchased();
				}
				// lets player leave restaurant if they click door
				else if (hit.tag == "Exit Door")
				{
					insideRestaurant.SetActive(false);
					city.SetActive(true);
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
		//loading in the waiters in the back
		/*
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
		//List<ChefData> c = activeRestaurant.data.chefs;
		if (c.Count > 0)
		{
			chefSpot.SetActive(true);
			//chefSpot.GetComponent<Chef>().RefreshChef(c[0]);
		}
		else
		{
			//chefSpot.SetActive(false);
		}
		*/
		SetDecorationSprites();
	}

	public void MakeNewRestaurant(RestaurantType r)
	{
		newRestaurantCanvas.SetActive(false);
		Debug.Log("You've made a new restaurant of type " + r);
		GameObject newRest = (GameObject)Instantiate(newRestaurantPrefab);
		newRest.transform.position = location;
		//getting the correct sprite
		//newRest.GetComponent<Restaurant>().title.sprite = GetRestaurantOutside(r);
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
		//PlayerData.playerData.restaurants.Add(newRest.GetComponent<Restaurant>().data);

		//adding in the restaurant into the user's inventory
		PurchaseItemRequest request = new PurchaseItemRequest();
        request.ItemId = "catfe";
        request.CatalogVersion = "Items";
        request.VirtualCurrency = "NM";
        request.Price = 0;
		PlayFabClientAPI.PurchaseItem(request, result => {

        }, error => {Debug.LogError(error.ErrorMessage);});
	}

	//loading in all the restaurants we own
	public void SetUpRestaurants(List<RestaurantData> rs)
	{
		foreach(RestaurantData r in rs)
		{
			GameObject newRest = (GameObject)Instantiate(newRestaurantPrefab);
			//newRest.GetComponent<Restaurant>().title.sprite = GetRestaurantOutside(r.type);
			//setting the data of this restaurant to be the saved data.
			newRest.GetComponent<Restaurant>().data = r;
			newRest.transform.SetParent(city.transform);
			newRest.transform.position = new Vector3(restaurantLocations[r.location].x, restaurantLocations[r.location].y, restaurantLocations[r.location].z);
			//r.PrintAll();
			//removing the forsale area if there is a restaurant there
			restaurantLocationsParent.transform.GetChild(r.location).gameObject.SetActive(false);
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
			storeConfirmation.GetComponent<ConfirmationPopup>().SetDecorationText(d);
			
		}
		if (r != null)
		{
			recipeToPurchase = r;
			//storeConfirmation.GetComponent<ConfirmationPopup>().UpdateText(null, r);
		}
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
