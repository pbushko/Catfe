using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.InteropServices;
using PlayFab;
using PlayFab.ClientModels;

public class CatInventory : MonoBehaviour {

	public static CatInventory catInv;

	public static List<RestaurantData> restaurants;

	public Dropdown sortCategories;

	public GameObject ChefInfoPrefab;
	public GameObject WaiterInfoPrefab;
	public GameObject DecorInfoPrefab;
	public GameObject DecorPref;
	public GameObject RecipeInfoPrefab;
	public GameObject OutfitPrefab;

	public GameObject ChefPanel;
	public GameObject DecorPanel;
	public GameObject RecipePanel;
	public GameObject OutfitPanel;

	public GameObject InventoryDecorPanel;
	public GameObject InventoryRecipePanel;
	public GameObject InventoryWaiterPanel;
	public GameObject InventoryOutfitPanel;



	public GameObject DecorInvWallPanel;
	public GameObject DecorInvTablePanel;
	public GameObject DecorInvFloorPanel;


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
		notPurchasedDecor = new List<GameObject>();
		StartChefs(chefStats);
		//StartWaiters(waiterStats);
		//StartDecorPlacementSpace();
		//StartDecorSpacePurchased();
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
			cat.transform.SetParent(InventoryWaiterPanel.transform, false);
			cat.GetComponent<WaiterCatRecruitStats>().data = w;
			waiterStats.Add(cat);
		}
	}

	// Initialization of owned decor into decor tile popup
	public void StartDecorPlacementSpace()
	{
		foreach (DecorationData d in PlayerData.playerData.purchasedDecor)
		{
			GameObject dec = (GameObject) Instantiate(DecorInfoPrefab);
			dec.GetComponent<Decoration>().data = d;
			switch (d.location) {
				case DecorationLocation.Wall:
					dec.transform.SetParent(DecorInvWallPanel.transform, false);
					break;
				case DecorationLocation.Table:
					dec.transform.SetParent(DecorInvTablePanel.transform, false);
					break;
				case DecorationLocation.Floor:
					dec.transform.SetParent(DecorInvFloorPanel.transform, false);
					break;
				default:
					break;
			}
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
		/*
		foreach(GameObject d in decor)
		{
			d.GetComponent<Decoration>().SetAddToRestaurant();
		}
		foreach(GameObject r in recipes)
		{
			r.GetComponent<RecipePanelData>().SetAddToRestaurant();
		}
		*/
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
		/*
		foreach(GameObject d in decor)
		{
			d.GetComponent<Decoration>().RemoveAddToRestaurant();
		}
		foreach(GameObject r in recipes)
		{
			r.GetComponent<RecipePanelData>().RemoveAddToRestaurant();
		}
		*/
	}

	public void AddToDecorCount(int i)
	{
		decor[i].GetComponent<Decoration>().AddSameDecorationToInv();
	}

	public void DeactivateAll() {
		ChefPanel.SetActive(false);
		InventoryWaiterPanel.SetActive(false);
		DecorPanel.SetActive(false);
		RecipePanel.SetActive(false);
	}

	public void SortChoice() 
	{
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

        List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		for (int i = 0; i < purchasedSorted.Count; i++) {
			purchasedSorted[i].transform.SetSiblingIndex(i);
		}
		for (int i = 0; i < notPurchasedSorted.Count; i++) {
			notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
		}

		List<GameObject> purchasedSortedRecipes = recipes.OrderBy(o=>o.transform.GetComponent<RecipePanelData>().data.recipeName).ToList();
		for (int i = 0; i < purchasedSortedRecipes.Count; i++) {
			purchasedSortedRecipes[i].transform.SetSiblingIndex(i);
		}

    }

    private void SortLowHigh()
    {

        List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.cost).ToList();
        List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.cost).ToList();
        for (int i = 0; i < purchasedSorted.Count; i++) {
	        purchasedSorted[i].transform.SetSiblingIndex(i);
        }
        for (int i = 0; i < notPurchasedSorted.Count; i++) {
	        notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
        }

		List<GameObject> purchasedSortedRecipes = recipes.OrderBy(o=>o.transform.GetComponent<RecipePanelData>().data.price).ToList();
		for (int i = 0; i < purchasedSortedRecipes.Count; i++) {
			purchasedSortedRecipes[i].transform.SetSiblingIndex(i);
		}

    }

    private void SortHighLow()
    {

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

		List<GameObject> purchasedSortedRecipes = recipes.OrderBy(o=>o.transform.GetComponent<RecipePanelData>().data.price).ToList();
		purchasedSortedRecipes.Reverse();
		//List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetChild(0).GetComponent<Text>().text).ToList();
		for (int i = 0; i < purchasedSortedRecipes.Count; i++) {
			purchasedSortedRecipes[i].transform.SetSiblingIndex(i);
		}
    }

    private void SortType()
    {

		List<GameObject> purchasedSorted = decor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.location).ToList();
        List<GameObject> notPurchasedSorted = notPurchasedDecor.OrderBy(o=>o.transform.GetComponent<Decoration>().data.location).ToList();
        for (int i = 0; i < purchasedSorted.Count; i++) {
	        purchasedSorted[i].transform.SetSiblingIndex(i);
        }
        for (int i = 0; i < notPurchasedSorted.Count; i++) {
	        notPurchasedSorted[i].transform.SetSiblingIndex(i + purchasedSorted.Count);
        }
    }

	public void GetPlayFabDecor()
	{
		Dictionary<string, ItemInstance> ownedItems = new Dictionary<string, ItemInstance>();
		//getting the user's inventory
		GetUserInventoryRequest request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, result => {
			restaurants = new List<RestaurantData>();			
			foreach (ItemInstance i in result.Inventory)
			{
				if (i.ItemClass == "cat")
				{
					if (i.ItemId == "waiter_cat")
					{
						AddCat(null, new WaiterData(i));
					}
					else if (i.ItemId == "chef_cat")
					{
						AddCat(new ChefData(i), null);
					}
				}
				else if (i.ItemClass == "Restaurant")
				{
					restaurants.Add(new RestaurantData(i));
				}
				else
				{
					ownedItems.Add(i.ItemId, i);
				}
			}
			CatfePlayerScript.script.SetUpRestaurants(restaurants);
		}, error => {});

		//getting the decor in the shop
		GetCatalogItemsRequest itemRequest = new GetCatalogItemsRequest();
		itemRequest.CatalogVersion = "Items";
		PlayFabClientAPI.GetCatalogItems(itemRequest, result => {
			List<CatalogItem> items = result.Catalog;
			foreach (CatalogItem i in items)
			{
				switch (i.ItemClass)
				{
					case "Decoration":
						DecorationData d = new DecorationData(i);
						GameObject newDecor = (GameObject)Instantiate(DecorPref);
						//if the item is owned, make sure the correct info is there
						if (ownedItems.ContainsKey(i.ItemId))
						{
							decor.Add(newDecor);
							GameObject inv = (GameObject)Instantiate(DecorInfoPrefab);
							d.numInInventory = (int)ownedItems[i.ItemId].RemainingUses;
							inv.GetComponent<Decoration>().ResetData(d);
							inv.transform.SetParent(InventoryDecorPanel.transform);
						}
						else
						{
							notPurchasedDecor.Add(newDecor);
						}
						newDecor.GetComponent<Decoration>().ResetData(d);
						newDecor.transform.SetParent(DecorPanel.transform);
						break;
					case "Recipe":
						Recipe r = new Recipe(i);
						GameObject newRecipe = (GameObject)Instantiate(RecipeInfoPrefab);
						recipes.Add(newRecipe);
						//if the item is owned, make sure the correct info is there
						if (ownedItems.ContainsKey(i.ItemId))
						{
							newRecipe.GetComponent<RecipePanelData>().ResetData(r);
							newRecipe.transform.SetParent(InventoryRecipePanel.transform);
						}
						//only put recipe panels in the store if they haven't been purchased
						else
						{
							newRecipe.GetComponent<RecipePanelData>().ResetData(r);
							newRecipe.transform.SetParent(RecipePanel.transform);
						}
						break;
					case "outfit":
						OutfitData o = new OutfitData(i);
						GameObject newOutfit = (GameObject)Instantiate(OutfitPrefab);
						//if the item is owned, make sure the correct info is there
						if (ownedItems.ContainsKey(i.ItemId))
						{
							newOutfit.GetComponent<Outfits>().ResetData(o);
							newOutfit.transform.SetParent(InventoryOutfitPanel.transform);
						}
						else
						{
							newOutfit.GetComponent<Outfits>().ResetData(o);
							newOutfit.transform.SetParent(OutfitPanel.transform);
						}
						break;
					default:
						break;
				}
			}
		}, error => {}
		);

	}

		// Puts either chef or waiter cat into player's inventory; one of the inputs should be null
	public void AddCat(ChefData c, WaiterData w)
	{
		if (c != null)
		{
			GameObject cat = (GameObject)Instantiate(ChefInfoPrefab);
        	cat.transform.SetParent(ChefPanel.transform, false);
			chefStats.Add(cat);
			cat.GetComponent<ChefCatRecruitStats>().data = c;
		}
		else if (w != null)
		{
			GameObject cat = (GameObject)Instantiate(WaiterInfoPrefab);
        	cat.transform.SetParent(InventoryWaiterPanel.transform, false);
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

	public void AddOwnedDecoration(DecorationData d, ItemInstance item)
	{
		for (int i = 0; i < notPurchasedDecor.Count; i++)
		{
			Decoration temp = notPurchasedDecor[i].GetComponent<Decoration>();
			if (temp.data.IsEqual(d))
			{
				d.numInInventory = (int)item.RemainingUses;
				temp.ResetData(d);
			}
		}
		GameObject inv = (GameObject)Instantiate(DecorInfoPrefab);
		inv.GetComponent<Decoration>().ResetData(d);
		inv.transform.SetParent(InventoryDecorPanel.transform);
	}

}
