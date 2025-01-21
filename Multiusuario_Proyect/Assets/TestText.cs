using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class TestText : MonoBehaviour
{
    public string StringToGetINT;

    public int GameID;
    void Start()
    {
        StringToGetINT = string.Concat(StringToGetINT.Where(Char.IsDigit));
        GameID = Int32.Parse(StringToGetINT);
        //print(GameID);
    }

   
}
