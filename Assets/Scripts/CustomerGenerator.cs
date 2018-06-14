﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerGenerator : MonoBehaviour {

    int maxCustomers = 4;
    static int curCustomers;
    float countdown;
    Customer temp;
    static float offset = 1.5f;
    static Vector3 LinePosition;
    public GameObject customerLine;
    public GameObject buttonPrefab;
    public static List<GameObject> customers;
    public static List<Sprite> customerSprites;

    // Use this for initialization
    void Start () {
        customers = new List<GameObject>();
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
            }
        }
	}

    private void addCustomer()
    {
        //trying to make the customer prefab button get into line
        GameObject c = (GameObject)Instantiate(buttonPrefab);
        c.transform.SetParent(customerLine.transform);

        //depending on what # customer this is, we want to offset the position
        c.transform.position = 
            new Vector3(LinePosition.x + (curCustomers * offset), LinePosition.y + (curCustomers * offset), LinePosition.z);
        
        c.GetComponent<Customer>().number = curCustomers;
        Recipe order = PlayerScript.getRandomRecipe();
        c.GetComponent<Customer>().order = order;
        c.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerScript.getFoodSprite(order);

        curCustomers++;

        customers.Add(c);
    }

    public static List<GameObject> getCustomers()
    {
        return customers;
    }

    public static void removeCustomer(int n)
    {
        curCustomers--;
        GameObject button = customers[n];
        customers.Remove(customers[n]);
        if (curCustomers > 0)
        {
            shiftPositions();
        }
        Destroy(button);   
    }

    private static void shiftPositions()
    {
        //just setting the 1st customer's position to the start of the line
        customers[0].transform.position = LinePosition;
        customers[0].GetComponent<Customer>().setNumber(0);
        //already checked the 1st position
        for (int i = 1; i < curCustomers; i++)
        {
            customers[i].GetComponent<Customer>().setNumber(i);
            //if the position is more than the offset, then it's not in the right spot
            if (customers[i].transform.position.x > customers[i-1].transform.position.x + offset)
            {
                //change all of the positions for the remaining buttons
                while (i < curCustomers)
                {
                    customers[i].GetComponent<Customer>().setNumber(i);
                    customers[i].transform.position = getNewPosition(customers[i]);
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
