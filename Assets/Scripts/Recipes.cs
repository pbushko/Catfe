using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public enum Ingredients { Carrot, Lettuce, Potato, Beef, Chicken, Milk, none };
public enum CookingTools { Knife, Oven, Stove, Blender, none };
public enum Dishes { CarrotSalad, ChickenAndBeef, CarrotSoup, none };

//a recipe object; stores what is needed for each recipe
public class Recipe {

    private int idNum;

    private string itemType;

    private string recipeName;

    private Ingredients[] ingredients;

    private List<Ingredients> ingreds;

    //the utensils have an order; like needing to chop up food before putting it into the oven
    private CookingTools utensils;

    private Dishes dish;

    private int price;

    private RestaurantType foodType;

    //the constructor to make a new recipe
    public Recipe(string name, Ingredients[] i, CookingTools c, int p)
    {
        recipeName = name;
        ingredients = i;
        utensils = c;
        price = p;
    }

    public Recipe(XmlNode recipe, string type)
    {
        if (recipe.Attributes["id"].Value != "")
        {
            idNum = int.Parse(recipe.Attributes["id"].Value);
        }
        recipeName = recipe.Attributes["name"].Value;
        if (recipe.Attributes["price"].Value != "")
        {
            price = int.Parse(recipe.Attributes["price"].Value);
        }

        ingreds = new List<Ingredients>();
        foreach (XmlNode ingred in recipe.SelectNodes("ingredient"))
        {
            if (ingred.Attributes["name"].Value != "")
            {
                ingreds.Add(GetXmlIngredient(ingred.Attributes["name"].Value));
            }
        }
        
        utensils = GetXmlCookingTool(recipe.SelectSingleNode("cookingTool").Attributes["name"].Value);

        ingredients = ingreds.ToArray();

        itemType = type;
    }

    private Ingredients GetXmlIngredient(string s)
    {
        switch (s)
        {
            case "Carrot":
                return Ingredients.Carrot;
                break;
                
            case "Lettuce":
                return Ingredients.Lettuce;
                break;

            case "Chicken":
                return Ingredients.Chicken;
                break;

            case "Beef":
                return Ingredients.Beef;
                break;

            default:
                return Ingredients.Carrot;
                break;
        }
    }

    private CookingTools GetXmlCookingTool(string s)
    {
        switch (s)
        {
            case "Knife":
                return CookingTools.Knife;
                break;
            case "Oven":
                return CookingTools.Oven;
                break;
            case "Stove":
                return CookingTools.Stove;
                break;
            default:
                return CookingTools.Knife;
                break;
        }
    }

    public string ToString()
    {
        string s = "id: " + idNum + " name: " + recipeName + " price: " + price + " type: " + itemType;
        s += "\ningredients: ";
        foreach (Ingredients i in ingredients)
        {
            s += "\n" + i;
        }
        s += "\nCooking tool: " + utensils;
        return s;
    }

    public string GetRecipeName()
    {
        return recipeName;
    }

    public Ingredients[] GetIngredients()
    {
        return ingredients;
    }

    public CookingTools GetUtensils()
    {
        return utensils;
    }

    public int GetPrice()
    {
        return price;
    }

    private bool SameIngredients(Ingredients[] ins)
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
        
    public bool SameRecipe(Ingredients[] ins, CookingTools c)
    {
        if (c == utensils)
        {
            return (SameIngredients(ins));
        }
        return false;
    }

    public static bool CompareRecipe(Recipe r1, Recipe r2)
    {
        return r1.GetRecipeName() == r2.GetRecipeName();
    }

}
