using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer {

    private Recipe order;

    public Customer()
    {
        order = PlayerScript.getRandomRecipe();
    }

    public Recipe getOrder()
    {
        return order;
    }

    public bool rightRecipe(Recipe r)
    {
        return Recipe.compareRecipes(order, r);
    }

}
