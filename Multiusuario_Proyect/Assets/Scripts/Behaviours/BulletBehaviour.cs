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

    private Collider col;
    private Renderer render;

    public override void OnNetworkSpawn()
    {
        BulletLife.OnValueChanged += BulletLifeLoseServerRPC;
        col = GetComponent<Collider>();
        render = GetComponent<Renderer>();
        
    }
   

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            LifeLoseBulletServerRPC();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            
            BulletDespawnServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void LifeLoseBulletServerRPC()
    {    
        BulletLife.Value--;
    }

    [ServerRpc(RequireOwnership = false)]
    public void BulletDespawnServerRPC()
    {
        render.enabled = false;
        col.enabled = false;
        BulletLife.Value -= BulletLife.Value;
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
