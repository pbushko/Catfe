using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State {upgrades, popup, gameplay}

public class RestaurantMain : MonoBehaviour {

	private State currentState;
	private PlayerScript playerScript;
	private CustomerGenerator customerGenerator;

	public static int playerMoney;

	private static List<Sprite> m_utensilSprites;
	private static List<string> m_utensilSpriteNames;

	public GameObject popUp;
	public GameObject curPopUp;
	public Text popUpText;

	private Collider2D curUtensil;

	// Use this for initialization
	void Start () {
		//always start the scene with buying upgrades
		currentState = State.upgrades;

		m_utensilSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Kitchen Utensils"));
		m_utensilSpriteNames = new List<string>();

		playerMoney = 100;
		MoneyTracker.ChangeMoneyCount();

		popUpText.enabled = false;

		//the game is paused in the upgrades state
		playerScript = GameObject.Find("Cat").GetComponent<PlayerScript>();
        customerGenerator = GameObject.Find("Customer Line").GetComponent<CustomerGenerator>();
		Pause();

		foreach (Sprite s in m_utensilSprites)
        {
            m_utensilSpriteNames.Add(s.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hit = Physics2D.Raycast(mousePos, Vector2.up).collider;
			if (hit)
			{
				//Vector3 loc = hit.collider.transform.position;
				//nothing should happen unless a utensil is clcicked
				// if (hit.collider.tag == "ingredients")
				// {
				// 	hit.collider.GetComponent<BoxScript>().OnClick();
				// 	m_locations.Enqueue(new Vector3(loc.x, loc.y + 2, loc.z));
				// }
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
						if (playerMoney >= temp.GetUpgradeCost())
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
	}

	public static void AddMoney(int n)
	{
		playerMoney += n;
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
}
