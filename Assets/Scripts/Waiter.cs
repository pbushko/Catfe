using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waiter : MonoBehaviour {

	public WaiterData waiter;
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
		//RefreshWaiter(waiter);
		//waiter = EmployeeGenerator.GenerateWaiter();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RefreshWaiter(WaiterData newData)
	{
		waiter = newData;
		if (!isUI)
		{
			body.sprite = PlayerData.playerData.GetCatSprite(waiter.sprites["body"]);
			face.sprite = PlayerData.playerData.GetCatSprite(waiter.sprites["face"]);
		}
		else
		{
			bodyImage.sprite = PlayerData.playerData.GetCatSprite(waiter.sprites["body"]);
			faceImage.sprite = PlayerData.playerData.GetCatSprite(waiter.sprites["face"]);
		}
	}

}
