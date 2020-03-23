using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public float noiseScale;
    public int cloudSize;
    public int octaves = 7;
    public int seed;
    private Vector2 origin = new Vector2(0.5f, 0.5f);
    [Range(0, 1)] public float persistence;
    public GameObject cloudPrefab;
    public float lacunarity;
    // the factor to increase the frequency by every octave
    public bool autoUpdate;
    public Vector2 center;

    public void GenerateCloudAt(Vector2 center) {  
        float[,] noiseMap = Noise.GenerateNoiseMap(cloudSize, seed, noiseScale, octaves, persistence, lacunarity, new Vector2(center.x, center.y));
        // generate the noise map with the given variables 
        Color[] colorMap = new Color[cloudSize * cloudSize];
        // make a new colormap to apply colors to
        for(int y = 0; y < cloudSize; y++) {
            for (int x = 0; x < cloudSize; x++) {
                float currentHeight = noiseMap[x, y];
                if(currentHeight > .2) {
                    colorMap[y * cloudSize + x] = Color.Lerp(Color.grey, Color.white, currentHeight*1.3f);
                  
                }
            }
        }

        Texture2D texture = new Texture2D(cloudSize, cloudSize);
        // create a new texture
        texture.filterMode = FilterMode.Point;
        // point (as opposed to bilinear) filtering
        texture.wrapMode = TextureWrapMode.Clamp;
        // clamp (as opposed to wrap) wrap mode
        texture.SetPixels(colorMap);
        // set the pixels of the texture
        texture.Apply();
        // apply it
        cloudPrefab.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, cloudSize, cloudSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        // create a sprite from the chunk
        //prefab.GetComponent<Transform>().SetParent(parent.transform);
        Color temp = cloudPrefab.GetComponent<SpriteRenderer>().color;
        temp.a = 0.3f;
        cloudPrefab.GetComponent<SpriteRenderer>().color = temp;
        cloudPrefab.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void Update() {
        GenerateCloudAt(center);
    }
}
