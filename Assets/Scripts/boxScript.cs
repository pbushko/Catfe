using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxScript : MonoBehaviour {

    public Ingredients ingredient;

    // Use this for initialization
    void Start () {

    }

    public void pushed()
    {
        PlayerScript.addIngredientToPlayerQueue(ingredient);
    }

    void Update()
    {

    }
}
