using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerGenerator : MonoBehaviour {

    int maxCustomers = 4;
    static int curCustomers;
    float countdown;
    Customer temp;
    static int offset = 60;
    static Vector3 LinePosition;
    public GameObject customerLine;
    public GameObject buttonPrefab;
    public static List<Customer> customers;
    public static List<Sprite> customerSprites;
    public static List<GameObject> customerButtons;

    // Use this for initialization
    void Start () {
        customers = new List<Customer>();
        customerButtons = new List<GameObject>();
        customerSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Patrons"));
        countdown = 1.0f;
        curCustomers = 0;
        LinePosition = customerLine.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //make a new customer appear if we are not at the max # of customers
        if (curCustomers < maxCustomers)
        {
            countdown -= Time.deltaTime;

            //if enough time has passed, put the next item in the queue into the player's hand
            if (countdown <= 0.0f)
            {
                countdown = 2.0f;
                addCustomer();
                curCustomers++;
            }
        }
	}

    private void addCustomer()
    {
        temp = new Customer(customerSprites[0]);
        customers.Add(temp);

        //trying to make the customer prefab button get into line
        GameObject c = (GameObject)Instantiate(buttonPrefab);
        c.transform.SetParent(customerLine.transform);
        
        c.GetComponent<Button>().onClick.AddListener(clicked);
        c.transform.GetChild(1).GetComponent<Image>().sprite = PlayerScript.getFoodSprite(temp.getOrder());

        //depending on what # customer this is, we want to offset the position
        c.transform.position = 
            new Vector3(LinePosition.x + (curCustomers * offset), LinePosition.y + (curCustomers * offset), LinePosition.z);

        customerButtons.Add(c);

        Debug.Log("a new customer!  They want: " + temp.getOrder().getRecipeName());
    }

    void clicked()
    {
        PlayerScript.givePlateToCustomer();
    }

    public static List<Customer> getCustomers()
    {
        return customers;
    }

    public static void removeCustomer(int c)
    {
        customers.Remove(customers[c]);
        curCustomers--;
        GameObject button = customerButtons[c];
        customerButtons.Remove(customerButtons[c]);
        shiftPositions();
        Destroy(button);   
    }

    private static void shiftPositions()
    {
        //just setting the 1st customer's position to the start of the line
        customerButtons[0].transform.position = LinePosition;
        //already checked the 1st position
        for (int i = 1; i < curCustomers; i++)
        {
            //if the position is more than the offset, then it's not in the right spot
            if (customerButtons[i].transform.position.x > customerButtons[i-1].transform.position.x - offset)
            {
                //change all of the positions for the remaining buttons
                while (i < curCustomers)
                {
                    customerButtons[i].transform.position = getNewPosition(customerButtons[i]);
                    i++;
                }
            }
        }
    }

    //helper function for shiftPosition()
    private static Vector3 getNewPosition(GameObject c)
    {
        return new Vector3(c.transform.position.x - offset, c.transform.position.y - offset, c.transform.position.z);
    }

}
