using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBehaviour : NetworkBehaviour
{
    #region Public Variables
    public NetworkVariable<int> Ammo = new NetworkVariable<int>(0 ,NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
    public int MaxAmmo = 5;
    public NetworkVariable<bool> HasPowerUp = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
   // public bool HasPowerUp = false;
    public GameObject Canon;

    public GameObject CommonBullet;

    public ParticleSystem ShootVFX;
    #endregion

    #region Private Variables
    public GameObject BulletHolder;
    
    private Collider ThisCollider;


    #endregion

    private PlayerControls ThisPlayerInputs;

    private PlayerAnimHandler playerAnimHandler;
     
    public struct BMaterial : INetworkSerializable
    {
        public Color bullmat;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter { serializer.SerializeValue(ref bullmat); }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            ThisPlayerInputs = new PlayerControls();

            ThisPlayerInputs.Enable();

            ThisPlayerInputs.PlayerInGame.Shoot.performed += OnShoot;

            Ammo.Value = MaxAmmo;

        }

    }


    private void Awake()
    {
        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();

        ThisCollider = GetComponent<Collider>();
        Canon = transform.GetChild(0).gameObject;
        ShootVFX = Canon.GetComponent<ParticleSystem>();
        Canon.SetActive(true);
        BulletHolder = CommonBullet;

    } 


    private void OnDisable() => ThisPlayerInputs.Disable();


    #region Shoot&Bullet Handling
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (Ammo.Value > 0)
        {
                OnProjectileSpawnServerRpc();
        }
    }

    public void OnProjectileSpawn()
    {
            ShootVFX.Play();
            playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.ATTACK);
            Ammo.Value--;

            SetBulletServerRPC();            
            
            if (HasPowerUp.Value) { HasPowerUp.Value = false; BulletHolder = CommonBullet; }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnProjectileSpawnServerRpc()
    {
        OnProjectileSpawn();
    }
    #endregion
   

    [ServerRpc(RequireOwnership = false)]
    public void UpdateAmmoServerRPC()
    {
        if (BulletHolder.GetComponent<BulletBehaviour>().IsPowerUp) { HasPowerUp.Value = true; Ammo.Value++; } else { Ammo.Value = MaxAmmo; }
        if (Ammo.Value > MaxAmmo) { Ammo.Value = MaxAmmo; }
    }

    [ServerRpc]
    public void DespawnAmmoPackageServerRPC(NetworkObjectReference g)
    {
        if(g.TryGet(out NetworkObject other))
        {
            other.gameObject.GetComponent<NetworkObject>().Despawn(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetBulletServerRPC(ServerRpcParams serverRpcParams = default)
    {
        GameObject Projectile = Instantiate(BulletHolder, Canon.transform.position, Canon.transform.rotation);
        if (HasPowerUp.Value) HasPowerUp.Value = false; BulletHolder = CommonBullet;
        Projectile.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId, true);
        Projectile.GetComponent<Rigidbody>().velocity = transform.forward * Projectile.GetComponent<BulletBehaviour>().BulletSpeed;

        Physics.IgnoreCollision(ThisCollider, Projectile.GetComponent<Collider>());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AmmoPackage"))
        {
            ChangeBulletServerRPC(other.gameObject);
            UpdateAmmoServerRPC();
            DespawnAmmoPackageServerRPC(other.gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeBulletServerRPC(NetworkObjectReference g)
    {
        if(g.TryGet(out NetworkObject other))
        {
            BulletHolder = other.GetComponent<AmmoPackageBehaviour>().ThisPackageBullet;
        }
        
    }
}
