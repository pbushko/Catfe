using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabIAPData : MonoBehaviour
{
    public IAPData data;

    public Text display_name;
    public Text description;
    public Text price;

    public Image sprite;

    // Start is called before the first frame update
    void Start()
    {
        //set up the info to appear in the shop menu
        if (data != null)
        {
            ResetData(data);
        }
    }

    public void ResetData(IAPData newData)
    {
        data = newData;
        if (display_name != null)
        {
            display_name.text = newData.name;
        }
        if (price != null)
        {
            int pricebase = 100;
            price.text = "$ " + newData.price/pricebase +"."+newData.price%pricebase;
        }
        if (description != null)
        {
            description.text = "" + newData.description;
        }
        if (sprite != null)
        {
            sprite.sprite = CatfePlayerScript.script.GetDecorationSprite(newData.sprite);
        }
    }

    public void OnClick()
    {
        PlayFabIAP.play.BuyProductID(data.id);
    }
}

[System.Serializable]
public class IAPData
{
    public string id;
    public string name;
    public string description;
    public int price;

    public string sprite;

    public IAPData(CatalogItem iapItem)
    {
        id = iapItem.ItemId;
        name = iapItem.DisplayName;
        description = iapItem.Description;
        price = (int)iapItem.VirtualCurrencyPrices["RM"];
        sprite = iapItem.ItemImageUrl;
    }
}