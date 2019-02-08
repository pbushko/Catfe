using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerGenerator : MonoBehaviour {

    public GameObject customerLine;

    private float m_countdown;
    private Customer m_temp;

    private static List<GameObject> m_customers;
    private List<Sprite> m_customerSprites;
    private static int m_customerCount;

    // Use this for initialization
    void Start ()
    {
        m_customers = new List<GameObject>();
        m_customerSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Patrons"));
        m_countdown = 1.0f;

        //loading in all the customers from the line
        for (int i = 0; i < customerLine.transform.childCount; i++)
        {
            m_customers.Add(customerLine.transform.GetChild(i).gameObject);
        }
        m_customerCount = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //make a new customer appear if we are not at the max # of customers
        if (m_customerCount < Variables.MAX_CUSTOMERS)
        {
            m_countdown -= Time.deltaTime;

            //if enough time has passed, get the next customer
            if (m_countdown <= 0.0f)
            {
                m_countdown = 5.0f;
                AddCustomer();
            }
        }

        //update the patience of all the customers and make ones leave if they run out
        for (int i = 0; i < m_customerCount; i++)
        {
            Customer c = m_customers[i].GetComponent<Customer>();
            int heartsLeft = c.UpdatePatience();
            //if out of patience, they leave
            if (heartsLeft < 0)
            {
                RemoveCustomer(i);
            }
            else if (heartsLeft <= 3)
            {
                m_customers[i].transform.GetChild(2).transform.GetChild(heartsLeft).GetComponent<SpriteRenderer>().enabled = false;
            }
        }
	}

    //resetting the customer line for when the minigame is accessed
    public void Reset()
    {
        m_customerCount = 0;
        if (m_customers != null)
        {
            foreach(GameObject c in m_customers)
            {
                c.SetActive(false);
            }
        }
    }

    private void AddCustomer()
    {
        //getting the customer that will be next in line
        GameObject newCustomer = m_customers[m_customerCount];
        Customer c = newCustomer.GetComponent<Customer>();

        //setting the info for the customer
        c.m_number = m_customerCount;
        c.m_order = PlayerScript.GetRandomRecipe();

        //setting the customer's body sprites
        int rand = Random.Range(0, 2);
        Sprite body = m_customerSprites[rand];
        Sprite face = m_customerSprites[Random.Range(2, 4)];
        c.body = body;
        c.face = face;
        
        //changing the patience
        if (rand == 1)
        {
            c.m_heartCount += rand;
        }
        else
        {
            c.transform.GetChild(2).transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
        }

        SetCustomerSprites(newCustomer, c);

        m_customerCount++;
        newCustomer.SetActive(true);

    }

    public static Customer GetCustomer(int n)
    {
        return m_customers[n].GetComponent<Customer>();
    }

    public static void RemoveCustomer(int n)
    {
        m_customerCount--;
        if (m_customerCount > 0 && n != m_customerCount)
        {
            ShiftPositions(n);
        }
        m_customers[m_customerCount].SetActive(false);   
    }

    private static void ShiftPositions(int n)
    {
        //going up the row from the one removed to change the data
        for (int i = n; i < m_customerCount; i++)
        {
            Customer cur = m_customers[i].GetComponent<Customer>();
            Customer next = m_customers[i+1].GetComponent<Customer>();

            cur.m_order = next.m_order;
            cur.m_patience = next.m_patience;
            cur.m_heartCount = next.m_heartCount;
            cur.body = next.body;
            cur.face = next.face;

            SetCustomerSprites(m_customers[i], next);
        }
    }

    private static void SetCustomerSprites(GameObject g, Customer c)
    {
        //setting the order
        g.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerData.GetFoodSprite(c.m_order);
        //setting the body
        g.GetComponent<SpriteRenderer>().sprite = c.body;
        //setting the face
        g.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = c.face;
        //setting the patience/hearts
        for (int i = 0; i < 4; i++)
        {
            //if this heart should be there, set it to show
            if (i < c.m_heartCount)
            {
                g.transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                g.transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }


}
