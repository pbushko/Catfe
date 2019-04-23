using PlayFab;
using PlayFab.ServerModels;
using PlayFab.Events;
using PlayFab.ClientModels;
using PlayFab.Json;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;
using System.Collections.Generic;

public class PlayFabLogin : MonoBehaviour
{

    public string id;
    public PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams infoRequestParams;

    public PlayFabIAP iap;

    public void Start()
    {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)){
            PlayFabSettings.TitleId = "144"; // Please change this value to your own titleId from PlayFab Game Manager
        }
    #if UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
        request.InfoRequestParameters = infoRequestParams;
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, result => {
            OnLoginSuccess(result);
        }, OnLoginFailure);
    #endif
    #if UNITY_IOS

    #endif
    }

    private void OnLoginSuccess(LoginResult result)
    {
        //for testing the customdata of items
        //Dictionary<string, string> i = new Dictionary<string, string>();
        //i.Add("outfit", "{\"hat\":\"wow\",\"glasses\":\"sunglasses\"}");
        //SetItemCutsomData(i, "891C2D292A879E29", "5055A279219519E2");
        Debug.Log("Logged in!");
        CatInventory.catInv.GetPlayFabDecor();
        PremiumMoneyTracker.SetMoney(result.InfoResultPayload.UserVirtualCurrency["PM"]); 
        MoneyTracker.SetMoneyText(result.InfoResultPayload.UserVirtualCurrency["NM"]);
        iap.RefreshIAPItems();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public static void GetMoney()
    {
        PlayFab.ClientModels.GetUserInventoryRequest request = new PlayFab.ClientModels.GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, result => {
            MoneyTracker.SetMoneyText(result.VirtualCurrency["NM"]);
        }, error => {});
    }

    public static void SetItemCutsomData(Dictionary<string, string> newData, string itemInstanceId, string playerId)
    {
        UpdateUserInventoryItemDataRequest request = new UpdateUserInventoryItemDataRequest();
        request.ItemInstanceId = itemInstanceId;
        request.Data = newData;
        request.PlayFabId = playerId;
        PlayFabServerAPI.UpdateUserInventoryItemCustomData(request, result => {
            Debug.Log("data updated!");
        }, error => {
            Debug.LogError(error.ErrorMessage);
        });
    }

    public static List<Recipe> GetMinigameRecipes()
    {
        List<Recipe> toRet = new List<Recipe>();
        PlayFab.ClientModels.GetCatalogItemsRequest itemRequest = new PlayFab.ClientModels.GetCatalogItemsRequest();
		itemRequest.CatalogVersion = "Items";
		PlayFabClientAPI.GetCatalogItems(itemRequest, result => {
			List<PlayFab.ClientModels.CatalogItem> items = result.Catalog;
			foreach (PlayFab.ClientModels.CatalogItem i in items)
			{
				if(i.ItemClass == "Recipe")
				{
                    if (i.ItemId == "carrotSalad" || i.ItemId == "beefAndChicken" || i.ItemId == "carrotSoup")
                    {
					    toRet.Add(new Recipe(i));
                        Debug.Log(i.ItemId);
                    }
					
				}
			}
		}, error => {}
		);
        return toRet;
    }
    
}