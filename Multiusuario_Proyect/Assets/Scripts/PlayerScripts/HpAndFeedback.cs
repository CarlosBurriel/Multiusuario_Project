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

    public GameObject UI;

    private PlayerAnimHandler playerAnimHandler;

    public GameObject[] PlayerSpawners= new GameObject[4];


    NetworkManager m_NetworkManager;

    private void Awake()
    {

        PlayerSpawners[] = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) { UI.SetActive(true); }

        base.OnNetworkSpawn();
        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();

        Render = GetComponentInChildren<SkinnedMeshRenderer>();
        OwnMaterial = Render.material;
 
        CurrentHP.Value = MaxHP;

    }

    private void Start()
    {
       
       transform.position = PlayerSpawners[Random.Range(0, PlayerSpawners.Length)].transform.position;
       Debug.Log(PlayerSpawners[Random.Range(0, PlayerSpawners.Length)].transform.position);
    }
    
        
    


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            OnHitFeedback();
            TakeDamage(other);
        }
    }

 
    public void TakeDamage(Collision collision)
    {

        CurrentHP.Value -= collision.gameObject.GetComponent<BulletBehaviour>().BulletDamage.Value;
        
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

            StartCoroutine("PlayerRespawn");
            
            //gameObject.GetComponent<NetworkObject>().Despawn();
           
            
            //GameManager.Instance.UpdateGameState(GameManager.GameState.Defeat);
        }
    }

    IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        
        yield return new WaitForSeconds(5f);
        //gameObject.transform.position = PlayerSpawners[Random.Range(0, PlayerSpawners.Length)].transform.position;
        gameObject.SetActive(true);

    }

 
}
