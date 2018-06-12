using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour {

    private static int numIngreds = 3;
    private static int numUtens = 2;

    //stores what the player will be doing.  can contain both ingreds and cookinguten
    static Queue playerQueue = new Queue();

    //stores the items CURRENTLY in the player's hand; this doesn't count any items that are in the queue
    List<Ingredients> itemsInHand = new List<Ingredients>();

    List<Sprite> foods;
    List<string> foodNames = new List<string>();

    static Recipe plateInHand = null;

    //stores all the recipes
    static List<Recipe> recipes = new List<Recipe>();

    //the countdown for the time between tasks being done
    float countdown = 3.0f;

    Recipe slop = new Recipe("slop", null, CookingTools.none, 0);

    SpriteRenderer plate;

    // Use this for initialization
    void Start () {
        foods = new List<Sprite>(Resources.LoadAll<Sprite>("Foods"));
        plate = GameObject.Find("plate").GetComponent<SpriteRenderer>();

        Debug.Log(plate.name);
        Debug.Log(foods[0].name);

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
        countdown -= Time.deltaTime;

        //if enough time has passed, put the next item in the queue into the player's hand
        if (countdown <= 0.0f && playerQueue.Count > 0)
        {
            countdown = 1.0f;
            //putting the ingredient into the player's hand
            if (playerQueue.Peek().GetType() == typeof(Ingredients))
            {
                itemsInHand.Add((Ingredients)playerQueue.Dequeue());
            }
            //"using" the kitchen utensil.  must check if the recipe exists
            else
            {
                //cannot make anything if there is nothing in your hand.
                if (itemsInHand.Count > 0)
                {
                    Recipe r = getRecipe(itemsInHand.ToArray(), (CookingTools)playerQueue.Dequeue());

                    changePlateInHand(r);

                    //clearing the ingredients
                    itemsInHand.Clear();
                }
            }
        }
	}

    private void changePlateInHand(Recipe r)
    {
        int index = 0;
        if (r != null)
        {
            plateInHand = r;
            //to change the sprite of the plate the chefcat holds
            index = foodNames.IndexOf(r.getRecipeName());
            plate.sprite = foods[index];
        }
        else
        {
            plateInHand = slop;
            index = foodNames.IndexOf("None");
            plate.sprite = foods[index];
        }
    }

    //allows the buttons from the crates/cooking utens to be added into the player queue
    public static void addIngredientToPlayerQueue(Ingredients i)
    {
        playerQueue.Enqueue(i);
    }

    public static void addCookingToolToPlayerQueue(CookingTools c)
    {
        playerQueue.Enqueue(c);
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

    public void givePlateToCustomer()
    {
        if (plateInHand == null)
        {
            Debug.Log("nothing to give the customer!");
            return;
        }
        Customer[] cs = CustomerGenerator.getCustomers().ToArray();

        for (int i = 0; i < cs.Length; i++)
        {
            if (cs[i].rightRecipe(plateInHand))
            {
                Debug.Log("Yay! The customer got his meal!");
                MoneyTracker.addMoney(plateInHand.getPrice());
                changePlateInHand(null);
                CustomerGenerator.removeCustomer(cs[i]);
                return;
            }
        }
        //if this is reached, the plate doesn't match any customers
        Debug.Log("doesn't match what any of the customers want!");        
    }

    public static Recipe getRandomRecipe()
    {
        int rand = Random.Range(0, recipes.Count);

        Recipe[] r = recipes.ToArray();

        return r[rand];
    }
}
