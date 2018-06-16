using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTracker : MonoBehaviour
{
    private Text moneyText;
    public int money;

	// Use this for initialization
	void Start ()
    {
        moneyText = GetComponent<Text>();
        money = 30;

        //Writing the money into the canvas
        moneyText.text = "Money: $" + money;
	}

    public void AddMoney(int m)
    {
        money += m;
        moneyText.text = "Money: $" + money;
    }
}
