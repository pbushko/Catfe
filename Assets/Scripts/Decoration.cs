using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decoration : MonoBehaviour {

	public DecorationData data;

	public Text name;
	public Text starLevel;
	public Text atmosphere;
	public Text cost;
	public Text numInInventoryText;

	//the image renderer of the decoration to go on the panel
	public Image sprite;

	public GameObject addToRestaurantButton;

	// Use this for initialization
	void Start () {
		//set up the info to appear in the shop menu
		if (data != null)
		{
			ResetData(data);
		}
	}

	public void ResetData(DecorationData newData)
	{
		data = newData;
		if (name != null)
		{
			name.text = newData.name;
		}
		if(cost != null)
		{
			cost.text = "" + newData.cost;
		}
		if(starLevel != null)
		{
			starLevel.text = "StarLevel: " + newData.starLevel;
		}
		if(atmosphere != null)
		{
			atmosphere.text = "Atmosphere: " + newData.atmosphere;
		}
		if(numInInventoryText != null)
		{
			numInInventoryText.text = "x" + newData.numInInventory;
		}
		if(sprite != null)
		{
			sprite.sprite = CatfePlayerScript.script.GetDecorationSprite(newData.sprite);
		}
	}

	public void AddSameDecorationToInv()
	{
		data.numInInventory++;
		numInInventoryText.text = "x" + data.numInInventory;
	}

	public void RemoveDecorationFromInv()
	{
		data.numInInventory--;
		numInInventoryText.text = "x" + data.numInInventory;
		CatInventory.catInv.RemoveDecoration(gameObject);
	}

	public void MoveDecorationToRestaurant()
	{
		//will only remove from inv if the transfer was successful
		if (CatfePlayerScript.script.activeRestaurant.AddDecoration(data))
		{
			RemoveDecorationFromInv();
			
		}
	}

	public void SetAddToRestaurant()
	{
		addToRestaurantButton.SetActive(true);
	}

	public void RemoveAddToRestaurant()
	{
		addToRestaurantButton.SetActive(false);
	}

	public void OnCLick()
	{
		CatfePlayerScript.script.ReadyPurchase(data, null);
	}
}
