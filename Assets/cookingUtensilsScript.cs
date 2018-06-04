using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cookingUtensilsScript : MonoBehaviour {

    public CookingUten utensil;

    public void pushed()
    {
        PlayerScript.addToPlayerQueue(Ingreds.none, utensil);
    }

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
