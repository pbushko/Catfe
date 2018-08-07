using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe : MonoBehaviour
{
    //public PlayerScript player;
    //public CustomerGenerator customerGenerator;

    //stores the LoadingBar for the utensils
    private static Queue<GameObject> m_loaders = new Queue<GameObject>();
    //stores the location of the next thing in the queue so the cat can move to it
    private static Queue<Vector3> m_locations = new Queue<Vector3>();

    public GameObject newRestaurantCanvas;

    public Sprite restaurantToPlace;

    private bool toShow;

    // Use this for initialization
    void Start ()
    {
        //newRestaurantCanvas.SetActive(false);
        toShow = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (toShow && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.up);
            if (hit.collider)
            {
                //Vector3 loc = hit.collider.transform.position;
                
            }
        }
	}

    //what to do when it's a new game
    public void NewGameRestaurantChoice()
    {
        //newRestaurantCanvas.SetActive(true);
    }

    public void SetNewRestaurantArea(Sprite s)
    {
        newRestaurantCanvas.SetActive(false);
        restaurantToPlace = s;
        toShow = true;
    }

}
