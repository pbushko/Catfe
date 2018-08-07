using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantInventoryPanel : MonoBehaviour {

	public static RestaurantInventoryPanel inv;

	public GameObject chefPanel;
	public GameObject waiterPanel;

	private int activeChefs;
	private int activeWaiters;

	// Use this for initialization
	void Start () {
		inv = this;
		Debug.Log(chefPanel.transform.childCount);
		activeChefs = 0;
		activeWaiters = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddChef(ChefData c)
	{
		activeChefs++;
		CheckChefPanelCount();
	}

	public void AddWaiter(WaiterData w)
	{
		activeWaiters++;
		CheckWaiterPanelCount();
	}

	public void SetChefs(List<ChefData> c)
	{
		activeChefs = c.Count;


		//make sure the add a new chef button is only available when it should be
		CheckChefPanelCount();
	}

	public void SetWaiters(List<WaiterData> w)
	{
		activeWaiters = w.Count;
		CheckWaiterPanelCount();
	}

	//if we are already at the max number of chefs, disable the button that hires more
	public void CheckChefPanelCount()
	{
		if (activeChefs > Variables.MAX_CHEFS_IN_RESTAURANT)
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
}
