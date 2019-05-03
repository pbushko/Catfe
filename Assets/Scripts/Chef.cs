using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chef : MonoBehaviour {

	ChefData chef;
	float m_fill;

	public SpriteRenderer body;
	public SpriteRenderer face;
	public SpriteRenderer accessory;

	public Image bodyImage;
	public Image faceImage;
	public Image accessoryImage;

	public bool isUI;

	// Use this for initialization
	void Start () {
		m_fill = 0;
		//chef = EmployeeGenerator.GenerateChef();
		//body.sprite = PlayerData.playerData.GetCatSprite(chef.sprites[0]);
		//face.sprite = PlayerData.playerData.GetCatSprite(chef.sprites[1]);
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

	public void RefreshChef(ChefData newData)
	{
		chef = newData;
		if (!isUI)
		{
			body.sprite = PlayerData.playerData.GetCatSprite(chef.sprites["body"]);
			face.sprite = PlayerData.playerData.GetCatSprite(chef.sprites["face"]);
		}
		else
		{
			bodyImage.sprite = PlayerData.playerData.GetCatSprite(chef.sprites["body"]);
			faceImage.sprite = PlayerData.playerData.GetCatSprite(chef.sprites["face"]);
		}
	}

}
