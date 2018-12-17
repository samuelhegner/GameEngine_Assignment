﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Pool : MonoBehaviour
{

    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;
    
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++){
                GameObject obj = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }
}
