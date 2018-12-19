using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

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
    [Range(0, 1)]
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

    public GameObject treePrefab;


    //tree variables
    [Range(0, 5000)]
    public int numberOfTrees;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {

                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (mode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (mode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (mode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }

    }

    void OnValidate()
    {


        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }
    }

    void Awake()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        if (randomOnStart)
        {
            seed = Random.Range(-1000, 1000);
            offset = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
        }
        GenerateMap();
        pools = Object_Pool.Instance;

        //set region range
        /* for(int i = 0; i < regions.Length; i++){
			if(i == 0){
				regions[i].minRange = 0;
				regions[i].maxRange = meshHeightMultiplier * meshHeightCurve.Evaluate(regions[i].height);
			}else if(i < regions.Length){
				regions[i].minRange = meshHeightMultiplier * meshHeightCurve.Evaluate(regions[i-1].height);
				regions[i].maxRange = meshHeightCurve.Evaluate(regions[i].height) * meshHeightMultiplier;
			}
		}*/
    }

    void Start()
    {

        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);




        // Tree spawning
        Mesh mesh = GameObject.Find("Mesh").GetComponent<MeshFilter>().sharedMesh;

        int treeRegions = 0;

        for (int i = 0; i < regions.Length; i++)
        {
            if (regions[i].trees)
            {
                treeRegions++;
            }
        }






        int dividedTreeCount = numberOfTrees / treeRegions;

        for (int i = 0; i < regions.Length; i++)
        {
			if (regions[i].trees)
            {
				for (int y = 0; y < mapChunkSize; y++)
            	{
                	for (int x = 0; x < mapChunkSize; x++)
                	{
						if (i == 0)
                    	{
							if (noiseMap[x, y] < regions[i].height && noiseMap[x, y] > 0){
								float ran = Random.Range(0, 10);
								if (ran < 1 /*&& regions[i].treeList.Count < dividedTreeCount*/)
                                {
                                    GameObject treeObj = Instantiate(treePrefab, mesh.vertices[((y * mapChunkSize) + x)] * 10, Quaternion.identity, GameObject.Find("Game_Manager").transform);
                                    regions[i].treeList.Add(treeObj);
                                }
							}
						}else{
							if (noiseMap[x, y] < regions[i].height && noiseMap[x, y] > regions[i - 1].height)
                            {
								float ran = Random.Range(0, 10);
                                if (ran < 1 /*&& regions[i].treeList.Count < dividedTreeCount*/)
                                {
                                    GameObject treeObj = Instantiate(treePrefab, mesh.vertices[((y * mapChunkSize) + x)] * 10, Quaternion.identity, GameObject.Find("Game_Manager").transform);
                                    regions[i].treeList.Add(treeObj);
                                }
                            }
						}
					}
				}
			}
        }
    }
}



[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

    public bool trees;

    public float minRange;
    public float maxRange;

    public List<GameObject> treeList;
}
