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

	public List<GameObject> allNotPurchasedRecipeGameObjects;

	//lists of things to save
	public List<ChefData> chefs;
	public List<WaiterData> waiters;
	public List<RestaurantData> restaurants;
	public List<DecorationData> purchasedDecor;
	public List<Recipe> purchasedRecipes;

	public GameObject decorToBuy;
	public GameObject decorSlots;
	public GameObject recipesToBuy;
	public GameObject recipeSlots;

	private static Dictionary<string, Dictionary<string, Sprite>> m_sprites;

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

		m_sprites = new Dictionary<string, Dictionary<string, Sprite>>();

		catSprites = new List<Sprite>(Resources.LoadAll<Sprite>("cats"));
		catSpriteNames = new List<string>();
		Dictionary<string, Sprite> temp = new Dictionary<string, Sprite>();
		foreach (Sprite s in catSprites)
		{
			catSpriteNames.Add(s.name);
			temp.Add(s.name, s);
		}
		m_sprites.Add("cat", temp);
		//loading the food sprites
		List<Sprite> m_foods = new List<Sprite>(Resources.LoadAll<Sprite>("Foods"));
		//Adding the food names to allow us to search for the sprite
		Dictionary<string, Sprite> foodTemp = new Dictionary<string, Sprite>();
        foreach (Sprite s in m_foods)
        {
            m_foodNames.Add(s.name);
			foodTemp.Add(s.name, s);
        }
		m_sprites.Add("food", foodTemp);
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
		//data.purchasedDecor = purchasedDecor;

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
			//purchasedDecor = data.purchasedDecor;

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
			//purchasedDecor = new List<DecorationData>();

			Cafe temp = new Cafe();
			temp.NewGameRestaurantChoice();
		}
		
		//restaurants = new List<RestaurantData>();	
	}

	public Sprite GetCatSprite(string spriteName)
	{
		return m_sprites["cat"][spriteName];
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

	public static Sprite GetFoodSprite(string s)
    {
		Sprite value;
		if (s != null && m_sprites["food"].TryGetValue(s, out value))
		{
			return value;
		}
		return m_sprites["food"]["None"];
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
	//public List<DecorationData> purchasedDecor;
	//any other game objects that will be saved...

}
