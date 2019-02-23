using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;

[System.Serializable]
public enum RestaurantType { Catfe, Italian, Sandwich, Burger, Asian, Indian, Bakery, NumOfRestaurantTypes };
public enum DecorationLocation { Wall, Floor, Table };

//the class to store the data for each store you own
[System.Serializable]
public class RestaurantData 
{
	//keeps track of the number of tables and the current upgrade count on them
	public List<int> tables;

	//the type of the restuarant
	public RestaurantType type;

	//keeps track of your employees currently at the restaurant
	public List<WaiterData> waiters;
	public List<ChefData> chefs;
	//the decor in the shop
	public List<DecorationData> decor;

	//the number of stars and portion of a star of the restaurant
	public int stars;
	public float starProgress;

	//the time the restaurant was opened to get idle income
	public bool isOpen;
	public DateTime timeToClose;
	public int storedIncome;

	//the last time the minigame was played in the restaurant
	public DateTime timeMinigamePlayed;
	//keeps track of the number of minigames completed in that shop
	//used to determine if the restaurant can be level up or not
	public int minigamesCompleted;
	//the state of the kitchen for the minigame
	public List<CookingUtensil> utensils;

	public int location;

	public RestaurantData(RestaurantType rt)
	{
		tables = new List<int>();
		type = rt;
		waiters = new List<WaiterData>();
		chefs = new List<ChefData>();
		decor = new List<DecorationData>();
		utensils = new List<CookingUtensil>();
		stars = 0;
		starProgress = 0f;
		minigamesCompleted = 0;
	}

	public void PrintEmployees()
	{
		if (waiters.Count == 0 && chefs.Count == 0)
		{
			Debug.Log("No employees!");
		}
		else
		{
			Debug.Log("Waiters: ");
			foreach (WaiterData w in waiters)
			{
				Debug.Log(w.ToString());
			}
			Debug.Log("Chefs: ");
			foreach (ChefData c in chefs)
			{
				Debug.Log(c.ToString());
			}
		}
	}

	public void PrintAll()
	{
		PrintEmployees();
		Debug.Log("Decor:");
		foreach (DecorationData d in decor)
		{
			Debug.Log(d.ToString());
		}
	}

	//this is income per min, based on the current employees
	public int GetTotalIncome()
	{
		int totalIncome = 0;
		foreach (WaiterData w in waiters)
		{
			totalIncome += w.income;
		}
		foreach (ChefData c in chefs)
		{
			totalIncome += c.income;
		}
		return totalIncome;
	}
	
}

[System.Serializable]
public class KitchenData 
{
	//keeps track of the available utensils and what upgrades they are at
	public List<int> utensils;

}

[System.Serializable]
public class WaiterData 
{

	//the name of the cat
	public string name;

	//the rarity of the cat 0 is lowest, increasing number is increasing rarity
	public int rarity;

	//number of times trained
	public int timesTrained;

	//keeps the sprites for the body, face, and accessory on the cat
	public List<string> sprites;

	//the amount this cat makes per hour
	public int income;

	//the time the cat started training and the time the cat's training should end
	public DateTime trainStartTime;
	public DateTime trainEndTime;
	public bool isTraining;

	public int catNumber;

	//constructor
	public WaiterData(string n, int r, List<string> s, int i)
	{
		name = n;
		rarity = r;
		//when initializing the waiter, it will always have been trained 0 times
		timesTrained = 0;
		isTraining = false;
		sprites = s;
		income = i;
	}

	public string ToString()
	{
		string s = "Waiter \nName: " + name + "\n" +
				"Rarity: " + rarity + "\n" +
				"timesTrained: " + timesTrained + "\n" +
				"Income: " + income;

		return s;
	}

}

[System.Serializable]
public class ChefData
{

	//the name of the cat
	public string name;

	//the rarity of the cat 0 is lowest, increasing number is increasing rarity
	public int rarity;

	//number of times trained
	public int timesTrained;

	//keeps the sprites for the body, face, and accessory on the cat
	public List<string> sprites;

	//the amount this cat makes per hour
	public int income;

	//the best dish type the chef makes
	public List<RestaurantType> specialties;

	//the time the cat started training and the time the cat's training should end
	public DateTime trainStartTime;
	public DateTime trainEndTime;
	public bool isTraining;

	public int catNumber;

	//constructor
	public ChefData(string n, int r, List<string> s, int i, List<RestaurantType> sp)
	{
		name = n;
		rarity = r;
		//when initializing the chef, it will always have been trained 0 times
		timesTrained = 0;
		sprites = s;
		income = i;
		isTraining = false;
		specialties = sp;
	}

	public string ToString()
	{
		string s = "Chef \nName: " + name + "\n" +
				"Rarity: " + rarity + "\n" +
				"timesTrained: " + timesTrained + "\n" +
				"Income: " + income;

		return s + "\n" + SpecialtiesToString();
	}

