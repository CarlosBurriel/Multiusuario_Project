using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoPlayer : MonoBehaviour
{
    public GameObject bullet;
    public GameObject canon;
    public Transform Tank;

    [Header("Municion")]
    public bool CanShoot;
    public int ActualAmmo = 0;
    public int MaxAmmo = 3;
    public SphereCollider AmmoItemTrigger;
    public GameObject AmmoItemGO;

    public float launchSpeed;

    [Header("PowerUp Rebote")]
    public bool rebote;

    public bool tiene_powerup;
    public SphereCollider PowerUpTrigger;
    public GameObject PowerUpGO;

    public HitBullet bala;

    private void OnTriggerEnter(Collider other)
    {
        if(other == AmmoItemTrigger)
        {
            AmmoItemGO.SetActive(false);
            ActualAmmo = MaxAmmo;
            CanShoot = true;
        }
        if (other == PowerUpTrigger)
        {
            PowerUpGO.SetActive(false);
            tiene_powerup = true;
        }
    }

    void Update()
    {
        if (CanShoot)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateBullet();
            }
        }

        if(ActualAmmo <= 0)
        {
            CanShoot = false;
        }

    }
    void CreateBullet()
    {
        if(tiene_powerup == true)
        {
            bala.rebotepared = true;
            bala.tienepoder = true;
            tiene_powerup = false;
            print("balarebota");
        }
        GameObject shell = Instantiate(bullet, canon.transform.position, canon.transform.rotation);
        shell.GetComponent<Rigidbody>().velocity = launchSpeed * Tank.forward;
        ActualAmmo -= 1;

    }
}
