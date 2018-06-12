using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

    private bool isEnabled;
    PlayerScript player;
    CustomerGenerator customer;

	// Use this for initialization
	void Start () {
        isEnabled = true;
        player = GameObject.Find("chefcat").GetComponent<PlayerScript>();
        customer = GameObject.Find("chefcat").GetComponent<CustomerGenerator>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void pause()
    {
        isEnabled = !isEnabled;
        player.enabled = isEnabled;
        customer.enabled = isEnabled;
    }
}
