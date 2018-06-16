using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {

    public Ingredients ingredient;

    // Use this for initialization
    void Start ()
    {

    }

    public void OnClick()
    {
        PlayerScript.AddIngredientToPlayerQueue(ingredient);
    }

    void Update()
    {

    }
}
