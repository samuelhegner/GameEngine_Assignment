using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that can be used to generate all of the data needed for a mesh
public static class MeshGenerator
{
    //function that creates mesh data that can be applied to mesh in the Mesh Data class
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve){
        
        //gets the width and length of the mesh data based on the 2d heightmap array
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        
        //variables created to allow for the mesh to be centered
        float topLeftX = (width -1)/ -2f;
        float topLeftZ = (height -1)/ 2f;


        //the local meshdata variable
        MeshData meshData = new MeshData(width, height);
        
        
        //keeps track of the current vertext index
        int vertexIndex = 0;

        for(int y = 0; y < height; y ++){
            for(int x = 0; x < width; x++){
                //works out the vertex position based on the x and z position, sets the y based on the curve and the multiplier
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, topLeftZ - y);
                //sets all of the uvs for the texture
                meshData.uvs[vertexIndex] = new Vector2(x/(float)width, y/(float)height);

                if(x < width-1 && y < height -1){
                    //creates the two triangles for the two squares
                    meshData.AddTriangle(vertexIndex, vertexIndex+width+1, vertexIndex+width);
                    meshData.AddTriangle(vertexIndex+width+1, vertexIndex, vertexIndex+1);
                }
                //increments the vertex count by one
                vertexIndex ++;
            }
        }
        //returns the meshdata
        return meshData;
    }
}


//class that contains all the information to create a meshdata object
public class MeshData{
    //the vertices of the mesh
    public Vector3[] vertices;
    //the triangles of the mesh
    public int[] triangles;
    //the uvs of the mesh
    public Vector2[] uvs;
    //int used to keep track of which triangle is being worked on
    int triangleIndex;

    //constructor of a mesh data obj that set all of its start values based on the given width and height
    public MeshData(int meshWidth, int meshHeight){
        //works out how many vertices are needed
        vertices = new Vector3[meshWidth * meshHeight];
        //works out how many uvs are needed
        uvs = new Vector2[meshWidth * meshHeight];
        //works out how many triangles are needed
        triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
    }

    //function that creates a trianlge based on vertex index numbers
    public void AddTriangle(int a, int b, int c){
        triangles[triangleIndex] = a;
        triangles[triangleIndex +1] = b;
        triangles[triangleIndex +2] = c;
        triangleIndex += 3;
    }

    //function to apply Mesh Data information to a mesh and return it
    public Mesh CreateMesh(){
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
