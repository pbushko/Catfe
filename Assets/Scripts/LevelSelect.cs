using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour {

	//save once you reach the level select screen; that is before you choose
	//a level and after you finish a level
	void Start() {
		PlayerData.playerData.Save();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hit = Physics2D.Raycast(mousePos, Vector2.up).collider;
			if (hit)
			{
				if (hit.tag == "levelButton")
				{
					int level = hit.GetComponent<LevelButtonData>().levelNum;
					List<Ingredients> ingreds = new List<Ingredients>();

					//load the lettuce and carrot
					if (level == 0)
					{
						ingreds.Add(Ingredients.Carrot);
						ingreds.Add(Ingredients.Lettuce);
					}
					//load the chicken, beef, and carrot
					if (level == 1)
					{
						ingreds.Add(Ingredients.Carrot);
						ingreds.Add(Ingredients.Beef);
						ingreds.Add(Ingredients.Chicken);
					}
					RestaurantMain.ingredients = ingreds;
					Application.LoadLevel("restaurant");
					
				}
			}
		}
	}

}
