﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WaiterCatRecruitStats : MonoBehaviour {

	public WaiterData data;
	
	public Text name;
	public Text rarity;
	public Text income;
	public Text price;
	public Text trainings;
	public Text trainingTimeLeft;

	public Button trainingButton;
	public Button addToRestaurantButton;
	public Button layOffButton;

	public Image body;
	public Image face;

	private int trainingCost;

	// Use this for initialization
	void Start () {
		if (data == null)
		{
			data = EmployeeGenerator.GenerateWaiter();
		}
		ResetData(data);
		trainingCost = 100;
	}

	void Update()
	{
		//if the cat is training, check the time it was training with the current time and upgrade if enough has passed
		if (data.isTraining && DateTime.Compare(DateTime.Now, data.trainEndTime) > 0)
		{
			data.timesTrained++;
			switch (data.rarity)
			{
				case 0:
					data.income += 10*data.timesTrained;
					break;
				case 1:
					data.income += 20*data.timesTrained;
					break;
				case 2:
					data.income += 40*data.timesTrained;
					break;
				case 3:
					data.income += 75*data.timesTrained;
					break;
				default:
					break;
			}
			ResetData(data);
			data.isTraining = !data.isTraining;
		}
		else if (data.isTraining)
		{
			TimeSpan timeLeft = data.trainEndTime.Subtract(DateTime.Now);
			trainingTimeLeft.text = "min: " + timeLeft.Minutes + " sec: " + timeLeft.Seconds;
			trainingButton.interactable = false;
		}
		else if (trainingTimeLeft != null)
		{
			trainingTimeLeft.text = "Train!";
			trainingButton.interactable = true;
		}
	}

	public void ResetData(WaiterData newData)
	{
		data = newData;
		name.text = newData.name;
		rarity.text = "Rarity: " + newData.rarity;
		income.text = "Income: " + newData.income;
		if (price != null)
		{
			price.text = "Price: " + 100 * (data.rarity + 1);
		}
		if (trainings != null)
		{
			trainings.text = "Times Trained: " + newData.timesTrained;
		}
		body.sprite = PlayerData.playerData.GetCatSprite(newData.sprites[0]);
		face.sprite = PlayerData.playerData.GetCatSprite(newData.sprites[1]);
	}

	public void RecruitWaiter()
	{
		//check if there is enough money, the cost is 100 * rarity + 1
		if (PlayerData.playerData.playerMoney >= 100 * (data.rarity + 1))
		{
			MoneyTracker.ChangeMoneyCount(-100 * (data.rarity + 1));
			CatInventory.catInv.AddCat(null, data);
		}
		else
		{
			Debug.Log("Not enough money!");
		}
	}
	
	public void LayOff()
	{
		RestaurantInventoryPanel.inv.RemoveCat(gameObject);
		CatInventory.catInv.LayOffWaiterCat(gameObject);
	}

	public void RemoveFromInv()
	{
		CatInventory.catInv.LayOffWaiterCat(gameObject);
	}

	public void Train()
	{
		if (data.isTraining || data.timesTrained >= 10 || PlayerData.playerData.playerMoney < trainingCost)
		{
			return;
		}
		MoneyTracker.ChangeMoneyCount(-trainingCost);
		data.isTraining = true;
		float time = 5.0f + 10.0f * data.timesTrained;
		data.trainEndTime = DateTime.Now.AddSeconds(time);
		trainingButton.interactable = false;
	}

	public void AddCatToRestaurant()
	{
		CatfePlayerScript.script.MoveCatToRestaurant(null, data);
		CatInventory.catInv.LayOffWaiterCat(gameObject);
	}

	public void RemoveCatFromRestaurant()
	{
		CatInventory.catInv.AddCat(null, data);
		RestaurantInventoryPanel.inv.RemoveCat(gameObject);
	}

	public void SetRestaurantButton()
	{
		addToRestaurantButton.gameObject.SetActive(true);
		layOffButton.gameObject.SetActive(false);
		trainingButton.gameObject.SetActive(false);
	}

	public void RemoveRestaurantButton()
	{
		addToRestaurantButton.gameObject.SetActive(false);
		layOffButton.gameObject.SetActive(true);
		trainingButton.gameObject.SetActive(true);
	}

}
