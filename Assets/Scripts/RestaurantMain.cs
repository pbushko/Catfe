using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State {upgrades, popup, gameplay}

public class RestaurantMain : MonoBehaviour {

	public static RestaurantMain restMain;

	public GameObject ingredientBox;
	public GameObject ingredientLine;

	public GameObject utensilLine;

	private State currentState;
	private PlayerScript playerScript;
	private CustomerGenerator customerGenerator;

	private static List<Sprite> m_utensilSprites;
	private static List<string> m_utensilSpriteNames;
	private static List<Sprite> m_ingredientSprites;
	private static List<string> m_ingredientSpriteNames;

	private Vector3 m_ingredientLinePosition;
	private Vector3 m_utensilLinePosition;

	public static List<Ingredients> ingredients;
	//cooking utensil prefabs
	public static List<GameObject> utensils;

	public GameObject popUp;
	public GameObject curPopUp;
	public Text popUpText;

	private Collider2D curUtensil;

	// Use this for initialization
	void Start () {
		restMain = this;
		//always start the scene with buying upgrades
		currentState = State.upgrades;

		m_utensilSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Kitchen Utensils"));
		m_utensilSpriteNames = new List<string>();
		m_ingredientSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Ingredients"));
		m_ingredientSpriteNames = new List<string>();

		m_ingredientLinePosition = ingredientLine.transform.position;
		m_utensilLinePosition = utensilLine.transform.position;

		popUpText.enabled = false;

		//the game is paused in the upgrades state
		playerScript = GameObject.Find("Cat").GetComponent<PlayerScript>();
        customerGenerator = GameObject.Find("Customer Line").GetComponent<CustomerGenerator>();
		Pause();

		foreach (Sprite s in m_utensilSprites)
        {
            m_utensilSpriteNames.Add(s.name);
        }
		foreach (Sprite s in m_ingredientSprites)
        {
            m_ingredientSpriteNames.Add(s.name);
        }

		setIngredientBoxes();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
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
		
	}

	//either enable or disable to popup
	private void setPopUp(bool t)
	{
		if (t)
		{
			popUpText.enabled = true;
			curPopUp = (GameObject)Instantiate(popUp);
		}
		else
		{
			popUpText.enabled = false;
			Destroy(curPopUp);
		}
	}

	public void Pause()
    {
        playerScript.enabled = !playerScript.enabled;
        customerGenerator.enabled = !customerGenerator.enabled;
    }

	public void FinishedUpgrades() {
		currentState = State.gameplay;
		Destroy(GameObject.Find("UpgradesFinishedButton"));
		setPopUp(false);
		Pause();
		//adding the upgrades purchased to the save file, doesn't auto save it here
		//only saves once the level has been completed
		PlayerData.playerData.utensils = utensils;
	}

	public static void AddMoney(int n)
	{
		PlayerData.playerData.playerMoney += n;
		MoneyTracker.ChangeMoneyCount();
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
		int n = utensils.Count;
		//setting each box
		for(int i = 0; i < n; i++)
		{
			GameObject utensil = (GameObject)Instantiate(utensils[i]);
			//setting the location
			utensil.transform.SetParent(utensilLine.transform);

			utensil.transform.position = 
            	new Vector3(m_utensilLinePosition.x + (i * Variables.UTENSIL_OFFSET), m_utensilLinePosition.y, m_utensilLinePosition.z);
		}
	}

}
