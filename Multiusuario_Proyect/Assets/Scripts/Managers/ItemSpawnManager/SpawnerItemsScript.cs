using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerItemsScript : MonoBehaviour
{
    [Header("Spawner GameObjects")]
    public Transform[] Spawners;

    [Header("Items")]
    public GameObject[] Items;

    public int RandomNum;
    public int RandomSpawner;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnNewItems", 1.0f, 10.0f);
    }

    void SpawnNewItems()
    {
        RandomNum = Random.Range(0, Items.Length); 
        RandomSpawner = Random.Range(0, Spawners.Length);

        Instantiate(Items[RandomNum], new Vector3(Spawners[RandomSpawner].position.x, Spawners[RandomSpawner].position.y, Spawners[RandomSpawner].position.z), Quaternion.identity); 

        //FALTA NO DEJAR QUE APAREZCAN NUEVOS OBJETOS EN POSICIONES QUE YA TIENEN O ELIMINAR EL ANTERIOR
        

    }
}
