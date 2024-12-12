using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptTest : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.RoundStart);
    }

    private void Update()
    {
        if (transform.childCount == 0 && SceneManager.GetActiveScene().name == "AlTest")
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Victory);
        }
    }

}
