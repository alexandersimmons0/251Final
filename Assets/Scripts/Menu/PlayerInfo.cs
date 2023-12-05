using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerInfo : MonoBehaviour{
    //public GameObject loginScreen;
    [HideInInspector]
    public PlayerProfileModel profile;

    public static PlayerInfo instance;

    void Awake(){
        instance = this;      
    }

    public void LoggedIn(){
        GetPlayerProfileRequest getProfileRequest = new GetPlayerProfileRequest{
            PlayFabId = LoginRegister.instance.playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints{
                ShowDisplayName = true
            },
        };

        PlayFabClientAPI.GetPlayerProfile(getProfileRequest,
            result => {
                profile = result.PlayerProfile;
                Debug.Log("Loaded in player: " + profile.DisplayName);
                Invoke("LogInSuccess", 0.2f);
            },
            error => Debug.Log(error.ErrorMessage)
        );
    }

    public void LogInSuccess(){
        GetComponent<MenuManager>().OnLogInPlayer(profile);
    }
}