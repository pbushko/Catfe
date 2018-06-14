using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer {

    private Recipe order;
    private Sprite orderSprite;
    private Sprite bubbleSprite;
    private Sprite customerSprite;

    public Customer(Sprite customer)
    {
        order = PlayerScript.getRandomRecipe();
        orderSprite = PlayerScript.getFoodSprite(order);
        customerSprite = customer;
    }

    public Recipe getOrder()
    {
        return order;
    }

    public Sprite getOrderSprite()
    {
        return orderSprite;
    }

    public Sprite getCustomerSprite()
    {
        return customerSprite;
    }

    public bool rightRecipe(Recipe r)
    {
        return Recipe.compareRecipes(order, r);
    }

}
