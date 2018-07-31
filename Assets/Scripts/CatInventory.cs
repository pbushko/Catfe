using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInventory : MonoBehaviour {

	public static CatInventory catInv;

	public GameObject ChefInfoPrefab;
	public GameObject WaiterInfoPrefab;

	public GameObject ChefPanel;
	public GameObject WaiterPanel;

	private List<GameObject> chefStats;
	private List<GameObject> waiterStats;
	private List<ChefData> chefdata;
	private List<WaiterData> waiterdata;

	// Use this for initialization
	//populate the list with the current cats in your inventory
	void Start () {
		catInv = this;
		chefdata = PlayerData.playerData.chefs;
		waiterdata = PlayerData.playerData.waiters;
		chefStats = new List<GameObject>();
		waiterStats = new List<GameObject>();
		foreach (ChefData c in chefdata)
		{
			GameObject cat = (GameObject)Instantiate(ChefInfoPrefab);
        	cat.transform.SetParent(ChefPanel.transform);
			chefStats.Add(cat);
		}
		foreach (WaiterData w in waiterdata)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(WaiterPanel.transform);
			waiterStats.Add(cat);
		}
		
	}

	public void ResetChefInv()
	{
		for (int i = 0; i < chefStats.Count; i++)
		{
			chefStats[i].GetComponent<ChefCatRecruitStats>().ResetData(chefdata[i]);
		}
	}

	public void ResetWaiterInv()
	{
		for (int i = 0; i < waiterStats.Count; i++)
		{
			waiterStats[i].GetComponent<WaiterCatRecruitStats>().ResetData(waiterdata[i]);
		}
	}

	public void AddCat(ChefData c, WaiterData w)
	{
		if (c != null)
		{
			GameObject cat = (GameObject)Instantiate(ChefInfoPrefab);
        	cat.transform.SetParent(ChefPanel.transform);
			chefStats.Add(cat);
			ResetChefInv();
		}
		else if (w != null)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(WaiterPanel.transform);
			waiterStats.Add(cat);
			ResetWaiterInv();
		}
	}

	void Update()
	{
		
	}

}
