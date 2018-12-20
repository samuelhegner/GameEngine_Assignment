using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator{

    //sets a colour map texture using the colour map from the map generator and the proposed width and height
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height){
        //creates texture of the needed size
        Texture2D texture = new Texture2D(width, height);
        //avoids blurryness
        texture.filterMode = FilterMode.Point;
        //stops texture showing on wrong side
        texture.wrapMode = TextureWrapMode.Clamp;
        //sets the pixels according to the colour map
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    //set a grayscale noise map from the noise map created in the noise script
    public static Texture2D TextureFromHeightMap(float[,] heightMap){
        int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);
        //creates a colourmap based on the noisemap array length
		Color[] colourMap = new Color[width*height];

		for(int y = 0; y < height; y++){
			for(int x = 0; x < width; x++){
                //each pixel is lerped to a grayscale value between black at 0 and white at 1
				colourMap[y*width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
			}
		}

        return TextureFromColourMap(colourMap, width, height);
    }
}
