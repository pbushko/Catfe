using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefCatRecruitStats : MonoBehaviour {

	public ChefData data;
	
	public Text name;
	public Text rarity;
	public Text income;
	public Text trainings;
	public Text specialties;

	public Image body;
	public Image face;

	// Use this for initialization
	void Start () {
		data = EmployeeGenerator.GenerateChef();
		ResetData(data);
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
		if (data.timesTrained >= 10)
		{
			return;
		}
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
	}
}
