using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerData : MonoBehaviour
{
	private static string SAVE_LOCATION;

	public static PlayerData playerData;

	public int playerMoney;

	public List<Sprite> catSprites;
	public List<string> catSpriteNames;

	//these are just things to load in every time the game opens
	public List<Recipe> recipes;
	public List<Recipe> minigameRecipes;
	public Recipe slop;
	public List<DecorationData> allDecor;
	public List<GameObject> allNotPurchasedDecorGameObjects;

	//lists of things to save
	public List<ChefData> chefs;
	public List<WaiterData> waiters;
	public List<RestaurantData> restaurants;
	public List<DecorationData> purchasedDecor;
	public List<Recipe> purchasedRecipes;

	public GameObject decorToBuy;
	public GameObject decorSlots;
	public GameObject recipesToBuy;

	private static List<Sprite> m_foods;
    private static List<string> m_foodNames = new List<string>();

	void Awake()
	{
		SAVE_LOCATION = Application.persistentDataPath + "/neko.dat";
		
		if (playerData == null)
		{
			DontDestroyOnLoad(gameObject);
			playerData = this;
			Load();
		}	
		else if (playerData != this)
		{
			Destroy(gameObject);
		}

		catSprites = new List<Sprite>(Resources.LoadAll<Sprite>("cats"));
		catSpriteNames = new List<string>();
		foreach (Sprite s in catSprites)
		{
			catSpriteNames.Add(s.name);
		}
		//loading the food sprites
		m_foods = new List<Sprite>(Resources.LoadAll<Sprite>("Foods"));
		//Adding the food names to allow us to search for the sprite
        foreach (Sprite s in m_foods)
        {
            m_foodNames.Add(s.name);
        }
		LoadRecipes();
		LoadDecor();
		SetDecorToBuy();
		SetRecipesToBuy();
	}

	void OnApplicationQuit()
	{
		Save();
	}

	public void Save()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream file = File.Create(SAVE_LOCATION);

		SaveData data = new SaveData();
		data.money = playerMoney;
		data.chefs = chefs;
		data.waiters = waiters;
		data.restaurants = restaurants;
		data.purchasedDecor = purchasedDecor;

		formatter.Serialize(file, data);

		file.Close();
	}

	public void Load()
	{
		//if there is data, then load it in
		if (File.Exists(SAVE_LOCATION))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(SAVE_LOCATION, FileMode.Open);

			SaveData data = (SaveData) formatter.Deserialize(file);

			playerMoney = 1000;
			chefs = data.chefs;
			waiters = data.waiters;
			restaurants = data.restaurants;
			purchasedDecor = data.purchasedDecor;

			if(restaurants == null || restaurants.Count == 0)
			{
				Cafe temp = new Cafe();
				temp.NewGameRestaurantChoice();		
			}

			file.Close();
		}
		//otherwise, load the base data
		else
		{
			chefs = new List<ChefData>();
			waiters = new List<WaiterData>();
			restaurants = new List<RestaurantData>();
			purchasedDecor = new List<DecorationData>();

			Cafe temp = new Cafe();
			temp.NewGameRestaurantChoice();
		}
		if (purchasedDecor == null)
		{
			purchasedDecor = new List<DecorationData>();
		}
		
		//restaurants = new List<RestaurantData>();	
	}

	public Sprite GetCatSprite(string spriteName)
	{
		return catSprites[catSpriteNames.IndexOf(spriteName)];
	}

	public void PrintEmployees()
	{
		Debug.Log("Chefs:");
		foreach (ChefData c in chefs)
		{
			Debug.Log(c.ToString());
		}
		Debug.Log("Waiters:");
		foreach (WaiterData w in waiters)
		{
			Debug.Log(w.ToString());
		}
	}

	private void LoadRecipes()
	{
		TextAsset recipiesText = (TextAsset)Resources.Load("recipes", typeof(TextAsset));

		minigameRecipes = new List<Recipe>();

		if (recipiesText != null)
		{
			XmlDocument recipesXml = new XmlDocument();
			recipesXml.LoadXml(recipiesText.text);

			recipes = new List<Recipe>();

			foreach (XmlNode type in recipesXml.SelectSingleNode("recipes").SelectNodes("foodType"))
			{
				string mealType = type.Attributes["type"].Value;
				foreach(XmlNode star in type.SelectNodes("star"))
				{
					int starLevel = int.Parse(star.Attributes["level"].Value);
					foreach (XmlNode recipe in star.SelectNodes("recipe"))
					{
						Recipe r = new Recipe(recipe, mealType, starLevel);
						if (r.idNum == 10 || r.idNum == 11 || r.idNum == 12)
						{
							minigameRecipes.Add(r);
						}
						if (r.idNum == 13)
						{
							slop = r;
						}
						recipes.Add(r);
					}
				}
			}
		}
		else
		{
			Debug.Log("Couldn't load the recipes.");
		}
	}

	private void LoadDecor()
	{
		TextAsset decorText = (TextAsset)Resources.Load("decorations", typeof(TextAsset));

		if (decorText != null)
		{
			XmlDocument decorsXml = new XmlDocument();
			decorsXml.LoadXml(decorText.text);

			allDecor = new List<DecorationData>();

			foreach (XmlNode type in decorsXml.SelectSingleNode("decorations").SelectNodes("type"))
			{
				string location = type.Attributes["type"].Value;
				foreach(XmlNode star in type.SelectNodes("star"))
				{
					int starLevel = int.Parse(star.Attributes["level"].Value);
					foreach (XmlNode decor in star.SelectNodes("decor"))
					{
						allDecor.Add(new DecorationData(decor, starLevel, location));
					}
				}
			}
		}
		else
		{
			Debug.Log("Couldn't load the decor.");
		}
	}

	//used to populate the store with its decor items
	public void SetDecorToBuy()
	{
		allNotPurchasedDecorGameObjects = new List<GameObject>();
		foreach (DecorationData d in allDecor)
		{
			//decorToBuy.transform.GetChild(i).GetComponent<Decoration>().data = allDecor[i];
			//making the decor panel and populating it
			bool wasFound = false;
			foreach (DecorationData pd in purchasedDecor)
			{
				if (d.IsEqual(pd))
				{
					wasFound = true;
					break;
				}
			}
			if (!wasFound)
			{
				GameObject newDecor = (GameObject)Instantiate(decorToBuy);
				newDecor.GetComponent<Decoration>().ResetData(d);
				newDecor.transform.SetParent(decorSlots.transform);
				allNotPurchasedDecorGameObjects.Add(newDecor);
			}
		}
	}

	public void SetRecipesToBuy()
	{
		for (int i = 0; i < recipesToBuy.transform.childCount; i++)
		{
			recipesToBuy.transform.GetChild(i).GetComponent<RecipePanelData>().data = recipes[i];
		}
	}

	public static Sprite GetFoodSprite(Recipe food)
    {
        if (food != null)
        {
            //need to find the food and then return its sprite if possible
            int i = m_foodNames.IndexOf(food.GetRecipeName());
            //compare against -1 since that is returned when not found
            if (i != -1)
            {
                return m_foods[i];
            }
        }
        //return no image if no match
        return m_foods[m_foodNames.IndexOf("None")];
    }

}


//stores all the data that the game has to keep track of between levels
[System.Serializable]
class SaveData {

	//the money the player currently has; will carry over from level to level
	public int money;

	//recipies that are unlocked
	public List<Recipe> recipies;

	public List<ChefData> chefs;
	public List<WaiterData> waiters;
	public List<RestaurantData> restaurants;
	public List<DecorationData> purchasedDecor;
	//any other game objects that will be saved...

}
