using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePurchaseConfirmer : MonoBehaviour {

	public Text name;
	public Text starLevel;
	public Text atmosphere;
	public Text cost;
	public Text description;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateText(DecorationData d)
	{
		if (d != null)
		{
			name.text = d.name;
			atmosphere.text = "Atmosphere: " + d.atmosphere;
			cost.text = "Cost: " + d.cost;
			starLevel.text = "Star level: " + d.starLevel;
			description.text = d.description;
		}
	}

	public void CompletePurchase()
	{
		//Debug.Log("purchased!");
		CatfePlayerScript.script.PurchaseItem(true);
		gameObject.SetActive(false);
	}

	public void RejectPurchase()
	{
		CatfePlayerScript.script.PurchaseItem(false);
		gameObject.SetActive(false);
	}
}
