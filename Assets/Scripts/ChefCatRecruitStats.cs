using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChefCatRecruitStats : MonoBehaviour {

	public ChefData data;
	
	public Text name;
	public Text rarity;
	public Text income;
	public Text trainings;
	public Text specialties;
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
			data = EmployeeGenerator.GenerateChef();
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
			if (data.timesTrained % 10 == 0)
			{
				data.AddRandomRestaurantType();
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

	public void ResetData(ChefData newData)
	{
		data = newData;
		name.text = newData.name;
		rarity.text = "Rarity: " + newData.rarity;
		income.text = "Income: " + newData.income;
		if (trainings != null)
		{
			trainings.text = "Times Trained: " + newData.timesTrained;
		}
		specialties.text = newData.SpecialtiesToString();
		body.sprite = PlayerData.playerData.GetCatSprite(newData.sprites[0]);
		face.sprite = PlayerData.playerData.GetCatSprite(newData.sprites[1]);
	}

	public void RecruitChef()
	{
		//check if there is enough money, the cost is 100 * rarity + 1
		if (PlayerData.playerData.playerMoney >= 100 * (data.rarity + 1))
		{
			MoneyTracker.ChangeMoneyCount(-100 * (data.rarity + 1));
			CatInventory.catInv.AddCat(data, null);
		}
		else
		{
			Debug.Log("Not enough money!");
		}
	}

	public void LayOff()
	{
		RestaurantInventoryPanel.inv.RemoveCat(gameObject);
		CatInventory.catInv.LayOffChefCat(gameObject);
	}

	public void RemoveFromInv()
	{
		CatInventory.catInv.LayOffChefCat(gameObject);
	}

	//training will increase the income of the cat; every 10th training will award a chef specialty, rarer cats can train more
	public void Train()
	{
		if (data.isTraining || data.timesTrained >= (10 * (data.rarity + 1))  || PlayerData.playerData.playerMoney < trainingCost)
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
		CatfePlayerScript.script.MoveCatToRestaurant(data, null);
		CatInventory.catInv.LayOffChefCat(gameObject);
		CatInventory.catInv.RemoveAddToRestaurant();
	}

	public void RemoveCatFromRestaurant()
	{
		CatInventory.catInv.AddCat(data, null);
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
