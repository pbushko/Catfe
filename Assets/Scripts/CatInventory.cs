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
			cat.GetComponent<ChefCatRecruitStats>().data = c;
			chefStats.Add(cat);
		}
		foreach (WaiterData w in waiterdata)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(WaiterPanel.transform);
			cat.GetComponent<WaiterCatRecruitStats>().data = w;
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
			cat.GetComponent<ChefCatRecruitStats>().data = c;
		}
		else if (w != null)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(WaiterPanel.transform);
			waiterStats.Add(cat);
			cat.GetComponent<WaiterCatRecruitStats>().data = w;
		}
	}

	public void LayOffChefCat(GameObject c)
	{
		//going through and finding the chef to lay off, remove them from the player's list of cats
		for (int i = 0; i < chefStats.Count; i++)
		{
			//if these objects are the same, then we can safely remove them
			if (GameObject.ReferenceEquals(c, chefStats[i]))
			{
				chefdata.RemoveAt(i);
				chefStats.RemoveAt(i);
				Destroy(c);
			}
		}	
	}

	public void LayOffWaiterCat(GameObject c)
	{
		//going through and finding the chef to lay off, remove them from the player's list of cats
		for (int i = 0; i < waiterStats.Count; i++)
		{
			//if these objects are the same, then we can safely remove them
			if (GameObject.ReferenceEquals(c, waiterStats[i]))
			{
				waiterdata.RemoveAt(i);
				waiterStats.RemoveAt(i);
				Destroy(c);
			}
		}	
	}

	void Update()
	{
		
	}

}
