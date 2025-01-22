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
using UnityEngine.Networking;

public class HpAndFeedback : NetworkBehaviour
{
    public int MaxHP;
    [SerializeField]public NetworkVariable<int> CurrentHP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Material FlashMaterial;

    private bool IsSizeChanging = false;
    private int TempDamage;
    private bool IsRespawning;

    private SkinnedMeshRenderer Render;
    private Material OwnMaterial;
    private Collider col;
    private Rigidbody rb;

    public GameObject UI;

    private PlayerAnimHandler playerAnimHandler;
    private ShootBehaviour playerShoot;
    private PlayerSmovement playerMove;
    private PHPHandler PHP;

    public List<GameObject> SpawnPoints = new List<GameObject>();

    [HideInInspector] public string lastkillername;
    

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
        if (IsOwner) { UI.SetActive(true); StartCoroutine(GamesPlayedCoroutines()); }

        base.OnNetworkSpawn();
        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();
        playerShoot = GetComponent<ShootBehaviour>();
        playerMove = GetComponent<PlayerSmovement>();
        PHP = GetComponent<PHPHandler>();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        Render = GetComponentInChildren<SkinnedMeshRenderer>();
        OwnMaterial = Render.material;
 
        CurrentHP.Value = MaxHP;
        PHP.Deaths.Value = 0;
        PHP.Kills.Value = 0;

        
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
            if (!IsRespawning)
            {
                StartCoroutine(PlayerRespawn());

            }
        }
    }

    #region Respawn Hell
    [ServerRpc(RequireOwnership = false)]
    public void upDeathsServerRPC()
    {
        PHP.Deaths.Value++;
    }
    IEnumerator PlayerRespawn()
    {
        IsRespawning = true;
        upDeathsServerRPC();
        GameManager.Instance.total_deaths.Value++;
        StartCoroutine(DeathsCoroutines());
        StartCoroutine(DeathTablesCoroutines());

        yield return new WaitForSeconds(1f);
        DespawnLogicClientRPC();
       
        yield return new WaitForSeconds(2f);
       
        RespawnLogicClientRPC();
        IsRespawning = false;


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

    IEnumerator DeathsCoroutines()
    {

        WWWForm form = new WWWForm();
        form.AddField("username", PasableUsername.instance.username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/Deaths.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                print(responseText);

            }
        }


    }
    IEnumerator GamesPlayedCoroutines()
    {

        WWWForm form = new WWWForm();
        form.AddField("username", PasableUsername.instance.username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/GamesPlayed.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                print(responseText);

            }
        }


    }
    IEnumerator DeathTablesCoroutines()
    {

        WWWForm form = new WWWForm();
        
        form.AddField("gameid", GameManager.Instance.CurrentGameID);
        form.AddField("deathpositionx", transform.position.x.ToString());
        form.AddField("deathpositionz", transform.position.z.ToString());
        form.AddField("killerid", lastkillername);
        form.AddField("victimid", PHP.PlayerUsername.Value.ToString());
        form.AddField("timeofdeath", GameManager.Instance.CurrentTime);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/DeathTable.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                print(responseText);

            }
        }


    }
    public IEnumerator GamesHistoryCoroutines()
    {
        if (!IsOwner && !IsClient) { yield return null; }
        WWWForm form = new WWWForm();
        form.AddField("gameid", GameManager.Instance.CurrentGameID);
        form.AddField("username", PHP.PlayerUsername.Value.ToString());
        form.AddField("deaths", PHP.Deaths.Value);
        form.AddField("kills", PHP.Kills.Value);
        form.AddField("ammo", gameObject.GetComponent<ShootBehaviour>().AmmoGathered);
        form.AddField("powerups", gameObject.GetComponent<ShootBehaviour>().PowerUpsGathered);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/GamesHistory.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                print(responseText);
               


            }
           
            
        }


    }

}
