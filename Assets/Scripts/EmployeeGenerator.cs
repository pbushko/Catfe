using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeGenerator {

	//the name of the cat
	string name;
	int rarity;
	int income;

	//keeps the sprites for the body, face, and accessory on the cat
	List<Sprite> sprites;
	//the best dish type the chef makes
	List<RestaurantType> specialties;

	public WaiterData GenerateWaiter()
	{
		GenerateSharedData();
		return new WaiterData(name, rarity, sprites, income);
	}

	public ChefData GenerateChef()
	{
		GenerateSharedData();
		specialties = new List<RestaurantType>();

		//the number of specialties that come default dedpend on the rarity
		//the # of specialites is rarity - 1, so 2 star gets 1, 3 star gets 2, etc.
		for (int i = rarity; i > 1; i--)
		{
			RestaurantType temp = GetRandomRestaurantType();
			//this is to prevent having the same specialty twice
			while (specialties.Contains(temp))
			{
				temp = GetRandomRestaurantType();
			}
			specialties.Add(temp);
		}

		return new ChefData(name, rarity, sprites, income, specialties);

	}

	private void GenerateSharedData()
	{
		//get a name from the file of potential names:
		//for now, just lb
		name = "lb";

		//randomly get a rarity, harder to get higher rarities
		float rand = Random.Range(0f, 1.0f);
		if (rand <= 0.5f)
		{
			rarity = 0;
			income = 50;
		}
		else if (rand <= 0.8f)
		{
			rarity = 1;
			income = 100;
		}
		else if (rand <= 0.95f)
		{
			rarity = 2;
			income = 175;
		}
		else
		{
			rarity = 3;
			income = 300;
		}

		//putting a range on the income
		income = (int) (income * (1f + rand));

		//get sprites for the cat

		
	}

	private RestaurantType GetRandomRestaurantType()
	{
		int i = (int) RestaurantType.NumOfRestaurantTypes;
		int n = 0;	//keeps track of the current enum we're checking
		float rand = Random.Range(0f, 1.0f);
		float interval = 1f/i;
		float cur = interval;

		//going through the different restaurant types
		while (n < i)
		{
			//start by checking if the random num is less than the smallest interval and slowly building up
			if (rand < cur)
			{
				return (RestaurantType)n;
			}
			//if not in the range, try the next enum range
			n++;
			cur += interval;
		}
		//if for some reason nothing was found, just return the catfe type
		return RestaurantType.Catfe;
	}

}
