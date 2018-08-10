﻿using System.Collections;
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

	public List<Sprite> restaurantSprites;
	public List<string> restaurantSpriteNames;

	private Vector3 location;

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
					CatInventory.catInv.ResetChefInv();
					CatInventory.catInv.ResetWaiterInv();
				}
				else if (hit.tag == "Location Switch")
				{
					city.SetActive(!city.activeSelf);
					insideRestaurant.SetActive(!insideRestaurant.activeSelf);
					currentState = States.CityMap;
				}
			}
		}
	}

	public void EnterRestaurant()
	{
		city.SetActive(false);
		insideRestaurant.SetActive(true);
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
		currentState = States.InsideRestaurant;
	}

	public bool IsCanvasActive()
	{
		return chefRecruitment.activeSelf || waiterRecruitment.activeSelf || catInventory.activeSelf || restaurantPanel.activeSelf;
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
			PlayerData.playerData.chefs.Remove(c);
			invPanelScript.AddChef(c);
		}
		if (w != null)
		{
			r.data.waiters.Add(w);
			PlayerData.playerData.waiters.Remove(w);
			invPanelScript.AddWaiter(w);
		}
	}

}
