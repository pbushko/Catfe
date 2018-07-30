using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaiterCatRecruitStats : MonoBehaviour {

	public WaiterData data;
	
	public Text name;
	public Text rarity;
	public Text income;

	public Image body;
	public Image face;

	// Use this for initialization
	void Start () {
		data = EmployeeGenerator.GenerateWaiter();
		ResetData(data);
	}

	public void ResetData(WaiterData newData)
	{
		data = newData;
		name.text = newData.name;
		rarity.text = "Rarity: " + newData.rarity;
		income.text = "Income: " + newData.income;
		body.sprite = newData.sprites[0];
		face.sprite = newData.sprites[1];
	}
	
}
