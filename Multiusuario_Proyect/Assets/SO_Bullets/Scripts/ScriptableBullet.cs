using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommonBullet", menuName = "BulletScriptable")]
public class ScriptableBullet : ScriptableObject
{
    public float LaunchSpeed;

    public int BulletDamage;

    public int BulletBounces = 0;

    public bool IsPowerUp = false;

    public GameObject BulletType;

    public Material BulletMaterial;

    public PhysicMaterial PhysicMaterial;

    public GameObject VFX;


}
