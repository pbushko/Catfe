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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDecorationText(DecorationData d)
    {
        decorName.text = "" + d.name;
        decorCost.text = "Cost: " + d.cost;
        decorAtmosphere.text = "Atmosphere: " + d.atmosphere;
        decorLocation.text = "Location: " + d.location;
        decorDescription.text = "" + d.description;
    }
}
