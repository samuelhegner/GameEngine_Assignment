using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode
	{
		NoiseMap,
		ColourMap,
		Mesh
	};

	const int mapChunkSize = 241;

	public DrawMode mode;

	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;

	public float meshHeightMultiplier;
	public Vector2 offset;

	public bool autoUpdate;
	public bool randomOnStart;

	public AnimationCurve meshHeightCurve;

	public TerrainType[] regions;

	Object_Pool pools;


	//tree variables
	[Range(0, 300)]
	public float numberOfTrees;

	public void GenerateMap(){
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

		for(int y = 0; y < mapChunkSize; y++){
			for(int x = 0; x < mapChunkSize; x++){
				float currentHeight = noiseMap[x,y];

				for(int i = 0; i < regions.Length; i ++){

					if(currentHeight <= regions[i].height){
						colourMap[y *mapChunkSize +x] = regions [i].colour;
						break;
					}
				}
			}
		}
		MapDisplay display = FindObjectOfType<MapDisplay>();
		if(mode == DrawMode.NoiseMap){
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
		}else if (mode == DrawMode.ColourMap){
			display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		}else if(mode == DrawMode.Mesh){
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		}
		
	}

	void OnValidate(){
		

		if(lacunarity < 1){
			lacunarity = 1;
		}

		if(octaves < 0){
			octaves = 0;
		}
	}

	void Awake(){
		if(randomOnStart){
			seed = Random.Range(-1000, 1000);
			offset = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
		}
		GenerateMap();
		pools = Object_Pool.Instance;
	}

	void Start(){


		// Tree spawning saved for a later date

		Mesh mesh = GameObject.Find("Mesh").GetComponent<MeshFilter>().sharedMesh;
		pools.SpawnFromPool("Tree", mesh.vertices[0] * 10f, Quaternion.identity);


		//float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		//Mesh mesh = GameObject.Find("Mesh").GetComponent<MeshFilter>().mesh;

		//for(int i = 0; i < mesh.vertices.Length; i ++){
		//	float vertexHeight = mesh.vertices[i].y;
			
		//	pools.SpawnFromPool("Tree", mesh.vertices[i], Quaternion.identity);
		//}
	}
}



[System.Serializable]
public struct TerrainType
{
	public string name;
	public float height;
	public Color colour;

	public bool trees;
}
