﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //stores what the player will be doing.  can contain both ingreds and cookinguten
    private static Queue m_playerQueue = new Queue();

    //stores the location of the next thing in the queue so the cat can move to it
    private static Queue<Vector3> m_locations = new Queue<Vector3>();
    private static Vector3 m_nextLocation;
    private static bool m_needsToMove;

    //stores the items CURRENTLY in the player's hand; this doesn't count any items that are in the queue
    private List<Ingredients> m_itemsInHand = new List<Ingredients>();

    private static Recipe m_plateInHand = null;

    //stores all the recipes
    private static List<Recipe> m_recipes = new List<Recipe>();

    private static SpriteRenderer m_plate;

    // Use this for initialization
    void Start ()
    {
        m_plate = GameObject.Find("plate").GetComponent<SpriteRenderer>();

        m_nextLocation = transform.position;
        m_needsToMove = false;

        m_recipes = PlayFabLogin.GetMinigameRecipes();
    }

    //to reset the minigame after it's been done more than once
    public void Reset() 
    {
        m_playerQueue = new Queue();
        m_locations = new Queue<Vector3>();
        m_needsToMove = false;
        m_itemsInHand = new List<Ingredients>();
        ChangePlateInHand(null);
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.up);
            if (hit.collider)
            {
                Vector3 loc = hit.collider.transform.position;
                if (hit.collider.tag == "ingredients")
                {
                    hit.collider.GetComponent<IngredientBoxScript>().OnClick();
                    m_locations.Enqueue(new Vector3(loc.x, loc.y + 2, loc.z));
                }
                else if(hit.collider.tag == "customer")
                {
                    hit.collider.GetComponent<Customer>().OnClick();
                }
            }
        }
        //setting the player to move if there is only one location to go to
        if (!m_needsToMove && m_locations.Count >= 1)
        {
            m_nextLocation = m_locations.Dequeue();
            m_needsToMove = true;
        }

        //if enough time has passed, put the next item in the queue into the player's hand
        if (!m_needsToMove && m_playerQueue.Count > 0)
        {
            if (m_locations.Count > 0)
            {
                m_nextLocation = m_locations.Dequeue();
                m_needsToMove = true;
            }
            //putting the ingredient into the player's hand
            if (m_playerQueue.Peek().GetType() == typeof(Ingredients))
            {
                m_itemsInHand.Add((Ingredients)m_playerQueue.Dequeue());
            }
            //"using" the kitchen utensil.  must check if the recipe exists
            else if (m_playerQueue.Peek().GetType() == typeof(CookingUtensilScript))
            {
                CookingUtensilScript tool = (CookingUtensilScript)m_playerQueue.Dequeue();

                //if there is a plate for us to pick up.
                if (tool.HasPlate())
                {
                    //can only pick up the plate if there is no other plate in our hand
                    if (m_plateInHand == null)
                    {
                        //we picked up a plate
                        ChangePlateInHand(tool.PickUpPlate());
                    }
                }
                //if no plate, we will place the recipe as long as we have items to cook and there is nothing on the stove
                else if (m_itemsInHand.Count > 0)
                {
                    Recipe r = GetRecipe(m_itemsInHand.ToArray(), tool.utensil.utensil);

                    //either putting a recipe in or finding what we should get from clicking on the utensil
                    tool.Loading(PlayerData.GetFoodSprite(r.recipeName), r);

                    m_itemsInHand.Clear();
                }
 
            }
            //clicked on a customer
            else
            {
                Customer c = CustomerGenerator.GetCustomer((int)m_playerQueue.Dequeue());
                GivePlateToCustomer(c.GetOrder(), c.GetCustomerNumber());
            }
        }

        if (m_needsToMove)
        {
            Vector3 catPos = transform.position;
            if (catPos == m_nextLocation)
            {
                m_needsToMove = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(catPos, m_nextLocation, 0.2f);
            }
        }
    }

    private static void ChangePlateInHand(Recipe r)
    {
        m_plateInHand = r;
        if (r != null) {
            m_plate.sprite = PlayerData.GetFoodSprite(r.recipeName);
        }
        else {
            m_plate.sprite = PlayerData.GetFoodSprite("");
        }
    }

    //allows the buttons from the crates/cooking utens to be added into the player queue
    public static void AddIngredientToPlayerQueue(Ingredients i)
    {
        m_playerQueue.Enqueue(i);
    }

    public static void AddCookingToolToPlayerQueue(CookingUtensilScript c)
    {
        m_playerQueue.Enqueue(c);
    }

    public static void AddCustomerToPlayerQueue(int i)
    {
        m_playerQueue.Enqueue(i);
    }

    Recipe GetRecipe(Ingredients[] items, CookingTools uten)
    {
        foreach (Recipe r in m_recipes)
        {
            if (r.SameRecipe(items, uten))
            {
                return r;
            }
        }
        return PlayerData.playerData.slop;
    }

    public void ThrowPlateAway()
    {
        ChangePlateInHand(null);
        RestaurantMain.AddMoney(-5);
    }

    public static void GivePlateToCustomer(Recipe order, int n)
    {
        if (m_plateInHand != null && Recipe.CompareRecipe(m_plateInHand, order))
        {
            RestaurantMain.AddMoney(m_plateInHand.GetPrice());
            ChangePlateInHand(null);

            CustomerGenerator.RemoveCustomer(n);
        }
    }

    public static Recipe GetRandomRecipe()
    {
        return m_recipes[Random.Range(0, m_recipes.Count)];
    }

    public static void AddLocation(Vector3 v) {
        m_locations.Enqueue(v);
    }

}
