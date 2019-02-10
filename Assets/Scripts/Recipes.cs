using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public enum Ingredients { Carrot, Lettuce, Potato, Beef, Chicken, Milk, none };
public enum CookingTools { Knife, Oven, Stove, Blender, none };
public enum Dishes { CarrotSalad, ChickenAndBeef, CarrotSoup, none };

//a recipe object; stores what is needed for each recipe
[System.Serializable]
public class Recipe 
{
    public int idNum;

    public string itemType;

    public string recipeName;

    public List<Ingredients> ingredients;

    //the utensils have an order; like needing to chop up food before putting it into the oven
    public CookingTools utensils;

    public Dishes dish;

    //the price the customer will pay for it
    public int price;
    //the cost to buy the recipe
    public int cost;

    public int starLevel;

    public RestaurantType foodType;

	public string sprite;

    public string description;

    //getting the recipe from the xml file
	public Recipe(XmlNode recipe, string type, int star)
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

        ingredients = new List<Ingredients>();
        foreach (XmlNode ingred in recipe.SelectNodes("ingredient"))
        {
            if (ingred.Attributes["name"].Value != "")
            {
                ingredients.Add(GetXmlIngredient(ingred.Attributes["name"].Value));
            }
        }
        //this is used to make the slop's ingredients null
        if (ingredients.Count == 0)
        {
            ingredients = null;
        }

        starLevel = star;
        
        utensils = GetXmlCookingTool(recipe.SelectSingleNode("cookingTool").Attributes["name"].Value);

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
                return CookingTools.none;
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
        return ingredients.ToArray();
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
        int inLen = ingredients.Count;
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
    
    //checks if the ingredients picked up by the player and the cooking tool used matches this recipe
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
        return r1.idNum == r2.idNum;
    }

}
