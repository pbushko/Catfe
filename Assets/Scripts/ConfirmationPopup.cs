using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmationPopup : MonoBehaviour
{
    public TextMeshProUGUI decorName;
    public TextMeshProUGUI decorCost;
    public TextMeshProUGUI decorAtmosphere;
    public TextMeshProUGUI decorLocation;
    public TextMeshProUGUI decorDescription;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI upgradeCookTime;

    private int cost;
    private GameObject toDeactivate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDecorationText(GameObject pref, DecorationData d)
    {
        decorName.text = "" + d.name;
        decorCost.text = "Cost: " + d.cost;
        decorAtmosphere.text = "Atmosphere: " + d.atmosphere;
        decorLocation.text = "Location: " + d.location;
        decorDescription.text = "" + d.description;
        toDeactivate = pref;
        cost = d.cost;
    }

    public void CompletePurchase()
	{
        //if the player doesn't have enough money, don't purchase it
        if (PlayerData.playerData.playerMoney < cost) {
            Debug.Log("Not enough money!");
        }
        else {
            Debug.Log("" + PlayerData.playerData.playerMoney + " cost: " + cost);
            CatfePlayerScript.script.PurchaseItem(true);
            CatInventory.catInv.SortChoice();
            MoneyTracker.ChangeMoneyCount(-cost);
            toDeactivate.SetActive(false);
            gameObject.SetActive(false);
        }
	}

	public void RejectPurchase()
	{
        Debug.Log("" + PlayerData.playerData.playerMoney + " cost: " + cost);
		CatfePlayerScript.script.PurchaseItem(false);
		gameObject.SetActive(false);
	}

}
