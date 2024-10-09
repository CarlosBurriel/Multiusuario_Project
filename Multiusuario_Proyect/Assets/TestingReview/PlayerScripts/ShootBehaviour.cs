using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBehaviour : MonoBehaviour
{
    #region Public Variables
    [SerializeField]private int Ammo = 0;
    public int MaxAmmo = 5;
    public bool HasPowerUp = false;

    public ScriptableBullet CommonBullet;
    #endregion

    #region Private Variables
    private ScriptableBullet BulletHolder;
    private Transform Canon;
    #endregion

    private void Start()
    {
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
            GameObject Projectile = Instantiate(BulletHolder.BulletType, Canon.transform.position, Canon.transform.rotation);
            
            if (BulletHolder.PhysicMaterial) { Projectile.GetComponent<Collider>().material = BulletHolder.PhysicMaterial; }
            if (BulletHolder.BulletMaterial) { Projectile.GetComponent<MeshRenderer>().material = BulletHolder.BulletMaterial; }
            Projectile.GetComponent<BulletBehaviour>().BulletLife = BulletHolder.BulletBounces;
            Projectile.GetComponent<Rigidbody>().velocity = transform.forward * BulletHolder.LaunchSpeed;
            
            if (HasPowerUp) { HasPowerUp = false; BulletHolder = CommonBullet; }
    }
    #endregion
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AmmoPackage")
        {
            BulletHolder = other.GetComponent<AmmoPackageBehaviour>().ThisPackageBullet;
            if (BulletHolder.IsPowerUp) {HasPowerUp = true; } else { Ammo = MaxAmmo; }
            Destroy(other.gameObject);
        }
    }
}
