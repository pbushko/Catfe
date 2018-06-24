using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {upgrades, gameplay}

public class RestaurantMain : MonoBehaviour {

	private State currentState;
	private PlayerScript playerScript;
	private CustomerGenerator customerGenerator;

	// Use this for initialization
	void Start () {
		//always start the scene with buying upgrades
		currentState = State.upgrades;

		//the game is paused in the upgrades state
		playerScript = GameObject.Find("Cat").GetComponent<PlayerScript>();
        customerGenerator = GameObject.Find("Customer Line").GetComponent<CustomerGenerator>();
		Pause();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == State.upgrades)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.up);
				if (hit.collider)
				{
					Vector3 loc = hit.collider.transform.position;
					//nothing should happen unless a utensil is clcicked
					// if (hit.collider.tag == "ingredients")
					// {
					// 	hit.collider.GetComponent<BoxScript>().OnClick();
					// 	m_locations.Enqueue(new Vector3(loc.x, loc.y + 2, loc.z));
					// }
					//access the utensil and check if the player wants to upgrade it
					if(hit.collider.tag == "utensil")
					{
						
					}
					// else if(hit.collider.tag == "customer")
					// {
					// 	hit.collider.GetComponent<Customer>().OnClick();
					// 	m_locations.Enqueue(new Vector3(loc.x - 5, loc.y + 1, loc.z));
					// }
				}

			}
		}
	}

	public void Pause()
    {
        playerScript.enabled = !playerScript.enabled;
        customerGenerator.enabled = !customerGenerator.enabled;
    }

	public void FinishedUpgrades() {
		currentState = State.gameplay;
		Destroy(GameObject.Find("UpgradesFinishedButton"));
		Pause();
	}
}
