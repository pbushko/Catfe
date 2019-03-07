using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.InteropServices;

public class CatInventory : MonoBehaviour {

	public static CatInventory catInv;

	public Dropdown sortCategories;

	public GameObject ChefInfoPrefab;
	public GameObject WaiterInfoPrefab;
	public GameObject DecorInfoPrefab;
	public GameObject RecipeInfoPrefab;

	public GameObject ChefPanel;
	public GameObject WaiterPanel;
	public GameObject DecorPanel;
	public GameObject RecipePanel;
	public GameObject DecorInvPanel;

	private List<GameObject> chefStats;
	private List<GameObject> waiterStats;
	private List<GameObject> decor;
	private List<GameObject> recipes;

	private List<GameObject> notPurchasedDecor;

	// Use this for initialization
	// populates the lists with the current chefs, waiters, decor, recipes and outfits in your inventory
	// Also, populates other screens on the start
	void Start () {
		catInv = this;
		chefStats = new List<GameObject>();
		waiterStats = new List<GameObject>();
		decor = new List<GameObject>();
		recipes = new List<GameObject>();
		notPurchasedDecor = PlayerData.playerData.allNotPurchasedDecorGameObjects;

		/*foreach (ChefData c in PlayerData.playerData.chefs)
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
			//COPY EACH TO NEW OBJ AND THEN SET THE COPY'S PARENT TO THE NEW GRIDVIEW FOR DECOR TILE


		}
		foreach (Recipe r in PlayerData.playerData.purchasedRecipes)
		{
			GameObject recipe = (GameObject)Instantiate(RecipeInfoPrefab);
			recipe.transform.SetParent(RecipePanel.transform, false);			
			recipe.GetComponent<RecipePanelData>().data = r;
			recipes.Add(recipe);
		}*/
		StartChefs(chefStats);
		StartWaiters(waiterStats);
		StartDecor(decor);
		StartRecipes(recipes);
		StartDecorSpacePurchased();
		Debug.Log("yeeeeet");
		SortChoice();
	}

	// Initialization of owned chefs
	public void StartChefs(List<GameObject> chefStats)
	{
		foreach (ChefData c in PlayerData.playerData.chefs)
		{
			GameObject cat = (GameObject)Instantiate(ChefInfoPrefab);
			cat.transform.SetParent(ChefPanel.transform, false);
			cat.GetComponent<ChefCatRecruitStats>().data = c;
			chefStats.Add(cat);
		}
	}
	
	//Initialization of owned waiters
	public void StartWaiters(List<GameObject> waiterStats)
	{
		foreach (WaiterData w in PlayerData.playerData.waiters)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
			cat.transform.SetParent(WaiterPanel.transform, false);
			cat.GetComponent<WaiterCatRecruitStats>().data = w;
			waiterStats.Add(cat);
		}
	}
	
	//Initialization of owned decor
	public void StartDecor(List<GameObject> decor)
	{
		foreach (DecorationData d in PlayerData.playerData.purchasedDecor)
		{
			GameObject dec = (GameObject)Instantiate(DecorInfoPrefab);
			dec.transform.SetParent(DecorPanel.transform, false);
			dec.GetComponent<Decoration>().data = d;
			decor.Add(dec);

		}
	}
	
	//Initialization of owned recipes
	public void StartRecipes(List<GameObject> recipes)
	{
		foreach (Recipe r in PlayerData.playerData.purchasedRecipes)
		{
			GameObject recipe = (GameObject)Instantiate(RecipeInfoPrefab);
			recipe.transform.SetParent(RecipePanel.transform, false);			
			recipe.GetComponent<RecipePanelData>().data = r;
			recipes.Add(recipe);
		}
	}

	// Initialization of owned decor into decor tile popup
	public void StartDecorSpacePurchased()
	{
		foreach (DecorationData d in PlayerData.playerData.purchasedDecor)
		{
			Debug.Log(d);
			GameObject dec = (GameObject)Instantiate(DecorInfoPrefab);
			dec.transform.SetParent(DecorInvPanel.transform, false);
			dec.GetComponent<Decoration>().data = d;
			//decor.Add(dec);

		}
	}
	// Puts either chef or waiter cat into player's inventory; one of the inputs should be null
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

	public void SortChoice() 
	{
		Debug.Log("yeeeeet2");
		switch(sortCategories.options[sortCategories.value].text)
		{
			case("Price: Low to High"):
				SortLowHigh();
				break;
			case("Price: High to Low"):
				SortHighLow();
				break;
			case("A -> Z"):
				SortAZ();
				break;
			case("Type"):
				SortType();
				break;
			default:
				SortAZ();
				break;
		}
	}

	private void SortAZ()
    {
	    Debug.Log("yeeeeetAZ");

        List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		for (int i = 0; i < purchasedSorted.Count; i++) {
			purchasedSorted[i].transform.SetSiblingIndex(i);
		}
		for (int i = 0; i < notPurchasedSorted.Count; i++) {
			notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
		}

		List<GameObject> purchasedSortedRecipes = PlayerData.playerData.allNotPurchasedRecipeGameObjects.OrderBy(o=>o.transform.GetComponent<RecipePanelData>().data.recipeName).ToList();
		for (int i = 0; i < purchasedSortedRecipes.Count; i++) {
			purchasedSortedRecipes[i].transform.SetSiblingIndex(i);
		}

    }

    private void SortLowHigh()
    {
	    Debug.Log("yeeeeetLowHigh");

        List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.cost).ToList();
        List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.cost).ToList();
        for (int i = 0; i < purchasedSorted.Count; i++) {
	        purchasedSorted[i].transform.SetSiblingIndex(i);
        }
        for (int i = 0; i < notPurchasedSorted.Count; i++) {
	        notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
        }

		List<GameObject> purchasedSortedRecipes = PlayerData.playerData.allNotPurchasedRecipeGameObjects.OrderBy(o=>o.transform.GetComponent<RecipePanelData>().data.price).ToList();
		for (int i = 0; i < purchasedSortedRecipes.Count; i++) {
			purchasedSortedRecipes[i].transform.SetSiblingIndex(i);
		}

    }

    private void SortHighLow()
    {
	    Debug.Log("yeeeeetHighLow");

	    List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.cost).ToList();
	    purchasedSorted.Reverse();
	    List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.cost).ToList();
		notPurchasedSorted.Reverse();
	    for (int i = 0; i < purchasedSorted.Count; i++) {
		    purchasedSorted[i].transform.SetSiblingIndex(i);
	    }
	    for (int i = 0; i < notPurchasedSorted.Count; i++) {
		    notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
	    }

		List<GameObject> purchasedSortedRecipes = PlayerData.playerData.allNotPurchasedRecipeGameObjects.OrderBy(o=>o.transform.GetComponent<RecipePanelData>().data.price).ToList();
		purchasedSortedRecipes.Reverse();
		//List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		for (int i = 0; i < purchasedSortedRecipes.Count; i++) {
			purchasedSortedRecipes[i].transform.SetSiblingIndex(i);
		}
    }

    private void SortType()
    {
	    Debug.Log("yeeeeet2Slow");

		List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.location).ToList();
        List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.location).ToList();
        for (int i = 0; i < purchasedSorted.Count; i++) {
	        purchasedSorted[i].transform.SetSiblingIndex(i);
        }
        for (int i = 0; i < notPurchasedSorted.Count; i++) {
	        notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
        }
    }

}
