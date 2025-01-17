using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasableUsername : MonoBehaviour
{
    public string username;

    public static PasableUsername instance;


    private void Awake()
    {
        instance = this; 
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeScene()
    {
        username = "AAAAAAAAAA";
        SceneManager.LoadScene("GameLevel2");
    }
   
}
