using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerItemsScript : NetworkBehaviour
{
    public static SpawnerItemsScript Instance;

    [Header("Spawner GameObjects")]
    public GameObject[] Spawners;

    [Header("Items")]
    public GameObject[] Items;

    public int RandomItem;
    public int RandomSpawner;

    public override void OnNetworkSpawn()
    {
        

        if (!IsServer)
        {
            enabled = false; return;
        }
        
    }

    private void Awake()
    {
        Instance = this;
    }

    public void StartItemSpawner()
    {
        InvokeRepeating("SpawnNewItems", 5.0f, 10.0f);
    }
    void SpawnNewItems()
    {
        RandomItem = Random.Range(0, Items.Length); 
        RandomSpawner = Random.Range(0, Spawners.Length);
        

        if (Spawners[RandomSpawner].transform.childCount == 0)
        {
            GameObject SpawnedGameObject = Instantiate(Items[RandomItem], Spawners[RandomSpawner].transform);
            SpawnedGameObject.GetComponent<NetworkObject>().Spawn(true);
            SpawnedGameObject.transform.SetParent(Spawners[RandomSpawner].transform); //ERROR AL HACER HIJOS A LOS OBJETOS

        }
    }
}
