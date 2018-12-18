using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pool : MonoBehaviour
{

    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;

    public static Object_Pool Instance;

    void Awake(){
        Instance = this;
    }
    
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

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation){
        
        if(!poolDictionary.ContainsKey(tag)){
            Debug.LogWarning("Doesn't exist");
            return null;
        }


        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);

        return obj;
    }

    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }
}
