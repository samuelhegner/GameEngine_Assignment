using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_Night : MonoBehaviour
{

    GameObject mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GameObject.Find("Mesh");
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(mesh.transform.position, Vector3.forward, 10f * Time.deltaTime);
        transform.LookAt(mesh.transform.position);
    }
}
