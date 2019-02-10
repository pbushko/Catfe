using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Recipe m_order;
    public int m_number;
    public float m_patience = 15f;
    public int m_heartCount = 3;
    public Sprite body;
    public Sprite face;

    public Recipe GetOrder()
    {
        return m_order;
    }

    public int GetCustomerNumber()
    {
        return m_number;
    }

    public void SetCustomerNumber(int n)
    {
        m_number = n;
    }

    public bool CorrectRecipe(Recipe r)
    {
        return Recipe.CompareRecipe(m_order, r);
    }

    public void OnClick()
    {
        PlayerScript.AddCustomerToPlayerQueue(m_number);
        //to get every time since the customers can move, but this still might need to be
        //fixed later
        Vector3 loc = gameObject.transform.position;
        PlayerScript.AddLocation(new Vector3(loc.x - 5, loc.y + 1, loc.z));
    }

    public int UpdatePatience() 
    {
        m_patience -= Time.deltaTime;
        if (m_patience <= 0)
        {
            m_patience = 15f;
            return --m_heartCount;
        }
        //don't want to return the heart count every time or it will keep setting the sprites; unnecessary getting the sprites otherwise
        else
        {
            return 99;
        }
    }

    public int GetPatience()
    {
        return (int) m_patience;
    }

}
