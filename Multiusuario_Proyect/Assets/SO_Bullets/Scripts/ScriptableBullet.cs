using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "CommonBullet", menuName = "BulletScriptable")]
public class ScriptableBullet : ScriptableObject
{
    public float LaunchSpeed;

    public int BulletDamage;

    public int BulletBounces = 0;

    public bool IsPowerUp = false;

    public GameObject BulletType;

    public NetworkVariable<Material> BulletMaterial = new NetworkVariable<Material>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public PhysicMaterial PhysicMaterial;

    public GameObject VFX;


}
