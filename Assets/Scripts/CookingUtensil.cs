using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CookingUtensil {

	public CookingTools utensil;

    public int upgradeCost;

    //the current upgrade of the utensil
    public int upgradeNum;

    public float cookTime;

    public SpriteRenderer objectSprite;

	public CookingUtensil(CookingTools c, int uc, int un, float ct, SpriteRenderer s)
	{
		utensil = c;
		upgradeCost = uc;
		upgradeNum = un;
		cookTime = ct;
		objectSprite = s;
	}

}
