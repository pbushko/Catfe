using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingUtensilsScript : MonoBehaviour {

    public CookingTools utensil;

    public LoadingBar loader;

    private int upgradeCost;

    //the current upgrade of the utensil
    private int upgradeNum;

    private float cookTime;

    private SpriteRenderer objectSprite;

    public void OnClick()
    {
        PlayerScript.AddCookingToolToPlayerQueue(utensil, loader, cookTime);
    }

	// Use this for initialization
	void Start ()
    {
        upgradeCost = 0;
        upgradeCost = 10;
        cookTime = 1f;
        objectSprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Upgrade()
    {
        upgradeNum++;
        cookTime /= 2;
        upgradeCost *= 5;
        objectSprite.sprite = RestaurantMain.GetUpgradeSprite(objectSprite.sprite);
        Debug.Log("upgraded!");
    }

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }

    public float GetCookTime()
    {
        return cookTime;
    }
}
