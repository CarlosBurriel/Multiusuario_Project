using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBullet : MonoBehaviour
{
    public bool rebotepared;
    public Rigidbody Proyectil;
    Vector3 p_dir;
    public float rebotespeed;

    private void OnCollisionEnter(Collision other)
    {
       if( other.gameObject.tag == "Jugador")
       {
            Destroy(this.gameObject);
       }


        if (other.gameObject.tag == "Wall")
        {

            if (rebotepared == true)
            {
                Vector3 _wallnormal = other.contacts[0].normal;
                p_dir = Vector3.Reflect(Proyectil.velocity, _wallnormal).normalized;

                Proyectil.velocity = p_dir * rebotespeed;
            }
            else
            {
             Destroy(this.gameObject);

            }
        }
        
    }
}
