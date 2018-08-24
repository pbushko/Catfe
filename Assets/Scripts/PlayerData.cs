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
	public List<DecorationData> allDecor;

	//lists of things to save
	public List<ChefData> chefs;
	public List<WaiterData> waiters;
	public List<RestaurantData> restaurants;
	public List<DecorationData> purchasedDecor;

	public GameObject activeRestaurant;

	public GameObject decorToBuy;

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
		LoadRecipes();
		LoadDecor();
		SetDecorToBuy();
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
		//purchasedDecor = new List<DecorationData>();
		
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

		if (recipiesText != null)
		{
			XmlDocument recipesXml = new XmlDocument();
			recipesXml.LoadXml(recipiesText.text);

			recipes = new List<Recipe>();

			foreach (XmlNode type in recipesXml.SelectSingleNode("recipes").SelectNodes("foodType"))
			{
				foreach(XmlNode star in type.SelectNodes("star"))
				{
					foreach (XmlNode recipe in star.SelectNodes("recipe"))
					{
						recipes.Add(new Recipe(recipe, type.Attributes["type"].Value));
					}
				}
			}
			/*foreach (Recipe r in recipes)
			{
				Debug.Log(r.ToString());
			}*/
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
				foreach(XmlNode star in type.SelectNodes("star"))
				{
					foreach (XmlNode decor in star.SelectNodes("decor"))
					{
						allDecor.Add(new DecorationData(decor));
					}
				}
			}
			/*foreach (DecorationData d in allDecor)
			{
				Debug.Log(d.ToString());
			}*/
		}
		else
		{
			Debug.Log("Couldn't load the decor.");
		}
	}

	//used to populate the store with its decor items
	public void SetDecorToBuy()
	{
		Debug.Log(decorToBuy.transform.childCount);
		for (int i = 0; i < decorToBuy.transform.childCount; i++)
		{
			decorToBuy.transform.GetChild(i).GetComponent<Decoration>().data = allDecor[i];
		}
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
