using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator {
    public static Texture2D TextureFromColorMap(Color[] colorMap, int size) {
        Texture2D texture = new Texture2D(size, size);
        // create a new texture
        texture.filterMode = FilterMode.Point;
        // point, as opposed to bilinear, filtering
        texture.wrapMode = TextureWrapMode.Clamp;
        // clamp, as opposed to wrap, wrap mode
        texture.SetPixels(colorMap);
        // set the pixels of the texture
        texture.Apply();
        // apply it
        return texture;
        // return it
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap) {
        int size = heightMap.GetLength(0);
        // get the width of the map based on the dimensions of the float[,]
        Color[] colorMap = new Color[size * size];
        // create a new one-dimensional array of colors from the width and height
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                colorMap[y * size + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                // at the given index, get the color (from black to white) based on the noisemap point at (x,y)
            }
        }
        return TextureFromColorMap(colorMap, size);
    }
}
