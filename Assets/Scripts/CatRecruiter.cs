using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CatRecruiter : MonoBehaviour {

	private List<WaiterData> waiters;
	private List<ChefData> chefs;

	public WaiterCatRecruitStats cat1;
	public WaiterCatRecruitStats cat2;
	public WaiterCatRecruitStats cat3;

	public ChefCatRecruitStats cat4;
	public ChefCatRecruitStats cat5;
	public ChefCatRecruitStats cat6;

	private DateTime refreshTime;
	public Text refreshTimeText;


	// Use this for initialization
	void Start () {
		RefreshWaiterCats();
		RefreshChefCats();
		refreshTime = DateTime.Now.AddSeconds(30f);
	}
	
	// Update is called once per frame
	void Update () {
		//update the counter for time until a refresh
		TimeSpan timeLeft = refreshTime.Subtract(DateTime.Now);
		refreshTimeText.text = "min: " + timeLeft.Minutes + " sec: " + timeLeft.Seconds;
		if (DateTime.Compare(DateTime.Now, refreshTime) > 0) {
			RefreshWaiterCats();
			RefreshChefCats();
		}
	}

	public void RefreshWaiterCats()
	{
		//getting the waiter data to put into the buy menu
		waiters = new List<WaiterData>();
		//putting in 3 waiters
		waiters.Add(EmployeeGenerator.GenerateWaiter());
		waiters.Add(EmployeeGenerator.GenerateWaiter());
		waiters.Add(EmployeeGenerator.GenerateWaiter());

		cat1.ResetData(waiters[0]);
		cat2.ResetData(waiters[1]);
		cat3.ResetData(waiters[2]);

		refreshTime = DateTime.Now.AddSeconds(30f);
	}

	public void RefreshChefCats()
	{
		//getting the waiter data to put into the buy menu
		chefs = new List<ChefData>();
		//putting in 3 waiters
		chefs.Add(EmployeeGenerator.GenerateChef());
		chefs.Add(EmployeeGenerator.GenerateChef());
		chefs.Add(EmployeeGenerator.GenerateChef());

		cat4.ResetData(chefs[0]);
		cat5.ResetData(chefs[1]);
		cat6.ResetData(chefs[2]);

		refreshTime = DateTime.Now.AddSeconds(30f);
	}
}
