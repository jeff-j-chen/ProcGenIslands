using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
    public int mapSize;
    private int seed;
    public int mapScale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    private Vector2 origin = new Vector2(0.5f, 0.5f);
    private ChunkGenerator chunkGenerator;
    private Rect mapRect;
    private Player player;
    public float hueFrequency;
    public float hueStrength;

    private void Start() {
        chunkGenerator = FindObjectOfType<ChunkGenerator>();
        player = FindObjectOfType<Player>();
        mapRect = new Rect(0, 0, mapSize, mapSize);
        seed = chunkGenerator.seed;
    }

    private void FixedUpdate() {
        Vector2 center = new Vector2(Mathf.RoundToInt(player.transform.position.x * (mapScale / chunkGenerator.noiseScale)), Mathf.Round(player.transform.position.y * (mapScale / chunkGenerator.noiseScale)));
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, seed, mapScale, octaves, persistence, lacunarity, center);
        float[,] hueMap = Noise.GenerateHueMap(mapSize, seed, mapScale, hueFrequency, center);
        // apply hue map
        Color[] colorMap = new Color[mapSize * mapSize];
        float H, S, V;
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                for (int i = 0; i < chunkGenerator.regions.Length; i++) {
                    // for every region
                    if (noiseMap[x, y] <= chunkGenerator.regions[i].height) {
                        // found the region that the point belongs to 
                        Color newColor = chunkGenerator.regions[i].color;
                        if (noiseMap[x, y] <= 0.2 && hueMap[x, y] > 0) {
                            Color.RGBToHSV(newColor, out H, out S, out V);
                            // get the HSV variables from the color
                            newColor = Color.HSVToRGB(H - hueMap[x, y]/hueStrength, S, V);
                            // use the hsv variables to create a new color, but with modified hue (make it more green or blue)
                        }
                        colorMap[y * mapSize + x] = newColor;
                        // assign the color at given point to the colormap
                        break;
                        // no need to check other chunkGenerator.regions, so break out
                    }
                }
            }
        }
        Texture2D texture = new Texture2D(mapSize, mapSize);
        // create a new texture
        texture.filterMode = FilterMode.Point;
        // point (as opposed to bilinear) filtering
        texture.wrapMode = TextureWrapMode.Clamp;
        // clamp (as opposed to wrap) wrap mode
        texture.SetPixels(colorMap);
        // set the pixels of the texture
        texture.Apply();
        // apply it
        // GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, mapRect, origin, 1f, 0u, SpriteMeshType.FullRect);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, mapSize, mapSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        // create a sprite from the chunk
    }

}
