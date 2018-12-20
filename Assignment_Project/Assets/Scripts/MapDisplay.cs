using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

	//the 2d texture renderer on the plane
	public Renderer textureRenderer;
	//the meshfilter of the mesh
	public MeshFilter meshFilter;
	//the mesh renderer of the mesh
	public MeshRenderer meshRenderer;
	//the mesh collider of the mesh
	public MeshCollider meshCollider;

	//function that draws the texture onto the mesh
	public void DrawTexture(Texture2D texture){
		//draws texture onto plane and sets its scale to fit the texture size
		textureRenderer.sharedMaterial.mainTexture = texture;
		textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
	}

	//function that draws and creates the mesh from the mesh data class
	public void DrawMesh(MeshData meshData, Texture2D texture){
		//updates the mesh, the collider and the material
		meshFilter.sharedMesh = meshData.CreateMesh();
		meshCollider.sharedMesh = meshData.CreateMesh();
		meshRenderer.sharedMaterial.mainTexture = texture;
	}
}
