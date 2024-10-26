using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region UI Variables Balas
    public Image Bullet1;
    public Image Bullet2;
    public Image Bullet3;
    public Image Bullet4;
    public Image Bullet5;
    #endregion

    public TextMeshProUGUI HealthText;

    //Script de Disparo del Player.
    public DisparoPlayer Shoot;
    //Script de Player General
    public PlayerMovement Player;

    void Start()
    {
        //No se si el Start es realmente necesario.
        #region Inicializacion Balas 
        Bullet1.enabled = false;
        Bullet2.enabled = false;
        Bullet3.enabled = false;
        Bullet4.enabled = false;
        Bullet5.enabled = false;
        #endregion


        
    }

    void Update()
    {
        //Creo que esto se puede hacer en una funcion desde el script del disparo, para no hacerlo en un Update.
        //Pero prefiero esperarme a tener todos los scripts en una sola escena. 
        #region Bullet Visual Switch
        switch (Shoot.ActualAmmo)
        {
            case 0:
                Bullet1.enabled = false;
                Bullet2.enabled = false;
                Bullet3.enabled = false;
                Bullet4.enabled = false;
                Bullet5.enabled = false;
                break;

            case 1:
                Bullet1.enabled = true;
                Bullet2.enabled = false;
                Bullet3.enabled = false;
                Bullet4.enabled = false;
                Bullet5.enabled = false;
                break;

            case 2:
                Bullet1.enabled = true;
                Bullet2.enabled = true;
                Bullet3.enabled = false;
                Bullet4.enabled = false;
                Bullet5.enabled = false;
                break;

            case 3:
                Bullet1.enabled = true;
                Bullet2.enabled = true;
                Bullet3.enabled = true;
                Bullet4.enabled = false;
                Bullet5.enabled = false;
                break;

            case 4:
                Bullet1.enabled = true;
                Bullet2.enabled = true;
                Bullet3.enabled = true;
                Bullet4.enabled = true;
                Bullet5.enabled = false;
                break;

            case 5:
                Bullet1.enabled = true;
                Bullet2.enabled = true;
                Bullet3.enabled = true;
                Bullet4.enabled = true;
                Bullet5.enabled = true;
                break;

            default:
                print("NO BULLETS");
                break;
        }
        #endregion

        HealthText.text = "Health: " + Player.vidasPlayer.ToString();

    }
}
