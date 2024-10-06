using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public int vidas = 3;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Proyectil")
        {
            vidas -= 1;
        }
    }

    void Update() {


        if (vidas <= 0)
        {
            Destroy(this.gameObject);
        }

        
    }
}
