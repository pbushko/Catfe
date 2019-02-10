using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CookingUtensil {

	public CookingTools utensil;

    private int upgradeCost = 10;

    //the current upgrade of the utensil
    private int upgradeNum = 0;

    private float cookTime = 1f;

	public CookingUtensil(CookingTools c)
	{
		utensil = c;
		upgradeCost = 10;
		upgradeNum = 0;
		cookTime = 1f;
	}

	public CookingUtensil(CookingTools c, int uc, int un, float ct)
	{
		utensil = c;
		upgradeCost = uc;
		upgradeNum = un;
		cookTime = ct;
	}

	public void SetUtensil(CookingTools c)
	{
		utensil = c;
	}

}
