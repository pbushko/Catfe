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

    // Use this for initialization
    void Start ()
    {
        m_customers = new List<GameObject>();
        m_customerSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Patrons"));
        m_countdown = 1.0f;
        m_linePosition = customerLine.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //make a new customer appear if we are not at the max # of customers
        if (m_customers.Count < Variables.MAX_CUSTOMERS)
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
        for (int i = 0; i < m_customers.Count; i++)
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
                m_customers[i].transform.GetChild(2).transform.GetChild(heartsLeft).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
	}

    private void AddCustomer()
    {
        //trying to make the customer prefab button get into line
        GameObject c = (GameObject)Instantiate(buttonPrefab);
        c.transform.SetParent(customerLine.transform);

        //depending on what # customer this is, we want to offset the position
        c.transform.position = 
            new Vector3(m_linePosition.x + (m_customers.Count * Variables.CUSTOMER_OFFSET), m_linePosition.y + (m_customers.Count * Variables.CUSTOMER_OFFSET), m_linePosition.z);
        
        c.GetComponent<Customer>().m_number = m_customers.Count;
        Recipe order = PlayerScript.GetRandomRecipe();
        c.GetComponent<Customer>().m_order = order;
        c.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerScript.GetFoodSprite(order);

        m_customers.Add(c);
    }

    public static Customer GetCustomer(int n)
    {
        return m_customers[n].GetComponent<Customer>();
    }

    public static void RemoveCustomer(int n)
    {
        GameObject button = m_customers[n];
        m_customers.Remove(m_customers[n]);
        if (m_customers.Count > 0)
        {
            ShiftPositions();
        }
        Destroy(button);   
    }

    private static void ShiftPositions()
    {
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
                while (i < m_customers.Count)
                {
                    m_customers[i].GetComponent<Customer>().SetCustomerNumber(i);
                    m_customers[i].transform.position = GetNewPosition(m_customers[i]);
                    i++;
                }
            }
        }
    }

    //helper function for shiftPosition()
    private static Vector3 GetNewPosition(GameObject c)
    {
        return new Vector3(c.transform.position.x - Variables.CUSTOMER_OFFSET, c.transform.position.y - Variables.CUSTOMER_OFFSET, c.transform.position.z);
    }

}
