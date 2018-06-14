using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {

    int maxCustomers = 4;
    static int curCustomers;
    float countdown;
    Customer temp;
    public static List<Customer> customers;
    public static List<Sprite> customerSprites;

    // Use this for initialization
    void Start () {
        customers = new List<Customer>();
        customerSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Patrons"));
        countdown = 1.0f;
        curCustomers = 0;
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
                countdown = 10.0f;
                curCustomers++;
                addCustomer();
            }
        }
	}

    private void addCustomer()
    {
        temp = new Customer(customerSprites[0]);
        customers.Add(temp);
        Debug.Log("a new customer!  They want: " + temp.getOrder().getRecipeName());
    }

    public static List<Customer> getCustomers()
    {
        return customers;
    }

    public static void removeCustomer(Customer c)
    {
        customers.Remove(c);
        curCustomers--;
    }

}
