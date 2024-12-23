using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScriptableBullet : MonoBehaviour
{
    public float LaunchSpeed;

    public int BulletDamage;

    public int BulletBounces = 0;

    public bool IsPowerUp = false;

    public GameObject BulletType;

    public Material Bulletmaterial;

    public PhysicMaterial PhysicMaterial;

    public GameObject VFX;

}
