using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingUtensilScript : MonoBehaviour
{
    public CookingUtensil utensil;

    public Image objectSprite;

    // In the prefab
    public GameObject loader;
    public Image foodRenderer;

    private Image m_foregroundImage;
    private Sprite m_food;

    private bool m_finished;
    private bool m_processing;

    private Recipe m_recipe;
    private float m_fill;
    private float m_time;

    public void OnClick()
    {
        if (RestaurantMain.restMain.currentState == State.upgrades) 
        {
            if (utensil.upgradeNum >= Variables.MAX_UTENSIL_UPGRADE) {
                Debug.Log("It is the max upgrade already!");
            }
            else {
                RestaurantMain.restMain.UtensilUpgradeClicked(this);
            }
        }
        else if (RestaurantMain.restMain.currentState == State.gameplay)
        {
            PlayerScript.AddCookingToolToPlayerQueue(this);
            Vector3 loc = gameObject.transform.position;
            PlayerScript.AddLocation(new Vector3(loc.x - 0.5f, loc.y - 1, loc.z));
        }
    }

	// Use this for initialization
	void Start ()
    {
        loader.SetActive(false);

        m_foregroundImage = loader.transform.GetChild(1).gameObject.GetComponent<Image>();
        m_finished = false;
        m_processing = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_processing)
        {
            if (m_fill <= 1f)
            {
                m_fill += 0.01f/m_time;
            }
            else
            {
                m_processing = false;
                loader.SetActive(false);
                m_finished = true;
                
                foodRenderer.sprite = m_food;
            }
            m_foregroundImage.fillAmount = m_fill;
        }
	}

    public void Upgrade()
    {
        MoneyTracker.ChangeMoneyCount(-utensil.upgradeCost);
        utensil.upgradeNum++;
        utensil.cookTime /= 2;
        utensil.upgradeCost *= 5;
        objectSprite.sprite = RestaurantMain.GetUpgradeSprite(objectSprite.sprite);
    }

    public int GetUpgradeCost()
    {
        return utensil.upgradeCost;
    }

    public float GetCookTime()
    {
        return utensil.cookTime;
    }

    public void SetSprite(Sprite s)
    {
        objectSprite.sprite = s;
    }

    public void Loading(Sprite f, Recipe r)
    {
        if (!m_finished && !m_processing)
        {
            m_food = f;
            m_recipe = r;
            m_processing = true;
            loader.SetActive(true);
            m_time = utensil.cookTime;
            Reset();
        }
    }

    //checks if there is a plate
    public bool HasPlate()
    {
        return m_finished;
    }

    //takes the plate off of the utensil
    public Recipe PickUpPlate()
    {
        if (m_finished)
        {
            foodRenderer.sprite = PlayerData.GetFoodSprite(null);
            m_finished = false;
            return m_recipe;
        }
        return null;
    }

    public void Reset()
    {
        m_fill = 0f;
        m_foregroundImage.fillAmount = m_fill;
    }
}
