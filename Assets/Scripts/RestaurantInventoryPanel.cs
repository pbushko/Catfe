using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantInventoryPanel : MonoBehaviour {

	public static RestaurantInventoryPanel inv;

	public GameObject chefPanel;
	public GameObject waiterPanel;

	//these hold the slots the waiters/chefs will be viewed in, but not the buttons
	private List<GameObject> chefSlots;
	private List<GameObject> waiterSlots;

	private int activeChefs;
	private int activeWaiters;

	// Use this for initialization
	void Start () {
		inv = this;
		//Debug.Log(chefPanel.transform.childCount);
		activeChefs = 0;
		activeWaiters = 0;
		chefSlots = new List<GameObject>();
		waiterSlots = new List<GameObject>();

		for(int i = 0; i < chefPanel.transform.childCount - 1; i++)
		{
			chefSlots.Add(chefPanel.transform.GetChild(i).gameObject);
		}
		for(int i = 0; i < waiterPanel.transform.childCount - 1; i++)
		{
			waiterSlots.Add(waiterPanel.transform.GetChild(i).gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddChef(ChefData c)
	{
		chefSlots[activeChefs].GetComponent<ChefCatRecruitStats>().data = c;
		chefSlots[activeChefs].SetActive(true);
		chefSlots[activeChefs].GetComponent<ChefCatRecruitStats>().ResetData(c);

		activeChefs++;
		CheckChefPanelCount();
	}

	public void AddWaiter(WaiterData w)
	{
		waiterSlots[activeWaiters].GetComponent<WaiterCatRecruitStats>().data = w;
		waiterSlots[activeWaiters].SetActive(true);
		waiterSlots[activeWaiters].GetComponent<WaiterCatRecruitStats>().ResetData(w);

		activeWaiters++;
		CheckWaiterPanelCount();
	}

	public void SetChefs(List<ChefData> c)
	{
		activeChefs = c.Count;

		for (int i = 0; i < Variables.MAX_CHEFS_IN_RESTAURANT; i++)
		{
			if(i >= activeChefs)
			{
				chefSlots[i].SetActive(false);
			}
			else
			{
				chefSlots[i].SetActive(true);
				chefSlots[i].GetComponent<ChefCatRecruitStats>().ResetData(c[i]);
			}
		}
		//make sure the add a new chef button is only available when it should be
		CheckChefPanelCount();
	}

	public void SetWaiters(List<WaiterData> w)
	{
		activeWaiters = w.Count;
		for (int i = 0; i < Variables.MAX_WAITERS_IN_RESTAURANT; i++)
		{
			if(i >= activeWaiters)
			{
				waiterSlots[i].SetActive(false);
			}
			else
			{
				waiterSlots[i].SetActive(true);
				waiterSlots[i].GetComponent<WaiterCatRecruitStats>().ResetData(w[i]);
			}
		}
		CheckWaiterPanelCount();
	}

	//if we are already at the max number of chefs, disable the button that hires more
	public void CheckChefPanelCount()
	{
		if (activeChefs >= Variables.MAX_CHEFS_IN_RESTAURANT)
		{
			//the button will always be at the end of the layout, so it will be the last child
			chefPanel.transform.GetChild(chefPanel.transform.childCount - 1).gameObject.SetActive(false);
		}
		else
		{
			chefPanel.transform.GetChild(chefPanel.transform.childCount - 1).gameObject.SetActive(true);
		}
	}

	//same for the waiters
	public void CheckWaiterPanelCount()
	{
		if (activeWaiters > Variables.MAX_WAITERS_IN_RESTAURANT)
		{
			waiterPanel.transform.GetChild(waiterPanel.transform.childCount - 1).gameObject.SetActive(false);
		}
		else
		{
			waiterPanel.transform.GetChild(waiterPanel.transform.childCount - 1).gameObject.SetActive(true);
		}
	}

	public void Open()
	{
		PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().Open();
	}
}
