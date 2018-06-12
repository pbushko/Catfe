using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTracker : MonoBehaviour {

    public static int money;

    static Text moneyText;

	// Use this for initialization
	void Start () {
        moneyText = GetComponent<Text>();
        money = 30;

        //Writing the money into the canvas
        moneyText.text = "Money: $" + money;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void addMoney(int m)
    {
        money += m;
        moneyText.text = "Money: $" + money;
    }
}
