using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_Night : MonoBehaviour
{
    //The mesh the sun and moon look at
    GameObject mesh;

    void Start()
    {
        mesh = GameObject.Find("Mesh");
    }


    void Update()
    {
        //This rotates the sun and moon around the mesh
        transform.RotateAround(mesh.transform.position, Vector3.forward, 10f * Time.deltaTime);
        //this keeps the sun and moon directional light looking at the mesh
        transform.LookAt(mesh.transform.position);
    }
}
