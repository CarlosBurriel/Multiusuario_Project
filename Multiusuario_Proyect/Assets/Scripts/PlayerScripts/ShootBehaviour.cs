using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBehaviour : MonoBehaviour
{
    #region Public Variables
    [HideInInspector]public int Ammo = 0;
    public int MaxAmmo = 5;
    public bool HasPowerUp = false;

    public ScriptableBullet CommonBullet;
    #endregion

    #region Private Variables
    private ScriptableBullet BulletHolder;
    private Transform Canon;
    private Collider ThisCollider;

    private UI_Manager UI;
    #endregion

    private void Start()
    {
        UI = transform.GetChild(2).gameObject.GetComponent<UI_Manager>();

        ThisCollider = GetComponent<Collider>();
        Canon = transform.GetChild(0);
        BulletHolder = CommonBullet;
    }

    #region InputHandling
    private PlayerControls ThisPlayerInputs;

    private void Awake() => ThisPlayerInputs = new PlayerControls();

    private void OnEnable()
    {
        ThisPlayerInputs.Enable();

        ThisPlayerInputs.PlayerInGame.Shoot.performed += OnShoot;
    }

    private void OnDisable() => ThisPlayerInputs.Disable();

    #endregion

    #region Shoot&Bullet Handling
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (Ammo > 0)
        {
            Ammo--;
            OnProjectileSpawn();
        }
    }

    public void OnProjectileSpawn()
    {
            UI.UpdateAmmoUI();
            GameObject Projectile = Instantiate(BulletHolder.BulletType, Canon.transform.position, Canon.transform.rotation);
            
            Physics.IgnoreCollision(ThisCollider, Projectile.GetComponent<Collider>());
            if (BulletHolder.PhysicMaterial) { Projectile.GetComponent<Collider>().material = BulletHolder.PhysicMaterial; }
            if (BulletHolder.BulletMaterial) { Projectile.GetComponent<MeshRenderer>().material = BulletHolder.BulletMaterial; }
            Projectile.GetComponent<BulletBehaviour>().BulletDamage = BulletHolder.BulletDamage;
            Projectile.GetComponent<BulletBehaviour>().BulletLife = BulletHolder.BulletBounces;
            Projectile.GetComponent<Rigidbody>().velocity = transform.forward * BulletHolder.LaunchSpeed;
            
            if (HasPowerUp) { HasPowerUp = false; BulletHolder = CommonBullet; }
    }
    #endregion
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AmmoPackage"))
        {
            BulletHolder = other.GetComponent<AmmoPackageBehaviour>().ThisPackageBullet;
            if (BulletHolder.IsPowerUp) {HasPowerUp = true; Ammo++; } else { Ammo = MaxAmmo; }
            if (Ammo > MaxAmmo) {Ammo = MaxAmmo; }
            UI.UpdateAmmoUI();
            Destroy(other.gameObject);
        }
    }
}