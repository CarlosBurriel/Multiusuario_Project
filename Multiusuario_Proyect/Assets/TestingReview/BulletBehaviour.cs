using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [HideInInspector]public int BulletLife;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            BulletLife--;
            CheckLife();
        }

        
    }

    void CheckLife()
    {
        if (BulletLife <= 0)
        {
            Destroy(gameObject);
        }
    }

}
