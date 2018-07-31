﻿using System.Collections;
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
		body.sprite = PlayerData.playerData.GetCatSprite(waiter.sprites[0]);
		face.sprite = PlayerData.playerData.GetCatSprite(waiter.sprites[1]);
		Debug.Log(waiter.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		/*
		//just some testing code
		if (m_fill <= 5.0f)
        {
            m_fill += 0.05f;
        }
		else
		{
			WaiterData temp = EmployeeGenerator.GenerateWaiter();
			Debug.Log(temp.ToString());
			body.sprite = temp.sprites[0];
			face.sprite = temp.sprites[1];
			m_fill = 0;
		}
		*/
	
	}

}