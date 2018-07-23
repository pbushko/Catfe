using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour {

	WaiterData waiter;
	float m_fill;

	public SpriteRenderer body;
	public SpriteRenderer face;
	public SpriteRenderer accessory;

	// Use this for initialization
	void Start () {
		m_fill = 0;
		waiter = EmployeeGenerator.GenerateWaiter();
		body.sprite = waiter.sprites[0];
		face.sprite = waiter.sprites[1];
		Debug.Log(waiter.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
