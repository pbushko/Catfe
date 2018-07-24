﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingUtensilsScript : MonoBehaviour {

    public LoadingBar loader;

    public CookingUtensil utensil;

    public void OnClick()
    {
        Debug.Log(utensil.utensil);
        PlayerScript.AddCookingToolToPlayerQueue(utensil.utensil, loader, utensil.cookTime);
    }

	// Use this for initialization
	void Start ()
    {
        utensil = new CookingUtensil(CookingTools.Knife, 10, 0, 1f, GetComponent<SpriteRenderer>());
        Debug.Log(utensil.utensil);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Upgrade()
    {
        utensil.upgradeNum++;
        utensil.cookTime /= 2;
        utensil.upgradeCost *= 5;
        utensil.objectSprite.sprite = RestaurantMain.GetUpgradeSprite(utensil.objectSprite.sprite);
        Debug.Log("upgraded!");
    }

    public int GetUpgradeCost()
    {
        return utensil.upgradeCost;
    }

    public float GetCookTime()
    {
        return utensil.cookTime;
    }

    public void SetUtensil(CookingTools c)
    {
        utensil = new CookingUtensil(c, utensil.upgradeCost, utensil.upgradeNum, utensil.cookTime, utensil.objectSprite);
       
    }
}
