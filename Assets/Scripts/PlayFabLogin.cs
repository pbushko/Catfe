using PlayFab;
using PlayFab.ServerModels;
using PlayFab.Events;
using PlayFab.ClientModels;
using PlayFab.Json;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayFabLogin : MonoBehaviour
{

    public string id;
    public PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams infoRequestParams;

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
        MoneyTracker.SetMoneyText(result.InfoResultPayload.UserVirtualCurrency["NM"]);
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
    
}