using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    //stores what the player will be doing.  can contain both ingreds and cookinguten
    private static Queue m_playerQueue = new Queue();
    //stores the LoadingBar for the utensils
    private static Queue<GameObject> m_loaders = new Queue<GameObject>();
    private static Queue<float> m_cookTimes = new Queue<float>();
    //stores the location of the next thing in the queue so the cat can move to it
    private static Queue<Vector3> m_locations = new Queue<Vector3>();
    private static Vector3 m_nextLocation;
    private static bool m_needsToMove;

    //stores the items CURRENTLY in the player's hand; this doesn't count any items that are in the queue
    private List<Ingredients> m_itemsInHand = new List<Ingredients>();

    private static List<Sprite> m_foods;
    private static List<string> m_foodNames = new List<string>();

    private static Recipe m_plateInHand = null;

    //stores all the recipes
    private static List<Recipe> m_recipes = new List<Recipe>();

    private Recipe m_slop = new Recipe("Slop", null, CookingTools.none, 0);

    private static SpriteRenderer m_plate;

    // Use this for initialization
    void Start ()
    {
        m_foods = new List<Sprite>(Resources.LoadAll<Sprite>("Foods"));
        m_plate = GameObject.Find("plate").GetComponent<SpriteRenderer>();

        m_nextLocation = transform.position;
        m_needsToMove = false;

        Ingredients[] i = new Ingredients[1];

        //for the first recipe
        i[0] = Ingredients.Carrot;

        Ingredients[] otherI = new Ingredients[2];

        otherI[0] = Ingredients.Beef;
        otherI[1] = Ingredients.Chicken;

        //loading all of the recipes; this will have to be done on a level-by level basis
        m_recipes.Add(new Recipe("Carrot Salad", i, CookingTools.Knife, 5));
        m_recipes.Add(new Recipe("Carrot Soup", i, CookingTools.Stove, 6));
        m_recipes.Add(new Recipe("Beef and Chicken", otherI, CookingTools.Stove, 10));

        //Adding the food names to allow us to search for the sprite
        foreach (Sprite s in m_foods)
        {
            m_foodNames.Add(s.name);
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
                Vector3 loc = hit.collider.transform.position;
                if (hit.collider.tag == "ingredients")
                {
                    hit.collider.GetComponent<BoxScript>().OnClick();
                    m_locations.Enqueue(new Vector3(loc.x, loc.y + 2, loc.z));
                }
                else if(hit.collider.tag == "utensil")
                {
                    hit.collider.GetComponent<CookingUtensilsScript>().OnClick();
                    m_locations.Enqueue(new Vector3(loc.x - 0.5f, loc.y - 1, loc.z));
                }
                else if(hit.collider.tag == "customer")
                {
                    hit.collider.GetComponent<Customer>().OnClick();
                    m_locations.Enqueue(new Vector3(loc.x - 5, loc.y + 1, loc.z));
                }
            }
            //setting the player to move if there is only one location to go to
            if (!m_needsToMove && m_locations.Count == 1)
            {
                m_nextLocation = m_locations.Dequeue();
                m_needsToMove = true;
            }
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
            else if (m_playerQueue.Peek().GetType() == typeof(CookingTools))
            {
                CookingTools tool = (CookingTools)m_playerQueue.Dequeue();
                LoadingBar loader = m_loaders.Dequeue().GetComponent<LoadingBar>();
                float time = m_cookTimes.Dequeue();

                //if there is a plate for us to pick up.
                if (loader.HasPlate())
                {
                    //can only pick up the plate if there is no other plate in our hand
                    if (m_plateInHand == null)
                    {
                        //we picked up a plate
                        ChangePlateInHand(loader.PickUpPlate());
                    }
                }
                //if no plate, we will place the recipe as long as we have items to cook and there is nothing on the stove
                else if (m_itemsInHand.Count > 0)
                {
                    Recipe r = GetRecipe(m_itemsInHand.ToArray(), tool);

                    //either putting a recipe in or finding what we should get from clicking on the utensil
                    loader.Loading(time, GetFoodSprite(r), r);

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
        m_plate.sprite = GetFoodSprite(r);
    }

    public static Sprite GetFoodSprite(Recipe food)
    {
        if (food != null)
        {
            return m_foods[m_foodNames.IndexOf(food.GetRecipeName())];
        }
        return m_foods[m_foodNames.IndexOf("None")];
    }

    //allows the buttons from the crates/cooking utens to be added into the player queue
    public static void AddIngredientToPlayerQueue(Ingredients i)
    {
        m_playerQueue.Enqueue(i);
    }

    public static void AddCookingToolToPlayerQueue(CookingTools c, GameObject l, float n)
    {
        m_playerQueue.Enqueue(c);
        m_loaders.Enqueue(l);
        m_cookTimes.Enqueue(n);
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
        return m_slop;
    }

    public void ThrowPlateAway()
    {
        ChangePlateInHand(null);
        RestaurantMain.AddMoney(-5);
    }

    public static void GivePlateToCustomer(Recipe order, int n)
    {
        if (m_plateInHand == null)
        {
            return;
        }

        if (Recipe.CompareRecipe(m_plateInHand, order))
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

}
