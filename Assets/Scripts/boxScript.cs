using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxScript : MonoBehaviour {

    public Ingreds ingredient;

    // Use this for initialization
    void Start () {
       // Debug.Log("start ");
    }

    public void pushed()
    {

        PlayerScript.addToPlayerQueue(ingredient, CookingUten.none);

    }

    void Update()
    {

    }
}
