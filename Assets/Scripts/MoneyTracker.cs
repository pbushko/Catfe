using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTracker : MonoBehaviour
{
    private static Text moneyText;

	// Use this for initialization
	void Start ()
    {
        moneyText = GetComponent<Text>();

        moneyText.text = "Money: $" + RestaurantMain.playerMoney;
	}

    public static void ChangeMoneyCount()
    {
        moneyText.text = "Money: $" + RestaurantMain.playerMoney;
    }
}
