using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpAndFeedback : MonoBehaviour
{
    public int MaxHP;


    [SerializeField]private int CurrentHP;

    private Collider cl;


    private void Start()
    {
        CurrentHP = MaxHP;
        cl = GetComponent<Collider>();
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
        if (CurrentHP <= 0)
        {
            //Destroy(gameObject);
            SceneManager.LoadScene("Loser");
        }
    }
}
