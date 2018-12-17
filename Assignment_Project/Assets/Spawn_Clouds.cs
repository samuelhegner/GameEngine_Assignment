using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Clouds : MonoBehaviour
{   
    public GameObject prefab;

    public float spawnRate;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCloud());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCloud(){
        while(true){
            Vector3 spawnPoint = new Vector3(Random.Range(-1000, 1000), 0, 0);

            Vector3 localSpawn = transform.TransformPoint(spawnPoint);

            Instantiate(prefab, localSpawn, transform.rotation, transform);
            yield return new WaitForSeconds(spawnRate);
        }

    }
}
