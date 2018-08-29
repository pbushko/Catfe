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

	public Button addToRestaurantButton;

	// Use this for initialization
	void Start () {
		//set up the info to appear in the shop menu
		if (name != null)
		{
			name.text = data.name;
		}
		if(cost != null)
		{
			cost.text = "" + data.cost;
		}
		if(starLevel != null)
		{
			starLevel.text = "StarLevel: " + data.starLevel;
		}
		if(atmosphere != null)
		{
			atmosphere.text = "Atmosphere: " + data.atmosphere;
		}
		if(numInInventoryText != null)
		{
			numInInventoryText.text = "x" + data.numInInventory;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
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
		PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().data.decor.Add(data);
		RemoveDecorationFromInv();
	}

	public void SetAddToRestaurant()
	{
		addToRestaurantButton.gameObject.SetActive(true);
	}

	public void RemoveAddToRestaurant()
	{
		addToRestaurantButton.gameObject.SetActive(false);
	}

	public void OnAddToRestaurantClick()
	{

	}

	public void OnCLick()
	{
		CatfePlayerScript.script.ReadyPurchase(data, null);
	}
}
