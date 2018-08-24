using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decoration : MonoBehaviour {

	public DecorationData data;

	public Text name;
	public Text starLevel;
	public Text atmosphere;
	public Text cost;
	//public Text description;

	// Use this for initialization
	void Start () {
		//set up the info to appear in the shop menu
		name.text = data.name;
		if(cost != null)
		{
			cost.text = "" + data.cost;
		}
		if(starLevel != null)
		{
			starLevel.text = "StarLevel: "; //need to add data.starLevel;
		}
		if(atmosphere != null)
		{
			atmosphere.text = "Atmosphere: " + data.atmosphere;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCLick()
	{
		CatfePlayerScript.script.ReadyPurchase(data, null);
	}
}
