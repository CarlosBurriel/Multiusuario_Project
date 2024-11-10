using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyScript : MonoBehaviour
{
    public int MaxHP;


    [SerializeField] public int CurrentHP;

   


    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            CurrentHP -= collision.gameObject.GetComponent<BulletBehaviour>().BulletDamage;
            CheckLife();
        }
        if (collision.gameObject.CompareTag("Explosion"))
        {
            CurrentHP--;
            CheckLife();
        }
    }

    void CheckLife()
    {
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
            
        }
    }
}
