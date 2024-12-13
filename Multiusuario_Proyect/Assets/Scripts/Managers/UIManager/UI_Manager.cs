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

    private HpAndFeedback PlayerHP;

    private ShootBehaviour PlayerAmmo;

    private void Awake()
    {
        PlayerHP = GetComponentInParent<HpAndFeedback>();
        PlayerAmmo = GetComponentInParent<ShootBehaviour>();
        HealthText = GetComponentInChildren<TextMeshProUGUI>();

        PlayerHP.CurrentHP.OnValueChanged += UpdateHpUI;

        PlayerHP.CurrentHP.Value = PlayerHP.MaxHP;

        PlayerAmmo.Ammo.OnValueChanged += UpdateAmmoUI;

    }

    public void UpdateAmmoUI(int previousValue, int newValue)
    {
        switch (newValue)
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
    }

    
    public void UpdateHpUI(int previousValue, int newValue)
    {
        HealthText.text = "Health: " + newValue.ToString();
    }
}
