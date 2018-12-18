using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Clouds : MonoBehaviour
{   
    public GameObject prefab;

    public Object_Pool pools;

    public float spawnRate;

    void Start()
    {
        pools = Object_Pool.Instance;
        StartCoroutine(SpawnCloud());
    }

    IEnumerator SpawnCloud(){
        while(true){
            Vector3 spawnPoint = new Vector3(Random.Range(-1000, 1000), 0, 0);

            Vector3 localSpawn = transform.TransformPoint(spawnPoint);

            pools.SpawnFromPool("Cloud", localSpawn, transform.rotation);
            yield return new WaitForSeconds(spawnRate);
        }

    }
}
