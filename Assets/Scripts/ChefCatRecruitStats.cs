using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefCatRecruitStats : MonoBehaviour {

	public ChefData data;
	
	public Text name;
	public Text rarity;
	public Text income;
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
		specialties.text = newData.SpecialtiesToString();
		body.sprite = newData.sprites[0];
		face.sprite = newData.sprites[1];
	}
}
