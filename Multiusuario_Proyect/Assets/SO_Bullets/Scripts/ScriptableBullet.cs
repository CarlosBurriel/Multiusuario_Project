using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static ShootBehaviour;

[CreateAssetMenu(fileName = "CommonBullet", menuName = "BulletScriptable")]
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

    public struct BMaterial : INetworkSerializable
    {
        public Color bullmat;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter { serializer.SerializeValue(ref bullmat); }
    }
}
