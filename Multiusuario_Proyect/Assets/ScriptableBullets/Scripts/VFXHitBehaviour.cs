using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHitBehaviour : MonoBehaviour
{
    public float DestroyTime;

    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
}
