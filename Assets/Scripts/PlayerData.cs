using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores all the data that the game has to keep track of between levels
public class PlayerData {

	//the money the player currently has; will carry over from level to level
	public int m_money;

	//the setup of the restaurant
	public GameObject oven1;
	public GameObject oven2;
	public GameObject counter1;
	public GameObject counter2;

	//the ingredients that can be used for the level
	public GameObject ingredient1;
	public GameObject ingredient2;
	public GameObject ingredient3;
	public GameObject ingredient4;

	//cat employees and toys for the catfe levels
	public List<GameObject> catEmployees;
	public List<GameObject> catToys;

	//any other game objects that will be saved... like a blender or something


}
