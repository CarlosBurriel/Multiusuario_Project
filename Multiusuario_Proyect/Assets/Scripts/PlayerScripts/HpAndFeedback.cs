using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class HpAndFeedback : NetworkBehaviour
{
    public int MaxHP;
    [SerializeField]public NetworkVariable<int> CurrentHP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Material FlashMaterial;

    private SkinnedMeshRenderer Render;
    private Material OwnMaterial;
    private Collider col;
    private Rigidbody rb;

    public GameObject UI;

    private PlayerAnimHandler playerAnimHandler;
    private ShootBehaviour playerShoot;
    private PlayerSmovement playerMove;

    public List<GameObject> SpawnPoints = new List<GameObject>();

    private void Awake()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (GameObject obj in objetos)
        {
            SpawnPoints.Add(obj);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) { UI.SetActive(true); }

        base.OnNetworkSpawn();
        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();
        playerShoot = GetComponent<ShootBehaviour>();
        playerMove = GetComponent<PlayerSmovement>();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        Render = GetComponentInChildren<SkinnedMeshRenderer>();
        OwnMaterial = Render.material;
 
        CurrentHP.Value = MaxHP;
    }

    private void Start()
    {
       transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Capacity)].transform.position;
    }
    
        
    


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            OnHitFeedback();
            TakeDamageClientRPC(other.gameObject);
        }
    }

    [ClientRpc]
    public void TakeDamageClientRPC(NetworkObjectReference g)
    {
        if (g.TryGet(out NetworkObject other))
        {
            CurrentHP.Value -= other.gameObject.GetComponent<BulletBehaviour>().BulletDamage.Value;
        }
    }

    public void OnHitFeedback()
    {
        SizeChange();
        StartCoroutine(Flash());
    }

    public void SizeChange()
    {
        transform.DOPunchScale(transform.localScale * Random.Range(1, 1.3f), 0.5f);
    }

    public IEnumerator Flash()
    {
        Render.material = FlashMaterial;   
        yield return new WaitForSeconds(0.2f);
        Render.material = OwnMaterial;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckLifeServerRPC(int previousValue, int newValue)
    {
        if (newValue <= 0)
        {
            playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.DEATH);
            GetComponent<PlayerSmovement>().enabled = false;

            StartCoroutine(PlayerRespawn());
            
            //gameObject.GetComponent<NetworkObject>().Despawn();
           
            
            //GameManager.Instance.UpdateGameState(GameManager.GameState.Defeat);
        }
    }

    IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(1f);
        PlayerDespawnServerRPC();
       
        yield return new WaitForSeconds(2f);
        //gameObject.transform.position = PlayerSpawners[Random.Range(0, PlayerSpawners.Length)].transform.position;
        PlayerRespawnServerRPC();

    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerRespawnServerRPC()
    {
        playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.IDLE);
        playerMove.enabled = true;
        Render.enabled = true;
        playerShoot.Ammo.Value = playerShoot.MaxAmmo;
        CurrentHP.Value = MaxHP;
        col.enabled = true;
        transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Capacity)].transform.position;
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerDespawnServerRPC()
    {
        rb.velocity = Vector3.zero;
        Render.enabled = false;
        col.enabled = false;

    }


    private void OnEnable()
    {
        transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Capacity)].transform.position;
    }


}
