using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

    public GameState State;

    private void Awake()
    {
        if (Instance == null) {Instance = this; DontDestroyOnLoad(gameObject);} else {Destroy(gameObject);}
    }

    public void UpdateGameState(GameState CurrentState)
    {
        State = CurrentState;

        switch (State)
        {
            case GameState.LoginOrRegister:
                break;
            case GameState.RoundStart:
                SceneManager.LoadScene("AlTest");
                break;
            case GameState.RoundEnd:
                break;
            case GameState.Victory:
                SceneManager.LoadScene("Winner");
                break;
            case GameState.Defeat:
                SceneManager.LoadScene("Loser");
                break;
        }
    }


    public enum GameState
    {
        LoginOrRegister, RoundStart, RoundEnd, Victory, Defeat
    }
}