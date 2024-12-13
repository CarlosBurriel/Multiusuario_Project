using DG.Tweening;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpAndFeedback : NetworkBehaviour
{
    public int MaxHP;
    [SerializeField]public NetworkVariable<int> CurrentHP = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Material FlashMaterial;

    private SkinnedMeshRenderer Render;
    private Material OwnMaterial;

    public GameObject UI;

    private PlayerAnimHandler playerAnimHandler;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) { UI.SetActive(true); }

        base.OnNetworkSpawn();
        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();

        Render = GetComponentInChildren<SkinnedMeshRenderer>();
        OwnMaterial = Render.material;

        CurrentHP.Value = MaxHP;

    }


    public void OnCollisionEnter(Collision collision)
    {
        if(!IsServer) return;
        if (collision.gameObject.CompareTag("Damage") && GetComponent<NetworkObject>().OwnerClientId != collision.gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            OnHitFeedback();
            CurrentHP.Value -= collision.gameObject.GetComponent<BulletBehaviour>().BulletDamage;
        }
    }

    public void OnHitFeedback()
    {
        SizeChange();
        StartCoroutine(Flash());
    }

    public void SizeChange()
    {
        transform.DOPunchScale(transform.localScale * Random.Range(1, 1.5f), 0.5f);
    }

    public IEnumerator Flash()
    {
        Render.material = FlashMaterial;   
        yield return new WaitForSeconds(0.5f);
        Render.material = OwnMaterial;
    }

    void CheckLife()
    {

        if (CurrentHP.Value <= 0)
        {
            playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.DEATH);
            GetComponent<PlayerSmovement>().enabled = false;
           //Destroy(gameObject);
           //GameManager.Instance.UpdateGameState(GameManager.GameState.Defeat);
        }
    }

 
}
