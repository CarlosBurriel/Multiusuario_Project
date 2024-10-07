using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBullet : MonoBehaviour
{
    public bool rebotepared;
    public Rigidbody Proyectil;
    Vector3 p_dir;
    public float rebotespeed;
    public int numeroRebotes=3;
    public bool tienepoder;



    public DisparoPlayer poder;

    public void Start()
    {
        if (tienepoder == false)
        {
            rebotepared = false;
        }
    }
    private void OnCollisionEnter(Collision other)
    {



          if( other.gameObject.tag == "Jugador")
          {
            tienepoder = false;
            Destroy(this.gameObject);
          }


        if (other.gameObject.tag == "Wall")
        {
            numeroRebotes -= 1;

            if (rebotepared == true)
            {
                Vector3 _wallnormal = other.contacts[0].normal;
                p_dir = Vector3.Reflect(Proyectil.velocity, _wallnormal).normalized;

                Proyectil.velocity = p_dir * rebotespeed;
                if (numeroRebotes <= 0)
                {
                    tienepoder = false;
                    rebotepared = false;
                    
                    numeroRebotes = 3;
                }
            }
            else
            {
             tienepoder = false;
             Destroy(this.gameObject);

            }
        }
        
    }
}
