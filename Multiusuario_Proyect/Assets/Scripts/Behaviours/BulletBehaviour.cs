using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [HideInInspector] public int BulletLife;

    [HideInInspector] public int BulletDamage = 1;

    [HideInInspector] public GameObject VFX;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            BulletLife--;
            CheckLife();
        }

        
    }

    void CheckLife()
    {
        if (BulletLife < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (VFX) { Instantiate(VFX, transform.position, Quaternion.identity); }
    }
}
