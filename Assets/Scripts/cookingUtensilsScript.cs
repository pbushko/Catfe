using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingUtensilsScript : MonoBehaviour {

    public LoadingBar loader;

    public CookingUtensil utensil;

    public SpriteRenderer objectSprite;

    public void OnClick()
    {
        PlayerScript.AddCookingToolToPlayerQueue(utensil.utensil, loader, utensil.cookTime);
    }

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Upgrade()
    {
        MoneyTracker.ChangeMoneyCount(-utensil.upgradeCost);
        utensil.upgradeNum++;
        utensil.cookTime /= 2;
        utensil.upgradeCost *= 5;
        objectSprite.sprite = RestaurantMain.GetUpgradeSprite(objectSprite.sprite);
    }

    public int GetUpgradeCost()
    {
        return utensil.upgradeCost;
    }

    public float GetCookTime()
    {
        return utensil.cookTime;
    }

    public void SetSprite(Sprite s)
    {
        objectSprite.sprite = s;
    }

}
