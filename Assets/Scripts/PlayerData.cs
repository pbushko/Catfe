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

	//the number of times the utensils have been upgraded
	public List<int> utensils;
	//the ingredients that have been unlocked? -> do we need this?
	public List<Ingredients> ingredients;
	public List<Recipe> recipies;
	public List<GameObject> catEmployees;
	public List<GameObject> catToys;

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
		data.utensils = utensils;
		data.ingredients = ingredients;
		data.catEmployees = catEmployees;
		data.catToys = catToys;

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
			utensils = data.utensils;
			ingredients = data.ingredients;
			catEmployees = data.catEmployees;
			catToys = data.catToys;

			file.Close();
		}
		//otherwise, load the base data
		else
		{
			playerMoney = 100;
			
			//starting ingreds are carrots and lettuce
			ingredients = new List<Ingredients>();
			ingredients.Add(Ingredients.Carrot);
			ingredients.Add(Ingredients.Lettuce);
		}
	}
}

//stores all the data that the game has to keep track of between levels
[System.Serializable]
class SaveData {

	//the money the player currently has; will carry over from level to level
	public int money;

	//the setup of the restaurant
	public List<int> utensils;

	//the ingredients that can be used for the level
	public List<Ingredients> ingredients;

	//recipies that are unlocked
	public List<Recipe> recipies;

	//cat employees and toys for the catfe levels
	public List<GameObject> catEmployees;
	public List<GameObject> catToys;

	//any other game objects that will be saved...


}
