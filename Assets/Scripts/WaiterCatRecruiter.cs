using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaiterCatRecruiter : MonoBehaviour {

	private List<WaiterData> waiters;

	public WaiterCatRecruitStats cat1;
	public WaiterCatRecruitStats cat2;
	public WaiterCatRecruitStats cat3;

	// Use this for initialization
	void Start () {
		RefreshCats();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RefreshCats()
	{
		//getting the waiter data to put into the buy menu
		waiters = new List<WaiterData>();
		//putting in 3 waiters
		waiters.Add(EmployeeGenerator.GenerateWaiter());
		waiters.Add(EmployeeGenerator.GenerateWaiter());
		waiters.Add(EmployeeGenerator.GenerateWaiter());

		cat1.ResetData(waiters[0]);
		cat2.ResetData(waiters[1]);
		cat3.ResetData(waiters[2]);

	}
}
