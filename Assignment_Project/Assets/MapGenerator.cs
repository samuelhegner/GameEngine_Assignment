﻿using System.Collections;
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
	}
}
[System.Serializable]
public struct TerrainType
{
	public string name;
	public float height;
	public Color colour;
}
