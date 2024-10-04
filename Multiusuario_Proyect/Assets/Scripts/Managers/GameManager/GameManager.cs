using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

    public GameState State;

    private void Awake()
    {
        if (Instance == null)
        Instance = this;
    }

    public void UpdateGameState(GameState CurrentState)
    {
        State = CurrentState;

        switch (State)
        {
            case GameState.LoginOrRegister:
                break;
            case GameState.RoundStart:
                break;
            case GameState.RoundEnd:
                break;
            case GameState.Victory:
                break;
            case GameState.Defeat:
                break;
        }
    }


    public enum GameState
    {
        LoginOrRegister, RoundStart, RoundEnd, Victory, Defeat
    }
}