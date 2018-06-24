using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

    PlayerScript player;
    CustomerGenerator customer;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Cat").GetComponent<PlayerScript>();
        customer = GameObject.Find("Customer Line").GetComponent<CustomerGenerator>();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    
}
