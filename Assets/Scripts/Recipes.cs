using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredients { Carrot, Lettuce, Potato, Beef, Chicken, Milk, none };
public enum CookingTools { Knife, Oven, Stove, Blender, none };
public enum Dishes { CarrotSalad, ChickenAndBeef, CarrotSoup, none };

//a recipe object; stores what is needed for each recipe
public class Recipe {

    private string recipeName;

    private Ingredients[] ingredients;
    //the utensils have an order; like needing to chop up food before putting it into the oven
    private CookingTools utensils;

    private Dishes dish;

    private int price;
	
    //the constructor to make a new recipe
    public Recipe(string name, Ingredients[] i, CookingTools c, int p)
    {
        recipeName = name;
        ingredients = i;
        utensils = c;
        price = p;
    }

    public string getRecipeName()
    {
        return recipeName;
    }

    public Ingredients[] getIngreds()
    {
        return ingredients;
    }

    public CookingTools getUten()
    {
        return utensils;
    }

    public int getPrice()
    {
        return price;
    }

    private bool sameIngredients(Ingredients[] ins)
    {
        int inLen = ingredients.Length;
        int j;
        //if there aren't the same amount of ingredients, it can't be the same recipe
        if (ins.Length != inLen)
        {
            return false;
        }
        foreach (Ingredients i in ins)
        {
            for (j = 0; j < inLen; j++)
            {
                if (i == ingredients[j])
                {
                    j = inLen + 1;
                }
            }
            //if the ingredient wasn't found, then j will only be the same as inLen
            if (j == inLen)
            {
                return false;
            }
        }
        //if everything was found, return true
        return true;
    }
        
    public bool sameRecipe(Ingredients[] ins, CookingTools c)
    {
        if (sameIngredients(ins))
        {
            return (c == utensils);
        }
        return false;
    }

    public static bool compareRecipes(Recipe r1, Recipe r2)
    {
        return r1.getRecipeName() == r2.getRecipeName();
    }

}
