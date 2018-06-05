using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingreds { Carrot, Lettuce, Potato, Beef, Chicken, Milk, none };
public enum CookingUten { Knife, Oven, Stove, Blender, none };
public enum Dishes { };

//a recipe object; stores what is needed for each recipe
public class Recipe {

    private string recipeName;

    private Ingreds[] ingredients;
    //the utensils have an order; like needing to chop up food before putting it into the oven
    private CookingUten utensils;

    private Dishes dish;
	
    //the constructor to make a new recipe
    public Recipe(string name, Ingreds[] i, CookingUten c)
    {
        recipeName = name;
        ingredients = i;
        utensils = c;
    }

    public string getRecipeName()
    {
        return recipeName;
    }

    public Ingreds[] getIngreds()
    {
        return ingredients;
    }

    public CookingUten getUten()
    {
        return utensils;
    }

    private bool sameIngredients(Ingreds[] ins)
    {
        int inLen = ingredients.Length;
        int j;
        //if there aren't the same amount of ingredients, it can't be the same recipe
        if (ins.Length != inLen)
        {
            return false;
        }
        foreach (Ingreds i in ins)
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
        
    public bool sameRecipe(Ingreds[] ins, CookingUten c)
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
