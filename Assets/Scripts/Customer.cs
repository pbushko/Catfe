using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer {

    private Recipe order;
    private int number;
    private GameObject customerPrefab;

    public Customer(GameObject customer, int customerNumber)
    {
        order = PlayerScript.getRandomRecipe();
        customerPrefab = customer;
        number = customerNumber;
    }

    public Recipe getOrder()
    {
        return order;
    }

    public GameObject getCustomerPrefab()
    {
        return customerPrefab;
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
        PlayerScript.givePlateToCustomer(order, customerPrefab, number);
    }

}
