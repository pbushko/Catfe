using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum State {upgrades, popup, gameplay, finished}

public class RestaurantMain : MonoBehaviour {

	public static RestaurantMain restMain;

	public GameObject ingredientBox;
	public GameObject ingredientLine;

	public GameObject utensilLine;

	public State currentState;
	public PlayerScript playerScript;
	public CustomerGenerator customerGenerator;

	private static List<Sprite> m_utensilSprites;
	private static List<string> m_utensilSpriteNames;
	private static List<Sprite> m_ingredientSprites;
	private static List<string> m_ingredientSpriteNames;

	private Vector3 m_ingredientLinePosition;
	private Vector3 m_utensilLinePosition;

	public static List<Ingredients> ingredients;

	public GameObject popUp;
	public Text popUpText;
	public GameObject UpgradesFinishedButton;

	private Collider2D curUtensil;

	private DateTime minigameEndTime;
	public Text minigameTimeLeft;

	public GameObject minigameItems;

	public GameObject cat;
	private Vector3 catStartingPos;

	// Use this for initialization
	void Start () {
		restMain = this;

		m_utensilSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Kitchen Utensils"));
		m_utensilSpriteNames = new List<string>();
		m_ingredientSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Ingredients"));
		m_ingredientSpriteNames = new List<string>();

		m_ingredientLinePosition = ingredientLine.transform.position;
		m_utensilLinePosition = utensilLine.transform.position;

		popUpText.enabled = false;

		//this is temporary
		ingredients = new List<Ingredients>();
		ingredients.Add(Ingredients.Lettuce);
		ingredients.Add(Ingredients.Chicken);
		ingredients.Add(Ingredients.Carrot);
		ingredients.Add(Ingredients.Beef);

		catStartingPos = cat.transform.position;

		//the game is paused in the upgrades state
		Pause();

		foreach (Sprite s in m_utensilSprites)
        {
            m_utensilSpriteNames.Add(s.name);
        }
		foreach (Sprite s in m_ingredientSprites)
        {
            m_ingredientSpriteNames.Add(s.name);
        }

		Reset();
	}

