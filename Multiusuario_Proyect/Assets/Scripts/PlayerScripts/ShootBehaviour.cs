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
    public bool HasPowerUp = false;
    public GameObject Canon;

    public GameObject CommonBullet;

    public ParticleSystem ShootVFX;
    #endregion

    #region Private Variables
    private GameObject BulletHolder;
    
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
            
            if (HasPowerUp) { HasPowerUp = false; BulletHolder = CommonBullet; }
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
        if (BulletHolder.GetComponent<BulletBehaviour>().IsPowerUp) { HasPowerUp = true; Ammo.Value++; } else { Ammo.Value = MaxAmmo; }
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
        Debug.Log(Projectile);
        Projectile.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId, true);

        Physics.IgnoreCollision(ThisCollider, Projectile.GetComponent<Collider>());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AmmoPackage"))
        {
            BulletHolder = other.GetComponent<AmmoPackageBehaviour>().ThisPackageBullet;
            UpdateAmmoServerRPC();
            DespawnAmmoPackageServerRPC(other.gameObject);
        }
    }
}
