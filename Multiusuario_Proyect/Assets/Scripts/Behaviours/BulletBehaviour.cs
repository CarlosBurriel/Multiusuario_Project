using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletBehaviour : NetworkBehaviour
{
    public NetworkVariable<int> BulletLife = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<int> BulletDamage = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public float BulletSpeed = 1;

    public bool IsPowerUp;

    public GameObject VFX;

    public ShootBehaviour ShootBehaviour;

    public override void OnNetworkSpawn()
    {
        BulletLife.OnValueChanged += BulletLifeLoseServerRPC;
        
    }
   

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            LifeLoseBulletServerRPC();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void LifeLoseBulletServerRPC()
    {    
        BulletLife.Value--;
    }

    [ServerRpc(RequireOwnership = false)]
    void BulletLifeLoseServerRPC(int previousValue, int newValue)
    {
       
        if (newValue < 0)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }

    private new void OnDestroy()
    {
        if (VFX) { Instantiate(VFX, transform.position, Quaternion.identity); }
    }


}
