﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour {

    public GameObject cat;

    private static int numIngreds = 3;
    private static int numUtens = 2;

    //stores what the player will be doing.  can contain both ingreds and cookinguten
    static Queue playerQueue = new Queue();
    //stores the LoadingBar for the utensils
    static Queue<GameObject> loaders = new Queue<GameObject>();
    //stores the location of the next thing in the queue so the cat can move to it
    static Queue<Vector3> locations = new Queue<Vector3>();
    static Vector3 nextLocation;

    //stores the items CURRENTLY in the player's hand; this doesn't count any items that are in the queue
    List<Ingredients> itemsInHand = new List<Ingredients>();

    static List<Sprite> foods;
    static List<string> foodNames = new List<string>();

    static Recipe plateInHand = null;

    //stores all the recipes
    static List<Recipe> recipes = new List<Recipe>();

    //the countdown for the time between tasks being done
    float countdown = 3.0f;

    Recipe slop = new Recipe("slop", null, CookingTools.none, 0);

    static SpriteRenderer plate;

    // Use this for initialization
    void Start () {
        foods = new List<Sprite>(Resources.LoadAll<Sprite>("Foods"));
        plate = GameObject.Find("plate").GetComponent<SpriteRenderer>();

        nextLocation = cat.transform.position;

        Ingredients[] i = new Ingredients[1];

        //for the first recipe
        i[0] = Ingredients.Carrot;

        Ingredients[] otherI = new Ingredients[2];

        otherI[0] = Ingredients.Beef;
        otherI[1] = Ingredients.Chicken;

        //loading all of the recipes; this will have to be done on a level-by level basis
        recipes.Add(new Recipe("Carrot Salad", i, CookingTools.Knife, 5));

        recipes.Add(new Recipe("Carrot Soup", i, CookingTools.Stove, 6));

        recipes.Add(new Recipe("Beef and Chicken", otherI, CookingTools.Stove, 10));

        //Adding the food names to allow us to search for the sprite
        foreach (Sprite s in foods)
        {
            foodNames.Add(s.name);
        }
        
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
                if(hit.collider.tag == "ingredients")
                {
                    hit.collider.GetComponent<boxScript>().pushed();
                    locations.Enqueue(hit.collider.transform.position);
                }
                else if(hit.collider.tag == "utensil")
                {
                    hit.collider.GetComponent<cookingUtensilsScript>().pushed();
                    locations.Enqueue(hit.collider.transform.position);
                }
                else if(hit.collider.tag == "customer")
                {
                    locations.Enqueue(hit.collider.transform.position);
                    hit.collider.GetComponent<Customer>().pushed();
                }
            }
        }

        countdown -= Time.deltaTime;

        //if enough time has passed, put the next item in the queue into the player's hand
        if (countdown <= 0.0f && playerQueue.Count > 0)
        {
            nextLocation = locations.Dequeue();
            Debug.Log(nextLocation);
            countdown = 1.0f;
            //putting the ingredient into the player's hand
            if (playerQueue.Peek().GetType() == typeof(Ingredients))
            {
                itemsInHand.Add((Ingredients)playerQueue.Dequeue());
            }
            //"using" the kitchen utensil.  must check if the recipe exists
            else if (playerQueue.Peek().GetType() == typeof(CookingTools))
            {
                CookingTools tool = (CookingTools)playerQueue.Dequeue();
                LoadingBar loader = loaders.Dequeue().GetComponent<LoadingBar>();

                //if there is a plate for us to pick up.
                if (loader.hasPlate())
                {
                    //can only pick up the plate if there is no other plate in our hand
                    if (plateInHand == null)
                    {
                        //we picked up a plate
                        changePlateInHand(loader.pickUpPlate());
                    }
                }
                //if no plate, we will place the recipe as long as we have items to cook and there is nothing on the stove
                else if (itemsInHand.Count > 0)
                {
                    Recipe r = getRecipe(itemsInHand.ToArray(), tool);

                    //either putting a recipe in or finding what we should get from clicking on the utensil
                    loader.loading(1f, getFoodSprite(r), r);

                    itemsInHand.Clear();
                }
 
            }
            //clicked on a customer
            else
            {
                playerQueue.Dequeue();
            }
        }

        cat.transform.position = Vector3.MoveTowards(cat.transform.position, nextLocation, 0.1f);
    }

    private static void changePlateInHand(Recipe r)
    {
        plateInHand = r;
        plate.sprite = getFoodSprite(r);
    }

    public static Sprite getFoodSprite(Recipe food)
    {
        if (food != null)
        {
            return foods[foodNames.IndexOf(food.getRecipeName())];
        }
        return foods[foodNames.IndexOf("None")];
    }

    //allows the buttons from the crates/cooking utens to be added into the player queue
    public static void addIngredientToPlayerQueue(Ingredients i)
    {
        playerQueue.Enqueue(i);
    }

    public static void addCookingToolToPlayerQueue(CookingTools c, GameObject l)
    {
        playerQueue.Enqueue(c);
        loaders.Enqueue(l);
    }

    public static void addCustomerToPlayerQueue(int i)
    {
        playerQueue.Enqueue(i);
    }

    Recipe getRecipe(Ingredients[] items, CookingTools uten)
    {
        foreach (Recipe r in recipes)
        {
            if (r.sameRecipe(items, uten))
            {
                return r;
            }
        }
        return slop;
    }

    public void throwPlateAway()
    {
        changePlateInHand(null);
        MoneyTracker.addMoney(-5);
    }

    public static void givePlateToCustomer(Recipe order, int n)
    {
        if (plateInHand == null)
        {
            return;
        }

        if (Recipe.compareRecipes(plateInHand, order))
        {
            MoneyTracker.addMoney(plateInHand.getPrice());

            changePlateInHand(null);

            CustomerGenerator.removeCustomer(n);
        }
    }

    public static Recipe getRandomRecipe()
    {
        int rand = Random.Range(0, recipes.Count);

        Recipe[] r = recipes.ToArray();

        return r[rand];
    }

}
