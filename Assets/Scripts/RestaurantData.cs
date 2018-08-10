using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum RestaurantType { Catfe, Italian, Sandwich, Burger, Asian, Indian, Bakery, NumOfRestaurantTypes };

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

	public int location;

	public RestaurantData(RestaurantType rt)
	{
		tables = new List<int>();
		type = rt;
		waiters = new List<WaiterData>();
		chefs = new List<ChefData>();
		decor = new List<DecorationData>();
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

}

[System.Serializable]
public class DecorationData
{
	//cost to buy this decoration
	public int cost;

	//the amount it contributes to the atmosphere
	public int atmosphere;

}