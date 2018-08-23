using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInventory : MonoBehaviour {

	public static CatInventory catInv;

	public GameObject ChefInfoPrefab;
	public GameObject WaiterInfoPrefab;
	public GameObject DecorInfoPrefab;

	public GameObject ChefPanel;
	public GameObject WaiterPanel;
	public GameObject DecorPanel;

	private List<GameObject> chefStats;
	private List<GameObject> waiterStats;
	private List<GameObject> decor;

	// Use this for initialization
	//populate the list with the current cats in your inventory
	void Start () {
		catInv = this;
		chefStats = new List<GameObject>();
		waiterStats = new List<GameObject>();
		decor = new List<GameObject>();
		foreach (ChefData c in PlayerData.playerData.chefs)
		{
			GameObject cat = (GameObject)Instantiate(ChefInfoPrefab);
        	cat.transform.SetParent(ChefPanel.transform);
			cat.GetComponent<ChefCatRecruitStats>().data = c;
			chefStats.Add(cat);
		}
		foreach (WaiterData w in PlayerData.playerData.waiters)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(WaiterPanel.transform);
			cat.GetComponent<WaiterCatRecruitStats>().data = w;
			waiterStats.Add(cat);
		}
		foreach (DecorationData d in PlayerData.playerData.purchasedDecor)
		{
			GameObject dec = (GameObject)Instantiate(DecorInfoPrefab);
			dec.transform.SetParent(DecorPanel);
			dec.GetComponent<Decoration>().data = d;
			decor.Add(dec);
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
				PlayerData.playerData.chefs.RemoveAt(i);
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
				PlayerData.playerData.waiters.RemoveAt(i);
				waiterStats.RemoveAt(i);
				Destroy(c);
			}
		}	
	}

	void Update()
	{
		
	}

}