	public void Reset()
	{
		//always start the scene with buying upgrades
		currentState = State.upgrades;
		cat.transform.position = catStartingPos;
		setIngredientBoxes();
		setUtensils();
		playerScript.Reset();
		customerGenerator.Reset();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == State.finished)
		{
			Reset();
		}
		else if (currentState != State.gameplay && Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hit = Physics2D.Raycast(mousePos, Vector2.up).collider;
			if (hit)
			{
				//access the utensil and check if the player wants to upgrade it
				if (currentState == State.upgrades)
				{
					if(hit.tag == "utensil")
					{
						//will go into choosing to upgrade or not on the popup
						currentState = State.popup;
						setPopUp(true);
						curUtensil = hit;
					}
				}
				if (currentState == State.popup)
				{
					//don't want to buy the upgrade
					if (hit.tag == "noButton")
					{
						currentState = State.upgrades;
						setPopUp(false);
					}
					else if (hit.tag == "yesButton")
					{
						currentState = State.upgrades;
						CookingUtensilsScript temp = curUtensil.GetComponent<CookingUtensilsScript>();
						if (PlayerData.playerData.playerMoney >= temp.GetUpgradeCost())
						{
							temp.Upgrade();
							curUtensil = null;
						}
						else
						{
							Debug.Log("not enough money!");
						}
						setPopUp(false);
					}
				}
			}
		}
		else if (currentState == State.gameplay && DateTime.Compare(DateTime.Now, minigameEndTime) > 0)
		{
			minigameTimeLeft.text = "time up!";
			currentState = State.finished;
			Pause();
			minigameItems.SetActive(false);
			CatfePlayerScript.script.EnterRestaurant();
		}
		else if (currentState == State.gameplay)
		{
			TimeSpan timeLeft = minigameEndTime.Subtract(DateTime.Now);
			minigameTimeLeft.text = "min: " + timeLeft.Minutes + " sec: " + timeLeft.Seconds;
		}
		
	}

	//either enable or disable to popup
	private void setPopUp(bool t)
	{
		if (t)
		{
			popUpText.enabled = true;
			popUp.SetActive(true);
		}
		else
		{
			popUpText.enabled = false;
			popUp.SetActive(false);
		}
	}

	public void Pause()
    {
        playerScript.enabled = !playerScript.enabled;
        customerGenerator.enabled = !customerGenerator.enabled;
    }

	public void FinishedUpgrades() 
	{
		currentState = State.gameplay;
		UpgradesFinishedButton.SetActive(false);
		setPopUp(false);
		Pause();

		//need to set the newly upgraded utensils to be saved
		List<CookingUtensil> cs = new List<CookingUtensil>();
		for (int i = 0; i < utensilLine.transform.childCount; i++)
		{
			cs.Add(utensilLine.transform.GetChild(i).gameObject.GetComponent<CookingUtensilsScript>().utensil);
		}
		PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().data.utensils = cs;

		//setting the timer for the minigame, just 10s for now
		minigameEndTime = DateTime.Now.AddSeconds(30f);
	}

	public static void AddMoney(int n)
	{
		MoneyTracker.ChangeMoneyCount(n);
	}

	public static Sprite GetUpgradeSprite(Sprite sprite)
	{
		string s = sprite.name;
		//getting the number of the utensil
		int num = (int)char.GetNumericValue(s[s.Length - 1]);
		num ++;
		string temp = s.Substring(0, s.Length - 1) + num;
		Debug.Log(temp);
		Sprite toRet = m_utensilSprites[m_utensilSpriteNames.IndexOf(temp)];
		if (toRet != null)
		{
			return toRet;
		}
		else
		{
			return sprite;
		}
	}

	//takes the type of the cooking utensil and the number of times it was upgraded
	public static Sprite GetCookingUtenSprite(CookingTools c, int i)
	{
		switch (c)
		{
			case CookingTools.Knife:
				return m_utensilSprites[m_utensilSpriteNames.IndexOf("Knife_" + i)];
				break;
			case CookingTools.Stove:
				return m_utensilSprites[m_utensilSpriteNames.IndexOf("Oven_" + i)];
				break;
			default:
				return m_utensilSprites[m_utensilSpriteNames.IndexOf("Knife_0")];
				break;
		}
	}

	//set up the ingredient boxes for the level
	private void setIngredientBoxes()
	{
		int n = ingredients.Count;
		//setting each box
		for(int i = 0; i < n; i++)
		{
			GameObject box = (GameObject)Instantiate(ingredientBox);
			//setting the location
			box.transform.SetParent(ingredientLine.transform);

			//setting the ingredient
			box.GetComponent<BoxScript>().ingredient = ingredients[i];
			box.transform.position = 
            	new Vector3(m_ingredientLinePosition.x + (i * Variables.INGREDIENT_OFFSET), m_ingredientLinePosition.y, m_ingredientLinePosition.z);

			SpriteRenderer s = box.transform.GetChild(0).GetComponent<SpriteRenderer>();
			//setting the sprite
			switch (ingredients[i]) 
			{
				case Ingredients.Carrot:
					s.sprite = m_ingredientSprites[m_ingredientSpriteNames.IndexOf("Carrot")];
					break;
				case Ingredients.Lettuce:
					s.sprite = m_ingredientSprites[m_ingredientSpriteNames.IndexOf("Lettuce")];
					break;
				case Ingredients.Beef:
					s.sprite = m_ingredientSprites[m_ingredientSpriteNames.IndexOf("Beef")];
					break;
				case Ingredients.Chicken:
					s.sprite = m_ingredientSprites[m_ingredientSpriteNames.IndexOf("Chicken")];
					break;
				default:
					s.sprite = m_ingredientSprites[m_ingredientSpriteNames.IndexOf("Carrot")];
					break;
			}
		}
	}

	private void setUtensils()
	{
		List<CookingUtensil> cs = PlayerData.playerData.activeRestaurant.GetComponent<Restaurant>().data.utensils;
		UpgradesFinishedButton.SetActive(true);
		//set each kitchen utensil based on what the restaurant has
		if (cs != null && cs.Count > 0)
		{
			for (int i = 0; i < cs.Count; i++)
			{
				//if for some reason too much is saved, make sure we only go through possible things
				if (i > utensilLine.transform.childCount)
				{
					i = cs.Count;
				}
				else
				{
					CookingUtensilsScript u = utensilLine.transform.GetChild(i).gameObject.GetComponent<CookingUtensilsScript>();
					u.utensil = cs[i];
					//getting the right sprite to show
					if(cs[i].upgradeNum > 3)
					{
						cs[i].upgradeNum = 3;
					}
					u.SetSprite(GetCookingUtenSprite(cs[i].utensil, cs[i].upgradeNum));
					u.loader.PickUpPlate();
				}
			}
		}
		//if there are no utensils, use the default ones, a knife and a stove
		else 
		{
			CookingUtensilsScript u = utensilLine.transform.GetChild(0).gameObject.GetComponent<CookingUtensilsScript>();
			u.utensil = new CookingUtensil(CookingTools.Knife);
			u.SetSprite(GetCookingUtenSprite(CookingTools.Knife, 0));
			cs.Add(u.utensil);
			u.loader.PickUpPlate();
			
			u = utensilLine.transform.GetChild(1).gameObject.GetComponent<CookingUtensilsScript>();
			u.utensil = new CookingUtensil(CookingTools.Stove);
			u.SetSprite(GetCookingUtenSprite(CookingTools.Stove, 0));
			cs.Add(u.utensil);
			u.loader.PickUpPlate();
		}
	}

}
