using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantInventoryPanel : MonoBehaviour {

	public static RestaurantInventoryPanel inv;

	public GameObject newChefButton;

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

		for (int i = 1; i < Variables.MAX_CHEFS_IN_RESTAURANT; i++)
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
		for (int i = 1; i < Variables.MAX_WAITERS_IN_RESTAURANT; i++)
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

	public void RemoveCat(GameObject c)
	{
		//checking if the cat was a chef or waiter
		for (int i = 0; i < activeChefs; i++)
		{
			if (GameObject.ReferenceEquals(c, chefSlots[i]))
			{
				activeChefs--;
				//removing the chef from the restaurant's saved inventory
				CatfePlayerScript.script.activeRestaurant.data.chefs.RemoveAt(i);

				if (activeChefs == i)
				{
					c.SetActive(false);
				}
				else
				{
					//need to shift the active cats down a slot if there are any others
					for (int j = i + 1; j < activeChefs+1; j++)
					{
						//if the next slot is active, move its data down
						if (chefSlots[j].activeSelf)
						{
							chefSlots[j-1].GetComponent<ChefCatRecruitStats>().ResetData(chefSlots[j].GetComponent<ChefCatRecruitStats>().data);
						}
						//if not, just set the previous slot to inactive
						else
						{
							chefSlots[j-1].SetActive(false);
						}
						if (j == activeChefs)
						{
							chefSlots[j].SetActive(false);
						}
					}
				}
				CheckChefPanelCount();
				return;
			}
		}
		for (int i = 0; i < activeWaiters; i++)
		{
			if (GameObject.ReferenceEquals(c, waiterSlots[i]))
			{
				activeWaiters--;
				//removing the chef from the restaurant's saved inventory
				CatfePlayerScript.script.activeRestaurant.data.waiters.RemoveAt(i);

				if (activeWaiters == i)
				{
					c.SetActive(false);
				}
				else
				{
					//need to shift the active cats down a slot if there are any others
					for (int j = i + 1; j < activeWaiters+1; j++)
					{
						//if the next slot is active, move its data down
						if (waiterSlots[j].activeSelf)
						{
							waiterSlots[j-1].GetComponent<WaiterCatRecruitStats>().ResetData(waiterSlots[j].GetComponent<WaiterCatRecruitStats>().data);
						}
						//if not, just set the previous slot to inactive
						else
						{
							waiterSlots[j-1].SetActive(false);
						}
						if (j == activeChefs)
						{
							waiterSlots[j].SetActive(false);
						}
					}
				}
				CheckWaiterPanelCount();
				return;
			}
		}
		
	}

	//if we are already at the max number of chefs, disable the button that hires more
	public void CheckChefPanelCount()
	{
		if (activeChefs >= Variables.MAX_CHEFS_IN_RESTAURANT)
		{
			//the button will always be at the end of the layout, so it will be the last child
			newChefButton.SetActive(false);
		}
		else
		{
			newChefButton.SetActive(true);
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
		CatfePlayerScript.script.activeRestaurant.Open();
	}
}
