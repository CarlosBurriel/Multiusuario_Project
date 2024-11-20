using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpAndFeedback : MonoBehaviour
{
    public int MaxHP;

    [SerializeField]public int CurrentHP;

    private Collider cl;

    private UI_Manager UI;

    private PlayerAnimHandler playerAnimHandler;

    private void Start()
    {
        playerAnimHandler = GetComponentInChildren<PlayerAnimHandler>();

        CurrentHP = MaxHP;
        cl = GetComponent<Collider>();
        if (transform.childCount > 0) { UI = transform.GetChild(2).gameObject.GetComponent<UI_Manager>(); }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Damage"))
        {
            CurrentHP -= collision.gameObject.GetComponent<BulletBehaviour>().BulletDamage;
            CheckLife();
        }
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
