﻿using System.Collections;
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

	public Image body;
	public Image face;

	// Use this for initialization
	void Start () {
		//data = EmployeeGenerator.GenerateChef();
		ResetData(data);
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
		}
		else if (trainingTimeLeft != null)
		{
			trainingTimeLeft.text = "Finished training!";
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
		//check if there is enough money... for now, auto buy
		PlayerData.playerData.chefs.Add(data);
		CatInventory.catInv.AddCat(data, null);
		CatInventory.catInv.ResetChefInv();
	}

	public void LayOff()
	{
		CatInventory.catInv.LayOffChefCat(gameObject);
	}

	public void Train()
	{
		if (data.isTraining || data.timesTrained >= 10)
		{
			return;
		}
		data.isTraining = true;
		float time = 5.0f + 10.0f * data.timesTrained;
		data.trainEndTime = DateTime.Now.AddSeconds(time);
	}
}
