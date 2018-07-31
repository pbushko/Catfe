using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : MonoBehaviour {

	ChefData chef;
	float m_fill;

	public SpriteRenderer body;
	public SpriteRenderer face;
	public SpriteRenderer accessory;

	// Use this for initialization
	void Start () {
		m_fill = 0;
		chef = EmployeeGenerator.GenerateChef();
		body.sprite = PlayerData.playerData.GetCatSprite(chef.sprites[0]);
		face.sprite = PlayerData.playerData.GetCatSprite(chef.sprites[1]);
		Debug.Log(chef.ToString());
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
			ChefData temp = EmployeeGenerator.GenerateChef();
			Debug.Log(temp.ToString());
			body.sprite = temp.sprites[0];
			face.sprite = temp.sprites[1];
			m_fill = 0;
		}
		*/
	}
}
