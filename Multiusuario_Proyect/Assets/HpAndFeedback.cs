using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpAndFeedback : MonoBehaviour
{
    public int MaxHP;


    private int CurrentHP;

    private Collider cl;


    private void Start()
    {
        CurrentHP = MaxHP;
        cl = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (cl.tag == "Damage")
        {
            print(2);
            CurrentHP -= cl.GetComponent<BulletBehaviour>().BulletDamage;
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
