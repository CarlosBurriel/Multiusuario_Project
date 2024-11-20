using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpAndFeedback : MonoBehaviour
{
    public int MaxHP;

    [SerializeField]public int CurrentHP;

    public Material FlashMaterial;

    private SkinnedMeshRenderer Render;
    private Material OwnMaterial;

    private Collider cl;

    private UI_Manager UI;

    private PlayerAnimHandler playerAnimHandler;

    private void Start()
    {
        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();

        Render = GetComponentInChildren<SkinnedMeshRenderer>();
        OwnMaterial = Render.material;

        CurrentHP = MaxHP;
        cl = GetComponent<Collider>();
        if (transform.childCount > 0) { UI = transform.GetChild(2).gameObject.GetComponent<UI_Manager>(); }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Damage"))
        {
            OnHitFeedback();
            CurrentHP -= collision.gameObject.GetComponent<BulletBehaviour>().BulletDamage;
            CheckLife();
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
        UI.UpdateHpUI();
        if (CurrentHP <= 0)
        {
            playerAnimHandler.UpdateState(PlayerAnimHandler.PlayerState.DEATH);
           //Destroy(gameObject);
           GameManager.Instance.UpdateGameState(GameManager.GameState.Defeat);
        }
    }
}
