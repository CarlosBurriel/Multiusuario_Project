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
    public NetworkVariable<int> total_deaths = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> total_powerups = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> total_ammo_gathered = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
                if (IsOwner) { StartCoroutine(CreateGameDataCoroutines()); }
                
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
    List<PHPHandler> Losers = new List<PHPHandler>();
    public void SelectWinner()
    {
        int LowerDeathCount = 10;
        for (int i = 0; i < Players.Count; i++)
        {
            if(Players[i].Deaths.Value < LowerDeathCount)
            {
                LowerDeathCount = Players[i].Deaths.Value;
                Winners.Add(Players[i]);
            }
            else
            {
                Losers.Add(Players[i]);
            }
        }
        if(Winners.Count > 1)
        {
            if(Winners[0].Deaths.Value > Winners[1].Deaths.Value)
            {
                Losers.Add(Winners[0]);
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
        if (IsOwner)
        {
        StartCoroutine(GameDataCoroutines());
        }
        StartCoroutine(WinnersCoroutines());
        for (int i = 0; i < Losers.Count; i++)
        {
            print(Losers[i].PlayerUsername);
            StartCoroutine(LosersCoroutines(i));
        }
       for(int i=0; i< Winners.Count; i++)
        {
            print(Winners[i].PlayerUsername);
        }

       

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
    IEnumerator LosersCoroutines(int i)
    {

        WWWForm form = new WWWForm();
        form.AddField("username", Losers[i].PlayerUsername);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/Defeats.php", form))
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
    IEnumerator CreateGameDataCoroutines()
    {

        WWWForm form = new WWWForm();
        

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/CreateGamesID.php", form))
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
    IEnumerator GameDataCoroutines()
    {

        WWWForm form = new WWWForm();
        form.AddField("deaths", total_deaths.Value);
        form.AddField("powerups", total_powerups.Value);
        form.AddField("ammo", total_ammo_gathered.Value);
        form.AddField("winner", Winners[0].PlayerUsername);
        form.AddField("loserA", Losers[0].PlayerUsername);
        if (Losers.Count <= 2)
        {
            form.AddField("loserB", "furrito77");
        }
        else
        {
            form.AddField("loserB", Losers[1].PlayerUsername);
        }
        if (Losers.Count <= 3)
        {
            form.AddField("loserC", "furrito77");
        }
        else
        {
        form.AddField("loserC", Losers[2].PlayerUsername);
        }
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/GamesID.php", form))
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



    public enum GameState
    {
       Standby, RoundStart, RoundEnd
    }
}