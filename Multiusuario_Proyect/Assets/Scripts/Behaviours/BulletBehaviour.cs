using Unity.Netcode;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

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

    [HideInInspector]public PHPHandler PHPBullet;

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
            if (other.gameObject.GetComponent<HpAndFeedback>().CurrentHP.Value - BulletDamage.Value <= 0)
            {
                PHPBullet.Kills.Value++;
                other.gameObject.GetComponent<HpAndFeedback>().lastkillername = PHPBullet.PlayerUsername.Value.ToString();
                StartCoroutine(KillsCoroutines());
            }
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
        StartCoroutine(DespawnTimer());
        
        
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

    IEnumerator DespawnTimer()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<NetworkObject>().Despawn();
    }
    IEnumerator KillsCoroutines()
    {

        WWWForm form = new WWWForm();
        form.AddField("username", PasableUsername.instance.username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/Kills.php", form))
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
