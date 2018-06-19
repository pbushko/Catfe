using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Recipe m_order;
    public int m_number;

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

}
