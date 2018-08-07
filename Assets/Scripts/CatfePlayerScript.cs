using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatfePlayerScript : MonoBehaviour {

	public static CatfePlayerScript script;

	public GameObject newRestaurantPrefab;
	public GameObject newRestaurantCanvas;
	public GameObject restaurantLocationsParent;
	private List<Vector3> restaurantLocations;

	//to pull up the inventory of the restaurant
	public GameObject restaurantPanel;

	public List<Sprite> restaurantSprites;
	public List<string> restaurantSpriteNames;

	private Vector3 location;

	private bool choosing;

	// Use this for initialization
	void Start () {
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
		choosing = false;
	}

	private static bool FindFronts(Sprite s)
	{
		return s.name.Contains("Front_");
	}
	
	// Update is called once per frame
	void Update () {
		if (!choosing && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.up);
            if (hit.collider)
            {
				if (hit.collider.tag == "RestaurantSpace")
                {
					//make a new restaurant when you click on an empty space
                    newRestaurantCanvas.SetActive(true);
					choosing = true;
					location = hit.collider.transform.position;
                }
				if (hit.collider.tag == "Restaurant")
                {
					//checking the inventory of the restaurant you are clicking on
					restaurantPanel.SetActive(true);
                }
			}
		}
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
		choosing = false;
	}

	//loading in all the restaurants we own
	private void SetUpRestaurants()
	{
		foreach(RestaurantData r in PlayerData.playerData.restaurants)
		{
			GameObject newRest = (GameObject)Instantiate(newRestaurantPrefab);
			newRest.GetComponent<Restaurant>().title.sprite = GetRestaurantOutside(r.type);
			newRest.transform.position = restaurantLocations[r.location];
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

}
