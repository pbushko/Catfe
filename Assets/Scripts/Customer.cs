using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Recipe m_order;
    public int m_number;
    public float m_patience = 15f;
    public int m_heartCount = 3;
    public SpriteRenderer body;
    public SpriteRenderer face;
    public SpriteRenderer order;
    public List<SpriteRenderer> ingreds;
    public SpriteRenderer cookingUten;

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
        //to get every time since the customers can move, but this still might need to be
        //fixed later
        Vector3 loc = gameObject.transform.position;
        PlayerScript.AddLocation(new Vector3(loc.x - 5, loc.y + 1, loc.z));
    }

    public int UpdatePatience() 
    {
        m_patience -= Time.deltaTime;
        if (m_patience <= 0)
        {
            m_patience = 15f;
            return --m_heartCount;
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

    public void SetOrderSprites(Recipe r)
    {
        m_order = r;
        if (r != null) {
            order.sprite = PlayerData.GetFoodSprite(r.recipeName);
        }
        else {
            order.sprite = PlayerData.GetFoodSprite("");
        }
        for (int i = 0; i < ingreds.Count; i++)
        {
            //if there are fewer ingredients than sprites to list
            if (i >= r.ingredients.Count) {
                ingreds[i].enabled = false;
            }
            else {
                ingreds[i].enabled = true;
                ingreds[i].sprite = RestaurantMain.restMain.GetIngredientSprite(r.ingredients[i]);
            }
        }
        cookingUten.sprite = RestaurantMain.restMain.GetCookingUtenSprite(r.utensils, 0);
    }

    public void SetBodySprites(Sprite bodySprite, Sprite faceSprite) 
    {
        body.sprite = bodySprite;
        face.sprite = faceSprite;
    }

}
