using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class HpAndFeedback : NetworkBehaviour
{
    public int MaxHP;
    [SerializeField]public NetworkVariable<int> CurrentHP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Material FlashMaterial;

    private bool IsSizeChanging = false;
    private int TempDamage;

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
            TempDamage = other.gameObject.GetComponent<BulletBehaviour>().BulletDamage.Value;
            TakeDamageServerRPC();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            TempDamage = other.gameObject.GetComponent<BulletBehaviour>().BulletDamage.Value;
            TakeDamageServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRPC()
    {
        
            CurrentHP.Value -= TempDamage;
            ChangeSizeClientRPC();
            ChangeColorClientRPC();
            
        
    }

    #region Feedback
    [ClientRpc(RequireOwnership = false)]
    public void ChangeSizeClientRPC()
    {
        if (!IsSizeChanging)
        {
            IsSizeChanging = true;
            transform.DOPunchScale(transform.localScale * Random.Range(1, 1.3f), 0.2f, 10, 0).onComplete  = SizeFix;
            
        }
        
    }

    public void SizeFix()
    {
        
        IsSizeChanging = false;
        transform.DOPunchScale(Vector3.one, 0.1f);
    }

    [ClientRpc(RequireOwnership = false)]
    void ChangeColorClientRPC()
    {
        Render.material = FlashMaterial;
        StartCoroutine(Flash());
    }
    public IEnumerator Flash()
    {
        yield return new WaitForSeconds(0.2f);
        Render.material = OwnMaterial;
    }
    #endregion

    [ServerRpc(RequireOwnership = false)]
    public void CheckLifeServerRPC(int previousValue, int newValue)
    {
        if (newValue <= 0)
        {
            playerMove.enabled = false;
            playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.DEATH);
            StartCoroutine(PlayerRespawn());
        }
    }

    #region Respawn Hell
    IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(1f);
        DespawnLogicClientRPC();
       
        yield return new WaitForSeconds(2f);
       
        RespawnLogicClientRPC();

    }

   

    [ClientRpc(RequireOwnership = false)]
    public void RespawnLogicClientRPC()
    {
        transform.localScale = Vector3.one;
        transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position;
        playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.IDLE);
        playerMove.enabled = true;
        Render.gameObject.SetActive(true);
        playerShoot.Ammo.Value = playerShoot.MaxAmmo;
        CurrentHP.Value = MaxHP;
        col.enabled = true;
    }

  

    [ClientRpc(RequireOwnership = false)]
    public void DespawnLogicClientRPC()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        Render.gameObject.SetActive(false);
        col.enabled = false;
    }
    #endregion

    private void OnEnable()
    {
        transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position;
    }


}
