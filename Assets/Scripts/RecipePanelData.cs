using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePanelData : MonoBehaviour {

	public Recipe data;

	public Text name;
	public Text starLevel;
	public Text cost;

    public Image sprite;

	public GameObject addToRestaurantButton;

    void Start()
    {
        //set up the info to appear in the shop menu
		if (data != null)
        {
            ResetData(data);
        }
    }

    public void ResetData(Recipe newData)
    {
        if (name != null)
		{
			name.text = newData.recipeName;
		}
		if(cost != null)
		{
			cost.text = "" + newData.price;
		}
		if(starLevel != null)
		{
			starLevel.text = "StarLevel: " + newData.starLevel;
		}
		if (sprite != null)
        {
            sprite.sprite = PlayerData.GetFoodSprite(newData);
        }
        data = newData;
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
		CatfePlayerScript.script.ReadyPurchase(gameObject, null, data);
	}

}