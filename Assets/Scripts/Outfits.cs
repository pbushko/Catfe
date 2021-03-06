﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;


//a recipe object; stores what is needed for each recipe
public class Outfits : MonoBehaviour
{

    public OutfitData data;

	public Text name;
    public Text cost;

	//the image renderer of the decoration to go on the panel
	public Image sprite;

	// Use this for initialization
	void Start () {
		//set up the info to appear in the shop menu
		if (data != null)
		{
			ResetData(data);
		}
	}

	public void ResetData(OutfitData newData)
	{
		data = newData;
		if (name != null)
		{
			name.text = newData.name;
		}
		if (cost != null)
		{
			cost.text = "" + newData.cost;
		}
		if (sprite != null)
		{
			sprite.sprite = newData.sprite;
		}
	}

    public void onClick()
    {
        if (data.clothingArea == "hat")
        {
            PlayerData.playerData.areasToDress[0].sprite = data.sprite;
        }
        else if (data.clothingArea == "glasses")
        {
            PlayerData.playerData.areasToDress[1].sprite = data.sprite;
        }
        else if (data.clothingArea == "shirt")
        {
            PlayerData.playerData.areasToDress[2].sprite = data.sprite;
        }
        else if (data.clothingArea == "arms")
        {
            PlayerData.playerData.areasToDress[3].sprite = data.sprite;
        }
        else if (data.clothingArea == "pants")
        {
            PlayerData.playerData.areasToDress[4].sprite = data.sprite;
        }
        else if (data.clothingArea == "feet")
        {
            PlayerData.playerData.areasToDress[5].sprite = data.sprite;
        }
    }

}
