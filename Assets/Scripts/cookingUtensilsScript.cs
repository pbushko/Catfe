using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cookingUtensilsScript : MonoBehaviour {

    public CookingTools utensil;

    public void pushed()
    {
        PlayerScript.addCookingToolToPlayerQueue(utensil);
    }

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
