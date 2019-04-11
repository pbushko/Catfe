using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Restaurant : MonoBehaviour {

	public RestaurantData data;

	//public SpriteRenderer title;

	public TextMesh openText;

	public GameObject stars;

	//the time the restaurant is open, in sec
	private float openTime = 5.0f;

	// Use this for initialization
	void Start () 
	{
		//setting the star level on the restaurant
		for (int i = 0; i < data.stars; i++)
		{
			stars.transform.GetChild(i).gameObject.SetActive(true);
		}

		//setting the text for if the stor is open or not
		if (DateTime.Compare(DateTime.Now, data.timeToClose) > 0 && data.storedIncome != 0)
		{
			openText.text = "Money to Collect: " + data.storedIncome;
			data.isOpen = false;
		}
		else if(data.isOpen)
		{
			TimeSpan timeLeft = data.timeToClose.Subtract(DateTime.Now);
			openText.text = "Open for another " + "min: " + timeLeft.Minutes + " sec: " + timeLeft.Seconds;
		}
		else
		{
			openText.text = "Not Open";
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (data.isOpen && DateTime.Compare(DateTime.Now, data.timeToClose) > 0)
		{
			openText.text = "Money to Collect: " + data.storedIncome;
			data.isOpen = false;
		}
		else if(data.isOpen)
		{
			TimeSpan timeLeft = data.timeToClose.Subtract(DateTime.Now);
			openText.text = "Open for another " + "min: " + timeLeft.Minutes + " sec: " + timeLeft.Seconds;
		}
	}

	//open the restaurant to allow you to make money
	public void Open()
	{
		//can't open the shop if there is not at least 1 chef and 1 waiter
		if (data.chefs.Count >= 1 && data.waiters.Count >= 1)
		{
			CollectMoney();
			data.timeToClose = DateTime.Now.AddSeconds(5.0f);
			data.storedIncome = (int)((float)data.GetTotalIncome() *  (openTime/60f));
			data.isOpen = true;
			data.starProgress += 0.4f;
		}
		else
		{
			Debug.Log("Need at least 1 chef and 1 waiter before you can open shop!");
		}
	}

	//collect the money from the shop
	public int CollectMoney()
	{
		//the money will be the time spent open * the added income of ever cat working there
		//adding up the income/min
		if (!data.isOpen)
		{
			MoneyTracker.ChangeMoneyCount(data.storedIncome);
			int toRet = data.storedIncome;
			data.storedIncome = 0;
			openText.text = "Not Open";
			if (data.stars < Variables.MAX_STAR_LEVEL && data.starProgress > 1)
			{
				data.starProgress -= 1;
				AddStar();
			}
			return toRet;
		}
		return 0;
	}

	public void AddStar()
	{
		stars.transform.GetChild(data.stars).gameObject.SetActive(true);
		data.stars++;
	}

	// Puts decoration into restaurant (can replace)
	public bool AddDecoration(DecorationData d)
	{
		//go through the decor and see if it will be replacing something, or not
		for (int i = 0; i < data.decor.Count; i++)
		{
			DecorationData decData = data.decor[i];
			if (decData.location == d.location)
			{
				Debug.Log("Replaced " + decData.ToString());
				data.decor.Remove(decData);
				CatfePlayerScript.script.SetDecorationSprites();
			}
		}
		//we add the new decor here whether or not a decoration is being replaced
		data.decor.Add(d);
		return true;
	}

	public void SetDecorationSprites()
	{

	}

}
