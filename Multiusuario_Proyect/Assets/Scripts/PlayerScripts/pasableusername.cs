using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pasableusername : MonoBehaviour
{
    public string username;

    public static pasableusername instance;


    private void Awake()
    {
        instance = this; 
        DontDestroyOnLoad(this.gameObject);
    }


   
}
