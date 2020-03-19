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
}
