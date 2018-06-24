using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingUtensilsScript : MonoBehaviour {

    public CookingTools utensil;

    public GameObject loader;

    public int upgradeCost;

    public void OnClick()
    {
        PlayerScript.AddCookingToolToPlayerQueue(utensil, loader);
    }

	// Use this for initialization
	void Start ()
    {
        upgradeCost = 10;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
