using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public GameState State;

    [Space]
    [Header("Duration of Play")]
    [Space]
    public float DurationOfPlay;
    public float DurationOfEnding;

    [Space]
    [Header("Game Manager Canvas")]
    [Space]
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ResolutionText;

    public NetworkVariable<int> NumberOfPlayers = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [HideInInspector] public List<PHPHandler> Players;
    private float CurrentTime;
    private bool Started = false;


    

    private void Awake()
    {
        if (Instance == null) { Instance = this;} else { Destroy(gameObject); }

        NumberOfPlayers.OnValueChanged += CheckForPlayStart;

        TimerText.text = "";
        ResolutionText.text = "";
    }

    private void CheckForPlayStart(int previousValue, int newValue)
    {
        if(newValue > 1 && !Started)
        {
            Started = true;
            UpdateGameState(GameState.RoundStart);
        }
    }

    public void UpdateGameState(GameState CurrentState)
    {
        State = CurrentState;

        switch (State)
        {
            case GameState.Standby:
                TimerText.text = "";
                ResolutionText.text = "";
                NumberOfPlayers.Value = 0;
                break;
            case GameState.RoundStart:
                CurrentTime = DurationOfPlay;
                TimerText.text = CurrentTime.ToString();

                SpawnerItemsScript.Instance.StartItemSpawner();
                StartCoroutine(Timer());
                break;
            case GameState.RoundEnd:
                StartCoroutine(Timer());
                break;
        }
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        CurrentTime--;
        TimerText.text = CurrentTime.ToString();
        if (CurrentTime < 0) 
        { 
            TimerText.text = 0.ToString();
            if(State == GameState.RoundStart) { SelectWinner(); }
            if (State == GameState.RoundEnd) { EndGame(); }
            
        }
        else
        {
            StartCoroutine(Timer());
        }
    }

        List<PHPHandler> Winners = new List<PHPHandler>();
    public void SelectWinner()
    {
        int LowerDeathCount = 10;
        for (int i = 0; i < Players.Count; i++)
        {
            if(Players[i].Deaths < LowerDeathCount)
            {
                LowerDeathCount = Players[i].Deaths;
                Winners.Add(Players[i]);
            }
        }
        if(Winners.Count > 1)
        {
            if(Winners[0].Deaths > Winners[1].Deaths)
            {
                Winners.Remove(Winners[0]);
            }
            ResolutionText.text = "Is a draw between: ";
            for(int i = 0;i < Winners.Count;i++)
            {
                if(i == 0) ResolutionText.text += Winners[0].PlayerUsername;
                ResolutionText.text += ", " + Winners[0].PlayerUsername;
                if (i == Winners.Count-1) ResolutionText.text += " and " + Winners[0].PlayerUsername;
            }
            UpdateGameState(GameState.RoundEnd);
        }
        else
        {
            ResolutionText.text = "The winner is " + Winners[0].PlayerUsername;
            UpdateGameState(GameState.RoundEnd);
        }
    }

    public void EndGame()
    {
        
        StartCoroutine(WinnersCoroutines());
        print(Winners[0].PlayerUsername);

        //StartCoroutine(LoserCoroutines());

       

        for (int i = 0; i < Players.Count;i++)
        {
            Destroy(Players[i].gameObject);
            Players.Remove(Players[i]);
        }
        NetworkManager.Singleton.Shutdown();
        if(SceneManager.GetActiveScene().name == "GameLevel") { SceneManager.LoadScene("GameLevel2"); } else { SceneManager.LoadScene("GameLevel"); }

    }

    IEnumerator WinnersCoroutines()
    {

        WWWForm form = new WWWForm();
        form.AddField("username",Winners[0].PlayerUsername);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/Winner.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                print(responseText);
               
            }
        }


    }
    /*
    IEnumerator LoserCoroutines()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", Winners[0].PlayerUsername);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/Loser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                if (responseText.Contains("success"))
                {

                    print("ole");


                }

            }
        }
    }
    */

    public enum GameState
    {
       Standby, RoundStart, RoundEnd
    }
}