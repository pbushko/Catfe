using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour {

    private static int numIngreds = 3;
    private static int numUtens = 2;

    //stores what the player will be doing.  can contain both ingreds and cookinguten
    static Queue playerQueue = new Queue();

    //stores the items CURRENTLY in the player's hand; this doesn't count any items that are in the queue
    List<Ingreds> itemsInHand = new List<Ingreds>();

    //stores all the recipes
    List<Recipe> recipes = new List<Recipe>();

    //the countdown for the time between tasks being done
    float countdown = 3.0f;

    // Use this for initialization
    void Start () {
        Ingreds[] i = new Ingreds[1];

        //for the first recipe
        i[0] = Ingreds.Carrot;

        Ingreds[] otherI = new Ingreds[2];

        otherI[0] = Ingreds.Beef;
        otherI[1] = Ingreds.Chicken;

        //loading all of the recipes; this will have to be done on a level-by level basis
        recipes.Add(new Recipe("Chopped Carrots", i, CookingUten.Knife));

        recipes.Add(new Recipe("Carrot Soup", i, CookingUten.Stove));

        recipes.Add(new Recipe("Chicken + Beef", otherI, CookingUten.Stove));
    }
	
	// Update is called once per frame
	void Update () {
        countdown -= Time.deltaTime;

        //if enough time has passed, put the next item in the queue into the player's hand
        if (countdown <= 0.0f)
        {
            countdown = 3.0f;
            //putting the ingredient into the player's hand
            if (playerQueue.Peek().GetType() == typeof(Ingreds))
            {
                itemsInHand.Add((Ingreds) playerQueue.Dequeue());
                Debug.Log("got ingredient");
            }
            //"using" the kitchen utensil.  must check if the recipe exists
            else
            {
                if (isRecipe(itemsInHand.ToArray(), (CookingUten)playerQueue.Dequeue())) {
                    
                }
                
                //clearing the ingredients
                itemsInHand.Clear();
            }
        }
	}

    //allows the buttons from the crates/cooking utens to be added into the player queue
    public static void addToPlayerQueue(Ingreds i, CookingUten c)
    {
        if (i == Ingreds.none) {
            playerQueue.Enqueue(c);
        }
        else
        {
            playerQueue.Enqueue(i);
        }
    }

    bool isRecipe(Ingreds[] items, CookingUten uten)
    {
        foreach (Recipe r in recipes)
        {
            //Debug.Log("Checking... " + r.getRecipeName());
            if (r.sameRecipe(items, uten))
            {
                Debug.Log("Found the right recipe!  It is: " + r.getRecipeName());
                return true;
            }
        }

        Debug.Log("Doesn't match any recipe.");

        return false;
    }
}
