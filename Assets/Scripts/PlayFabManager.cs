using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
  //  [Header("Window")]
    public GameObject nameWindow;
    public GameObject leaderboardWindow;

  //  [Header("Display name Window")]
    public GameObject nameError;
    public InputField nameInput;        
 
  //   [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
           name = result.InfoResultPayload.PlayerProfile.DisplayName;
        if (name == null)
            nameWindow.SetActive(true);
    }
    public void SubmitNameButton() {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,

        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        //PlayFabClientAPI.AddGenericID(GenericServiceId, OnDisplayNameUpdate,nameInput,rowsParent );
        //if(name)
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated Display Name");
        //leaderboardWindow.SetActive(true);
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while lagging in/crerating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderBord(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {new StatisticUpdate { StatisticName="PlatformScore",
            Value = score} }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach(Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position+1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();


            Debug.Log(string.Format("PLACE: {0} | ID: {1} | VALUE: {2}",
                item.Position, item.PlayFabId, item.StatValue));
        }
    }
}