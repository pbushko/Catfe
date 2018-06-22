using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Recipe m_order;
    public int m_number;
    private float m_patience = 20f;
    private int heartCount = 4;

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
    }

    public int UpdatePatience() 
    {
        m_patience -= Time.deltaTime;
        if (m_patience/4 < heartCount)
        {
            return --heartCount;
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
