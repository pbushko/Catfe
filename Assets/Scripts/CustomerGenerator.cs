using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerGenerator : MonoBehaviour {

    public GameObject customerLine;
    public GameObject buttonPrefab;

    private float m_countdown;
    private Customer m_temp;

    private static Vector3 m_linePosition;
    private static List<GameObject> m_customers;
    private List<Sprite> m_customerSprites;
    private static int m_customerCount;

    // Use this for initialization
    void Start ()
    {
        m_customers = new List<GameObject>();
        m_customerSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Patrons"));
        m_countdown = 1.0f;
        m_linePosition = customerLine.transform.position;
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
                m_countdown = 2.0f;
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

    private void AddCustomer()
    {
        /*
        //trying to make the customer prefab button get into line
        GameObject c = (GameObject)Instantiate(buttonPrefab);
        c.transform.SetParent(customerLine.transform);

        //depending on what # customer this is, we want to offset the position
        c.transform.position = 
            new Vector3(m_linePosition.x + (m_customers.Count * Variables.CUSTOMER_OFFSET), m_linePosition.y + (m_customers.Count * Variables.CUSTOMER_OFFSET), m_linePosition.z);
        
        Customer temp = c.GetComponent<Customer>();
        temp.m_number = m_customers.Count;
        Recipe order = PlayerScript.GetRandomRecipe();
        temp.m_order = order;
        c.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerScript.GetFoodSprite(order);

        //changing the style of the customer to be random
        int rand = Random.Range(0, 2);
        c.GetComponent<SpriteRenderer>().sprite = m_customerSprites[rand];
        c.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = m_customerSprites[Random.Range(2, 4)];
        //changing the patience
        if (rand == 1)
        {
            temp.m_heartCount += rand;
        }
        else
        {
            temp.transform.GetChild(2).transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = null;
        }

        m_customers.Add(c);
        */

        //getting the customer that will be next in line
        GameObject newCustomer = m_customers[m_customerCount];
        Customer c = newCustomer.GetComponent<Customer>();

        //setting the info for the customer
        c.m_number = m_customerCount;
        c.m_order = PlayerScript.GetRandomRecipe();
        newCustomer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerScript.GetFoodSprite(c.m_order);

        //setting the customer's body sprites
        int rand = Random.Range(0, 2);
        Sprite body = m_customerSprites[rand];
        Sprite face = m_customerSprites[Random.Range(2, 4)];
        c.body = body;
        c.face = face;
        newCustomer.GetComponent<SpriteRenderer>().sprite = body;
        newCustomer.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = face;

        //changing the patience
        if (rand == 1)
        {
            c.m_heartCount += rand;
        }
        else
        {
            c.transform.GetChild(2).transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
        }

        m_customerCount++;
        newCustomer.SetActive(true);

    }

    public static Customer GetCustomer(int n)
    {
        return m_customers[n].GetComponent<Customer>();
    }

    public static void RemoveCustomer(int n)
    {
        if (m_customerCount - 1 > 0)
        {
            ShiftPositions(n);
        }
        m_customerCount--;
    }

    private static void ShiftPositions(int n)
    {
        /*
        //just setting the 1st customer's position to the start of the line
        m_customers[0].transform.position = m_linePosition;
        m_customers[0].GetComponent<Customer>().SetCustomerNumber(0);
        //already checked the 1st position
        for (int i = 1; i < m_customers.Count; i++)
        {
            m_customers[i].GetComponent<Customer>().SetCustomerNumber(i);
            //if the position is more than the offset, then it's not in the right spot
            if (m_customers[i].transform.position.x > m_customers[i-1].transform.position.x + Variables.CUSTOMER_OFFSET)
            {
                //change all of the positions for the remaining buttons
                while (i < m_customerCount)
                {
                    m_customers[i].GetComponent<Customer>().SetCustomerNumber(i);
                    m_customers[i].transform.position = GetNewPosition(m_customers[i]);
                    i++;
                }
            }
        }*/
        //going up the row from the one removed to change the data
        for (int i = n; n < m_customerCount; i++)
        {
            Customer cur = m_customers[n].GetComponent<Customer>();
            Customer next = m_customers[n+1].GetComponent<Customer>();
            cur = next;
            SetCustomerSprites(m_customers[n], next);
        }
    }

    private static void SetCustomerSprites(GameObject g, Customer c)
    {
        //setting the order
        g.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerScript.GetFoodSprite(c.m_order);
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

    //helper function for shiftPosition()
    private static Vector3 GetNewPosition(GameObject c)
    {
        return new Vector3(c.transform.position.x - Variables.CUSTOMER_OFFSET, c.transform.position.y - Variables.CUSTOMER_OFFSET, c.transform.position.z);
    }

}
