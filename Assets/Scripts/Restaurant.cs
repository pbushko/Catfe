using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Restaurant : MonoBehaviour {

	public RestaurantData data;

	public SpriteRenderer title;

	public TextMesh text;

	//the time the restaurant is open, in sec
	private float openTime = 5.0f;

	// Use this for initialization
	void Start () {

		if (DateTime.Compare(DateTime.Now, data.timeToClose) > 0 && data.storedIncome != 0)
		{
			text.text = "Money to Collect: " + data.storedIncome;
			data.isOpen = false;
		}
		else if(data.isOpen)
		{
			TimeSpan timeLeft = data.timeToClose.Subtract(DateTime.Now);
			text.text = "Open for another " + "min: " + timeLeft.Minutes + " sec: " + timeLeft.Seconds;
		}
		else
		{
			text.text = "Not Open";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (data.isOpen && DateTime.Compare(DateTime.Now, data.timeToClose) > 0)
		{
			text.text = "Money to Collect: " + data.storedIncome;
			data.isOpen = false;
		}
		else if(data.isOpen)
		{
			TimeSpan timeLeft = data.timeToClose.Subtract(DateTime.Now);
			text.text = "Open for another " + "min: " + timeLeft.Minutes + " sec: " + timeLeft.Seconds;
		}
	}

	//open the restaurant to allow you to make money
	public void Open()
	{
		//data.timeOpened = DateTime.Now;
		CollectMoney();
		data.timeToClose = DateTime.Now.AddSeconds(5.0f);
		data.storedIncome = (int)((float)data.GetTotalIncome() *  (openTime/60f));
		data.isOpen = true;
	}

	//collect the money from the shop
	public void CollectMoney()
	{
		//the money will be the time spent open * the added income of ever cat working there
		//adding up the income/min
		if (!data.isOpen)
		{
			PlayerData.playerData.playerMoney += data.storedIncome;
			data.storedIncome = 0;
			text.text = "Not Open";
		}
	}

}
