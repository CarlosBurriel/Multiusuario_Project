using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBehaviour : MonoBehaviour
{

    [SerializeField]private int Ammo = 0;
    public int MaxAmmo = 5;
    public bool HasPowerUp = false;


    public ScriptableBullet Bullet;
    private Transform Canon;

    private void Start()
    {
        Canon = transform.GetChild(0);
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
            GameObject Projectile = Instantiate(Bullet.BulletType, Canon.transform.position, Canon.transform.rotation);
            OnProjectileSpawn(Projectile);
        }
    }

    public void OnProjectileSpawn(GameObject ProjectileFunction)
    {
        if (Bullet.PhysicMaterial) { ProjectileFunction.GetComponent<Collider>().material = Bullet.PhysicMaterial; }
        if (Bullet.BulletMaterial) { ProjectileFunction.GetComponent<MeshRenderer>().material = Bullet.BulletMaterial; }
        ProjectileFunction.GetComponent<BulletBehaviour>().BulletLife = Bullet.BulletBounces;
        ProjectileFunction.GetComponent<Rigidbody>().velocity = transform.forward * Bullet.LaunchSpeed;
    }
    #endregion
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AmmoPackage")
        {
            Bullet = other.GetComponent<AmmoPackageBehaviour>().ThisPackageBullet;
            Destroy(other.gameObject);
        }
    }
}
