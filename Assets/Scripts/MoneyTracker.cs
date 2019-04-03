using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class MoneyTracker : MonoBehaviour
{
    private static Text moneyText;

	// Use this for initialization
	void Start ()
    {
        moneyText = GetComponent<Text>();
	}

    public static void ChangeMoneyCount(int i)
    {
        PlayerData.playerData.playerMoney += i;
        //moneyText.text = "" + PlayerData.playerData.playerMoney;
    }

    public static void SetMoneyText(int amnt)
    {
        moneyText.text = "" + amnt;
    }

}
