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

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
