using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CatInventory : MonoBehaviour {

	public static CatInventory catInv;

	public GameObject ChefInfoPrefab;
	public GameObject WaiterInfoPrefab;
	public GameObject DecorInfoPrefab;
	public GameObject RecipeInfoPrefab;

	public GameObject ChefPanel;
	public GameObject WaiterPanel;
	public GameObject DecorPanel;
	public GameObject RecipePanel;

	private List<GameObject> chefStats;
	private List<GameObject> waiterStats;
	private List<GameObject> decor;
	private List<GameObject> recipes;

	private List<GameObject> notPurchasedDecor;

	// Use this for initialization
	//populate the list with the current cats in your inventory
	void Start () {
		catInv = this;
		chefStats = new List<GameObject>();
		waiterStats = new List<GameObject>();
		decor = new List<GameObject>();
		recipes = new List<GameObject>();
		notPurchasedDecor = PlayerData.playerData.allNotPurchasedDecorGameObjects;

		foreach (ChefData c in PlayerData.playerData.chefs)
		{
			GameObject cat = (GameObject)Instantiate(ChefInfoPrefab);
        	cat.transform.SetParent(ChefPanel.transform, false);
			cat.GetComponent<ChefCatRecruitStats>().data = c;
			chefStats.Add(cat);
		}
		foreach (WaiterData w in PlayerData.playerData.waiters)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(WaiterPanel.transform, false);
			cat.GetComponent<WaiterCatRecruitStats>().data = w;
			waiterStats.Add(cat);
		}
		foreach (DecorationData d in PlayerData.playerData.purchasedDecor)
		{
			GameObject dec = (GameObject)Instantiate(DecorInfoPrefab);
			dec.transform.SetParent(DecorPanel.transform, false);
			dec.GetComponent<Decoration>().data = d;
			decor.Add(dec);

		}
		foreach (Recipe r in PlayerData.playerData.purchasedRecipes)
		{
			GameObject recipe = (GameObject)Instantiate(RecipeInfoPrefab);
			recipe.transform.SetParent(RecipePanel.transform, false);
			recipe.GetComponent<RecipePanelData>().data = r;
			recipes.Add(recipe);
		}
	}

	public void AddCat(ChefData c, WaiterData w)
	{
		if (c != null)
		{
			GameObject cat = (GameObject)Instantiate(ChefInfoPrefab);
        	cat.transform.SetParent(ChefPanel.transform, false);
			PlayerData.playerData.chefs.Add(c);
			chefStats.Add(cat);
			cat.GetComponent<ChefCatRecruitStats>().data = c;
		}
		else if (w != null)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(WaiterPanel.transform, false);
			//LOOK HERE, WHY IS IT ADDING THE CHEF CAT TO PLAYER DATA ABOVE BUT NOT HERE?
			PlayerData.playerData.waiters.Add(w);
			waiterStats.Add(cat);
			cat.GetComponent<WaiterCatRecruitStats>().data = w;
		}
	}

	public void AddDecor(DecorationData d)
	{
		GameObject dec = (GameObject)Instantiate(DecorInfoPrefab);
		dec.transform.SetParent(DecorPanel.transform, false);
		//as the first purchase, there will always only be one in the inventory
		d.numInInventory = 1;
		dec.GetComponent<Decoration>().data = d;
		decor.Add(dec);
	}

	public void AddRecipe(Recipe r)
	{
		GameObject recipe = (GameObject)Instantiate(RecipeInfoPrefab);
		recipe.transform.SetParent(RecipePanel.transform, false);
		recipe.GetComponent<RecipePanelData>().data = r;
		recipes.Add(recipe);
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
				return;
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
				return;
			}
		}
	}

	public void RemoveDecoration(GameObject d)
	{
		for (int i = 0; i < decor.Count; i++)
		{
			//if these objects are the same, then we can safely remove them
			if (GameObject.ReferenceEquals(d, decor[i]))
			{
				Decoration toRemove = d.GetComponent<Decoration>();

				//only fully remove the item from the inv if the inv count reaches 0
				if (toRemove.data.numInInventory <= 0)
				{
					PlayerData.playerData.purchasedDecor.RemoveAt(i);
					decor.RemoveAt(i);
					Destroy(d);
				}
				return;
			}
		}
	}

	//put the add to restaurant button on the cats
	public void ReadyAddToRestaurant()
	{
		foreach(GameObject c in chefStats)
		{
			c.GetComponent<ChefCatRecruitStats>().SetRestaurantButton();
		}
		foreach(GameObject w in waiterStats)
		{
			w.GetComponent<WaiterCatRecruitStats>().SetRestaurantButton();
		}
		foreach(GameObject d in decor)
		{
			d.GetComponent<Decoration>().SetAddToRestaurant();
		}
		foreach(GameObject r in recipes)
		{
			r.GetComponent<RecipePanelData>().SetAddToRestaurant();
		}
	}
	
	public void RemoveAddToRestaurant()
	{
		foreach(GameObject c in chefStats)
		{
			c.GetComponent<ChefCatRecruitStats>().RemoveRestaurantButton();
		}
		foreach(GameObject w in waiterStats)
		{
			w.GetComponent<WaiterCatRecruitStats>().RemoveRestaurantButton();
		}
		foreach(GameObject d in decor)
		{
			d.GetComponent<Decoration>().RemoveAddToRestaurant();
		}
		foreach(GameObject r in recipes)
		{
			r.GetComponent<RecipePanelData>().RemoveAddToRestaurant();
		}
	}

	public void AddToDecorCount(int i)
	{
		decor[i].GetComponent<Decoration>().AddSameDecorationToInv();
	}

	public void DeactivateAll() {
		ChefPanel.SetActive(false);
		WaiterPanel.SetActive(false);
		DecorPanel.SetActive(false);
		RecipePanel.SetActive(false);
	}

	public void SortAZ()
    {
        List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		for (int i = 0; i < purchasedSorted.Count; i++) {
			purchasedSorted[i].transform.SetSiblingIndex(i);
		}
		for (int i = 0; i < notPurchasedSorted.Count; i++) {
			notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
		}

		List<GameObject> purchasedSorted = recipes.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
    }

    public static void SortLowHigh(List<GameObject> toSort)
    {
        
    }

    public static void SortHighLow(List<GameObject> toSort)
    {
        
    }

    public static void SortType(List<GameObject> toSort)
    {
        if (toSort.Count != 0) {
            toSort[0].GetComponent<Decoration>();
        }
    }

}
