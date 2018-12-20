using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {

	//function to create a 2d noise map used to assign heights and colours later
	public static float [,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
		//creates a 2d float array to hold all of the noise values
		float [,] noiseMap = new float[mapWidth, mapHeight];

		//creates a random based on the seed provided in the map generation class, this allows the seed random to be the same (pseudo random)
		System.Random prng = new System.Random(seed);
		//Creates an array of Vector to offset the sample x and sample y coordinates
		Vector2[] octavesOffsets = new Vector2[octaves];

		//sets the random offsets for each octaveOffsets vector
		for (int i = 0; i < octaves; i ++){
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			octavesOffsets[i] = new Vector2(offsetX, offsetY);
		}

		//avoids divide by 0 error
		if(scale <= 0){
			scale = 0.000001f;
		}

		//variables to keep track of the highest and lowest noise values
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		//half width and height variables used to scale the noise from the middle and not the top left
		float halfWidth = mapWidth/2f;
		float halfHeight = mapHeight/2f;

		for(int y = 0; y < mapHeight; y++){
			for(int x = 0; x<mapWidth; x++){
				
				//base values for amp, freq and noiseHeight
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;


				for(int i = 0; i< octaves; i++){
					
					//the place where the noise will be sampled from
					float sampleX = (x - halfWidth)/scale * frequency + octavesOffsets[i].x;
					float sampleY = (y - halfHeight)/scale * frequency + octavesOffsets[i].y;
					
					//put perlin value in a range of -1 to 1 to get more interesting effects
					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseMap[x, y] = perlinValue;
					noiseHeight += perlinValue * amplitude;

					//updates the amp and freq for the next octave base on the user set lacunarity and persistance
					amplitude *= persistance;
					frequency *= lacunarity;
				}

				//updates the min and max current noise value if a new record shows
				if(noiseHeight > maxNoiseHeight){
					maxNoiseHeight = noiseHeight;
				}else if(noiseHeight < minNoiseHeight){
					minNoiseHeight = noiseHeight;
				}
				//sets the noiseMap value at x, y to the total calculated noiseheight
				noiseMap[x, y] = noiseHeight;
			}
		}

		for(int y = 0; y < mapHeight; y++){
			for(int x = 0; x<mapWidth; x++){
					//brings all of the floats back into a 0 to 1 range so they can be used
					noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
				}
			}

		return noiseMap;
	}
}
