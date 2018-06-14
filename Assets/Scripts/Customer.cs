using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {

    public Recipe order;
    public int number;

    public Recipe getOrder()
    {
        return order;
    }

    public int getCustomerNumber()
    {
        return number;
    }

    public void setNumber(int n)
    {
        number = n;
    }

    public bool rightRecipe(Recipe r)
    {
        return Recipe.compareRecipes(order, r);
    }

    public void pushed()
    {
        PlayerScript.addCustomerToPlayerQueue(number);
        PlayerScript.givePlateToCustomer(order, number);
    }

}