	public string SpecialtiesToString()
	{
		string s = "Specialties: None";
		if (specialties.Count > 0)
		{
			s = "Specialties: " + specialties[0];
			if (specialties.Count == 2)
			{
				s = s + ", " + specialties[1];
			}
		}
		return s;
	}

	public void AddRandomRestaurantType()
	{
		int i = (int) RestaurantType.NumOfRestaurantTypes;
		i -= specialties.Count; //need to subtract the number of specialties we currently have
		//if we already have all the restaurant types, don't add anything
		if (i == 0)
		{
			return;
		}
		int n = 0;	//keeps track of the enum we want from the random number
		//to get a random int that we will check from; if it returns j, we will get the jth specialty that we don't already have
		int rand = UnityEngine.Random.Range(0, i);
		int cur = 0; //the current index we're looking at

		//going through the different restaurant types
		while (n <= rand)
		{
			//if we already have this type, then move onto the next one
			if (specialties.Contains((RestaurantType)cur))
			{
				cur++;
			}
			//if we don't have this type, see if it's in the position that was randomly picked
			else if (n == rand)
			{
				Debug.Log((RestaurantType)cur);
				specialties.Add((RestaurantType)cur);
				return;
			}
			//otherwise, increment n since we passed one candidate
			else
			{
				n++;
				cur++;
			}
		}

	}

}

[System.Serializable]
public class DecorationData
{
	public int id;

	public string name;

	public string description;

	public int starLevel;

	//cost to buy this decoration
	public int cost;

	//the amount it contributes to the atmosphere
	public int atmosphere;

	public string sprite;

	public int numInInventory;

	public DecorationLocation location;

	public DecorationData(XmlNode d, int s, string loc)
	{
		if (d.Attributes["id"].Value != "")
        {
            id = int.Parse(d.Attributes["id"].Value);
        }
        name = d.Attributes["name"].Value;
		description = d.Attributes["text"].Value;
        if (d.Attributes["price"].Value != "")
        {
            cost = int.Parse(d.Attributes["price"].Value);
        }
		if (d.Attributes["atmosphere"].Value != "")
        {
            atmosphere = int.Parse(d.Attributes["atmosphere"].Value);
        }
		sprite = d.Attributes["image"].Value;
		starLevel = s;
		location = LocationFromString(loc);
	}

	private DecorationLocation LocationFromString(string s)
	{
		switch (s)
		{
			case "Wall":
				return DecorationLocation.Wall;
				break;
			case "Floor":
				return DecorationLocation.Floor;
				break;
			case "Table":
				return DecorationLocation.Table;
				break;
			default:
				return DecorationLocation.Wall;
				break;
		}
	}

	public bool IsEqual(DecorationData d) {
		return this.name == d.name;
	}

	public string ToString()
	{
		return "id: " + id + " name: " + name + " price: " + cost + " atmosp: " + atmosphere + "Location: " + location + "\n" +description;
	}

}


public class RecipeData
{
    public int idNum;

    public string itemType;

    public string recipeName;

    public List<Ingredients> ingredients;

    //the utensils have an order; like needing to chop up food before putting it into the oven
    public CookingTools utensils;

    public Dishes dish;

    public int price;

    public RestaurantType foodType;

	public string sprite;

	public RecipeData(XmlNode recipe, string type)
    {
        if (recipe.Attributes["id"].Value != "")
        {
            idNum = int.Parse(recipe.Attributes["id"].Value);
        }
        recipeName = recipe.Attributes["name"].Value;
        if (recipe.Attributes["price"].Value != "")
        {
            price = int.Parse(recipe.Attributes["price"].Value);
        }

        ingredients = new List<Ingredients>();
        foreach (XmlNode ingred in recipe.SelectNodes("ingredient"))
        {
            if (ingred.Attributes["name"].Value != "")
            {
                ingredients.Add(GetXmlIngredient(ingred.Attributes["name"].Value));
            }
        }
        
        utensils = GetXmlCookingTool(recipe.SelectSingleNode("cookingTool").Attributes["name"].Value);

        itemType = type;
    }

    private Ingredients GetXmlIngredient(string s)
    {
        switch (s)
        {
            case "Carrot":
                return Ingredients.Carrot;
                break;
                
            case "Lettuce":
                return Ingredients.Lettuce;
                break;

            case "Chicken":
                return Ingredients.Chicken;
                break;

            case "Beef":
                return Ingredients.Beef;
                break;

            default:
                return Ingredients.Carrot;
                break;
        }
    }

    private CookingTools GetXmlCookingTool(string s)
    {
        switch (s)
        {
            case "Knife":
                return CookingTools.Knife;
                break;
            case "Oven":
                return CookingTools.Oven;
                break;
            case "Stove":
                return CookingTools.Stove;
                break;
            default:
                return CookingTools.Knife;
                break;
        }
    }

    public string ToString()
    {
        string s = "id: " + idNum + " name: " + recipeName + " price: " + price + " type: " + itemType;
        s += "\ningredients: ";
        foreach (Ingredients i in ingredients)
        {
            s += "\n" + i;
        }
        s += "\nCooking tool: " + utensils;
        return s;
    }

}