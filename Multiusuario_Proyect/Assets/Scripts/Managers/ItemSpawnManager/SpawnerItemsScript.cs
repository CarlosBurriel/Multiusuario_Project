using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerItemsScript : MonoBehaviour
{
    [Header("Spawner GameObjects")]
    public GameObject[] Spawners;

    [Header("Items")]
    public GameObject[] Items;

    public int RandomItem;
    public int RandomSpawner;

    void Start()
    {
        InvokeRepeating("SpawnNewItems", 5.0f, 10.0f);
    }

    void SpawnNewItems()
    {
        RandomItem = Random.Range(0, Items.Length); 
        RandomSpawner = Random.Range(0, Spawners.Length);
        

        if (Spawners[RandomSpawner].transform.childCount == 0)
        {
            Instantiate(Items[RandomItem], Spawners[RandomSpawner].transform);

        }
    }
}
