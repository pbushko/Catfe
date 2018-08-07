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
		CatfePlayerScript.script.MakeNewRestaurant(type);	
	}
}
