using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRestaurantCreation : MonoBehaviour {

	public RestaurantType type;
	public Text text;
	public Sprite sprite;

	public Cafe cafe;

	void Start()
	{
		text.text = "" + type;
	}

	public void OnClick()
	{
		PlayerData.playerData.restaurants.Add(new RestaurantData(type));
		Debug.Log("You've made a new restaurant of type " + type);
		Debug.Log("Click where you want your restaurant to go.");
		cafe.SetNewRestaurantArea(sprite);
		
	}
}
