using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Clouds : MonoBehaviour
{   
    Object_Pool pools;

    public float spawnRate;
    [Range(100, 200)]
    public float snowFallLine;

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
