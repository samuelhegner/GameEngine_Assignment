using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //enum that holds the modes (create 2d noisemap, 2d colour map, 3d mesh)
    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh
    };

    //the biggest possible map size to avoid overcapping on vertices
    const int mapChunkSize = 241;

    //the current mode the component is in
    public DrawMode mode;
    //the value that zooms in and out of the sampled noise map
    public float noiseScale;
    //how many ocatves of noise you want
    public int octaves;
    //how much the higher detail octaves impact the landscape
    [Range(0, 1)]
    public float persistance;
    //The frequency of the higher octaves. Essentially how much detail the octaves have
    public float lacunarity;
    //The random seed
    public int seed;
    //how much the y of the mesh is multiplied by
    public float meshHeightMultiplier;
    //A Vector two to offset the seed even further
    public Vector2 offset;
    //updates on changes made
    public bool autoUpdate;
    //randomly set seed and offset on start
    public bool randomOnStart;
    // The curve that allows customisation of the height multiplication. The curve effects the multiplication. at 0 the multiplication is negated at 1 the multiplication is full
    public AnimationCurve meshHeightCurve;
    //Array of the region structs            
    public TerrainType[] regions;
    //to access the object pools
    Object_Pool pools;
    //the tree prefab
    public GameObject treePrefab;


    //tree variables of howmany tree are going to be created
    [Range(0, 5000)]
    public int numberOfTrees;

    //function that creates the map
    public void GenerateMap()
    {
        //creates a 2d array of noise values which then can be applied to 2d plane or mesh
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        //an array of colours to colour the 2d plane or mesh
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        //this loop creates the colours based on the region data
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                //grabs the float of the noise at the x y point
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {

                    if (currentHeight <= regions[i].height)
                    {
                        //sets the colour array at the specific pixel coordinate based on the regions colour selected in editor
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        //Checks which mode the component is in
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (mode == DrawMode.NoiseMap)
        {
            //draws a 2d noise map on the plane on Generate()
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (mode == DrawMode.ColourMap)
        {
            //draws a 2d colour map onto the plane on Generate()
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (mode == DrawMode.Mesh)
        {
            //draws the mesh and the texture onto the mesh on Generate()
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }

    }

    //gets called everytime a change is made in editor is executes this
    void OnValidate()
    {
        //clamps the lacunarity
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        //clamps the octaves
        if (octaves < 0)
        {
            octaves = 0;
        }
    }

    void Awake()
    {
        //Randomises the seed and offset at runtime
        if (randomOnStart)
        {
            seed = Random.Range(-1000, 1000);
            offset = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
        }
        GenerateMap();
        
        //grabs the singleton instance
        pools = Object_Pool.Instance;
    }

    void Start()
    {
        //gets the noisemap at the current values, this is used later to check regions in tree placement
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);




        // Tree spawning
        Mesh mesh = GameObject.Find("Mesh").GetComponent<MeshFilter>().sharedMesh;

        //how many regions need trees
        int treeRegions = 0;

        //checks each region if it needs trees
        for (int i = 0; i < regions.Length; i++)
        {
            if (regions[i].trees)
            {
                treeRegions++;
            }
        }





        //how many trees each region needs based on the total number of trees available
        int dividedTreeCount = numberOfTrees / treeRegions;


        //loops through all regions
        for (int i = 0; i < regions.Length; i++)
        {   
            //if that region has trees 
            if (regions[i].trees)
            {
                //while this region doesnt have all the trees it loops through all vertices, checks if that vertex is in the currentregion
                //and has a chance of placing a tree on it
                //it does this untill all trees are placed
                while (regions[i].treeList.Count < dividedTreeCount)
                {
                    for (int y = 0; y < mapChunkSize; y++)
                    {
                        for (int x = 0; x < mapChunkSize; x++)
                        {
                            if (i == 0)
                            {
                                if (noiseMap[x, y] < regions[i].height && noiseMap[x, y] > 0)
                                {
                                    float ran = Random.Range(0, 1000);
                                    if (ran < 1 && regions[i].treeList.Count < dividedTreeCount)
                                    {
                                        //if the random number is below 1 it spawns a tree on that vertex and adds it to the region list
                                        GameObject treeObj = Instantiate(treePrefab, mesh.vertices[((y * mapChunkSize) + x)] * 10, Quaternion.identity, GameObject.Find("Game_Manager").transform);
                                        regions[i].treeList.Add(treeObj);
                                    }
                                }
                            }
                            else
                            {
                                if (noiseMap[x, y] < regions[i].height && noiseMap[x, y] > regions[i - 1].height)
                                {
                                    //if the random number is below 1 it spawns a tree on that vertex and adds it to the region list
                                    float ran = Random.Range(0, 1000);
                                    if (ran < 1 && regions[i].treeList.Count < dividedTreeCount)
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
}


//the struct for the region
[System.Serializable]
public struct TerrainType
{
    //the name of the region
    public string name;
    //the cutoff height for that region
    public float height;
    //the colour of that region
    public Color colour;
    //if the region should have trees or not
    public bool trees;
    //list of trees to check count in the region
    public List<GameObject> treeList;
}
