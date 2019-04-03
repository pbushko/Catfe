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
            MoneyTracker.SetMoneyText(result.InfoResultPayload.UserVirtualCurrency["NM"]);
            OnLoginSuccess(result);
            TestInv();
        }, OnLoginFailure);
    #endif
    #if UNITY_IOS

    #endif
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        CatInventory.catInv.GetPlayFabDecor();
        
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


    public static void TestInv()
    {
        Debug.Log("TestInv");
        UpdateUserInventoryItemDataRequest request = new UpdateUserInventoryItemDataRequest();
        Dictionary<string, string> newData = new Dictionary<string, string>();
        newData.Add("howdy", "y'all");
        request.ItemInstanceId = "891C2D292A879E29";
        request.Data = newData;
        request.PlayFabId = "5055A279219519E2";
        PlayFabServerAPI.UpdateUserInventoryItemCustomData(request, result => {
            Debug.Log("worked!");
        }, error => {
            Debug.LogError(error.ErrorMessage);
        });
    }
    

}