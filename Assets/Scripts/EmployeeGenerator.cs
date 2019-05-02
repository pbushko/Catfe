using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EmployeeGenerator {

	//the name of the cat
	private static string name;
	private static int rarity;
	private static int income;

	//keeps the sprites for the body, face, and accessory on the cat
	private static List<string> sprites;
	//the best dish type the chef makes
	private static List<RestaurantType> specialties;
	//the names for cats
	private static List<string> names;

	private static List<Sprite> bodies = null;
	private static List<Sprite> faces = null;

	public static WaiterData GenerateWaiter()
	{
		GenerateSharedData();
		return new WaiterData(name, rarity, sprites, income);
	}

	public static ChefData GenerateChef()
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

	private static void GenerateSharedData()
	{
		//get a name from the file of potential names:
		GetNames(Application.dataPath + "/" + "cat_names.txt");
		//for now, just lb
		name = names[Random.Range(0, names.Count)];

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
		sprites = new List<string>();
		GenerateSprites();
		
	}

	private static RestaurantType GetRandomRestaurantType()
	{
		int i = (int) RestaurantType.NumOfRestaurantTypes;
		int rand = UnityEngine.Random.Range(0, i);

		return (RestaurantType)rand;
	}

	private static void GenerateSprites()
	{
		//first generate the body, then generate the face
		//if the body and face sprites haven't been separated yet, do that now
		if (bodies == null || faces == null)
		{
			List<Sprite> temp = new List<Sprite>(Resources.LoadAll<Sprite>("Katt"));
			bodies = temp.FindAll(FindBodies);
			faces = temp.FindAll(FindFaces);
		}

		//get a random face and body to add to the cat data
		sprites.Add(bodies[Random.Range(0, bodies.Count)].name);
		sprites.Add(faces[Random.Range(0, faces.Count)].name);
	}

	//helper methods to separate the bodies from the faces
	private static bool FindBodies(Sprite s)
	{
		return s.name.Contains("coat_");
	}
	private static bool FindFaces(Sprite s)
	{
		return s.name.Contains("face_");
	}

	private static void GetNames(string file_path)
	{
		//only do this if not done before
		if(names == null)
		{
			TextAsset txt = (TextAsset)Resources.Load("cat_names", typeof(TextAsset));
			string allNames = txt.ToString();

			names = new List<string>();
			using (System.IO.StringReader reader = new System.IO.StringReader(allNames))
			{
				string name;
				name = reader.ReadLine();
				while (name != null)
				{
					names.Add(name);
					name = reader.ReadLine();
				}
			}
		}
	}

}
