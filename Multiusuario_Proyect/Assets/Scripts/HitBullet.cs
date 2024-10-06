using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
       


        if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Jugador")
        {
            Destroy(this.gameObject);

        }

    }
}
