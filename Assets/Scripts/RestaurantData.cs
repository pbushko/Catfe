using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum RestaurantType { Catfe, Pizzaria, Sandwich, Burger, Asian, Indian, Bakery, NumOfRestaurantTypes };

//the class to store the data for each store you own
[System.Serializable]
public class RestaurantData 
{
	//keeps track of the number of tables and the current upgrade count on them
	public List<int> tables;

	//the type of the restuarant
	public RestaurantType type;

	//keeps track of your employees
	public List<WaiterData> waiters;
	public List<ChefData> chefs;

	//the decor in the shop
	public List<DecorationData> decor;

	//the number of stars and portion of a star of the restaurant
	public int stars;
	public float starProgress;

	//the time the restaurant was opened to get idle income
	public long timeOpened;

	//the last time the minigame was played in the restaurant
	public long timeMinigamePlayed;

	//keeps track of the number of minigames completed in that shop
	//used to determine if the restaurant can be level up or not
	public int minigamesCompleted;
	
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
	public List<Sprite> sprites;

	//the amount this cat makes per hour
	public int income;

	//constructor
	public WaiterData(string n, int r, List<Sprite> s, int i)
	{
		name = n;
		rarity = r;
		//when initializing the waiter, it will always have been trained 0 times
		timesTrained = 0;
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
	public List<Sprite> sprites;

	//the amount this cat makes per hour
	public int income;

	//the best dish type the chef makes
	public List<RestaurantType> specialties;

	//constructor
	public ChefData(string n, int r, List<Sprite> s, int i, List<RestaurantType> sp)
	{
		name = n;
		rarity = r;
		//when initializing the chef, it will always have been trained 0 times
		timesTrained = 0;
		sprites = s;
		income = i;
		specialties = sp;
	}

	public string ToString()
	{
		string s = "Chef \nName: " + name + "\n" +
				"Rarity: " + rarity + "\n" +
				"timesTrained: " + timesTrained + "\n" +
				"Income: " + income;
		if (specialties.Count > 0)
		{
			s = s + "\n" + "Specialties: " + specialties[0];
			if (specialties.Count == 2)
			{
				s = s + ", " + specialties[1];
			}
		}

		return s;
	}

}

[System.Serializable]
public class DecorationData
{
	//cost to buy this decoration
	public int cost;

	//the amount it contributes to the atmosphere
	public int atmosphere;

}