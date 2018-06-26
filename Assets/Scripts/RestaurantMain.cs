using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State {upgrades, popup, gameplay}

public class RestaurantMain : MonoBehaviour {

	private State currentState;
	private PlayerScript playerScript;
	private CustomerGenerator customerGenerator;

	//private System.Action<GameObject> yesButtonAction;
	public GameObject popUp;
	public GameObject curPopUp;
	public Text popUpText;

	// Use this for initialization
	void Start () {
		//always start the scene with buying upgrades
		currentState = State.upgrades;

		popUpText.enabled = false;

		//the game is paused in the upgrades state
		playerScript = GameObject.Find("Cat").GetComponent<PlayerScript>();
        customerGenerator = GameObject.Find("Customer Line").GetComponent<CustomerGenerator>();
		Pause();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.up);
			if (hit.collider)
			{
				//Vector3 loc = hit.collider.transform.position;
				//nothing should happen unless a utensil is clcicked
				// if (hit.collider.tag == "ingredients")
				// {
				// 	hit.collider.GetComponent<BoxScript>().OnClick();
				// 	m_locations.Enqueue(new Vector3(loc.x, loc.y + 2, loc.z));
				// }
				//access the utensil and check if the player wants to upgrade it
				if (currentState == State.upgrades)
				{
					if(hit.collider.tag == "utensil")
					{
					//will go into choosing to upgrade or not on the popup
					currentState = State.popup;
					setPopUp(true);
					}
				}
				if (currentState == State.popup)
				{
					//don't want to buy the upgrade
					if (hit.collider.tag == "noButton")
					{
						currentState = State.upgrades;
						setPopUp(false);
					}
					else if (hit.collider.tag == "yesButton")
					{

					}
				}
			}
		}	
		
	}

	//either enable or disable to popup
	private void setPopUp(bool t)
	{
		if (t)
		{
			popUpText.enabled = true;
			curPopUp = (GameObject)Instantiate(popUp);
		}
		else
		{
			popUpText.enabled = false;
			Destroy(curPopUp);
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
		Destroy(popUp);
		popUpText.enabled = false;
		Pause();
	}
}
