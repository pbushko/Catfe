using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerData : MonoBehaviour
{
	private static string SAVE_LOCATION;

	public static PlayerData playerData;

	public int playerMoney;

	public List<Sprite> catSprites;
	public List<string> catSpriteNames;

	public List<Recipe> recipies;
	public List<ChefData> chefs;
	public List<WaiterData> waiters;
	public List<RestaurantData> restaurants;

	public GameObject activeRestaurant;

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

			playerMoney = data.money;
			chefs = data.chefs;
			waiters = data.waiters;
			restaurants = data.restaurants;

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
			playerMoney = 100;
			chefs = new List<ChefData>();
			waiters = new List<WaiterData>();
			restaurants = new List<RestaurantData>();

			Cafe temp = new Cafe();
			temp.NewGameRestaurantChoice();	
			//Cafe.NewGameRestaurantChoice();	
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
	//any other game objects that will be saved...


}
