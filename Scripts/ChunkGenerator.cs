using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour {
    [Header("Main")]
    public GameObject mapPrefab;
    // the gameobject sprite prefab we are creating
	public int chunkSize;
    // size of the map to create (size x size) 
	public float noiseScale;
    // scale of the noise to create, bigger number is bigger noise
	public int octaves = 7;
    // # of iterations of perlin noises to stack
	[Range(0,1)] public float persistence;
    // how much each level of perlin noise affects the next one, higher is a greater effect
	public float lacunarity;
    // the factor to increase the frequency by every octave
    public float hueFrequency;
    // frequency of the noise to create, bigger number is bigger noise
	public int seed;
    // the seed to pass in
    public Vector2 center;
    // the center where to spawn the chunk at
    public TerrainType[] regions;
    // array of structure which manages the colors applied to varying heights
	public bool autoUpdate;
    // whether or not to automatically update the map every time something changes
    [Header("Hue")]
    public bool applyHueRegions;
    // whether or not to apply the hue (green/blue) regions onto the map
    public float hueStrength;
    private float[,] hueMap;
    // float array representing the hue map
    // the strength of the hue to apply, bigger number is smaller effect
    [Header("Dither")]
    public bool applyDither;
    // whether or not to apply dithering to the map
    public float ditherStrength;
    // the strength of the dither to apply, bigger number is smaller effect
    private float[,] ditherMap;
    // float array representing the dither map
    [Header("Coral")]
    public Color[] coralColors = new Color[5];
    public float coralScale;
    public float coralFrequency;
    [Header("Other")]
    public List<GameObject> chunks = new List<GameObject>();
    // list of created chunks
    public GameObject centerChunk;
    // pink purple yellow red blue
    private Vector2 origin = new Vector2(0.5f, 0.5f);
    // pivot point to give sprites a pivot at their center

	public GameObject GenerateChunkAt(Vector2 center, bool removeExisting=false) {
        if (applyHueRegions) { hueMap = Noise.GenerateHueMap(chunkSize, seed, noiseScale, hueFrequency, center); }
        if (applyDither) { ditherMap = Noise.GenerateDitherMap(chunkSize, seed, center, ditherStrength); }
		float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, seed, noiseScale, octaves, persistence, lacunarity, center);
        // generate the noise map with the given variables
        Color[] colorMap = new Color[chunkSize * chunkSize];
        // make a new colormap to apply colors to
        float H, S, V;
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                // for every coordinate
                // if apply falloffmap, alter (x,y) as such
                float currentHeight = noiseMap[x, y];
                // get the current height at (x, y)
                for (int i = 0; i < regions.Length; i++) {
                    // for every region
                    if (currentHeight <= regions[i].height) {
                        // found the region that the point belongs to 
                        Color newColor = regions[i].color;
                        if (applyHueRegions && currentHeight <= 0.2 && hueMap[x, y] > 0) {
                            Color.RGBToHSV(newColor, out H, out S, out V);
                            // get the HSV variables from the color
                            newColor = Color.HSVToRGB(H - hueMap[x, y]/hueStrength, S, V);
                            // use the hsv variables to create a new color, but with modified hue (make it more green or blue)
                        }
                        if (applyDither) {
                            Color.RGBToHSV(newColor, out H, out S, out V);
                            // get the HSV variables from the color
                            newColor = Color.HSVToRGB(H, S + ditherMap[x,y] / 2, V + ditherMap[x,y]);
                            // use the hsv variables to create a new color, but with modified saturation and value
                        }
                        colorMap[y * chunkSize + x] = newColor;
                        // assign the color at given point to the colormap
                        break;
                        // no need to check other regions, so break out
                    }
                }
            }
        }
        Texture2D texture = new Texture2D(chunkSize, chunkSize);
        // create a new texture
        texture.filterMode = FilterMode.Point;
        // point (as opposed to bilinear) filtering
        texture.wrapMode = TextureWrapMode.Clamp;
        // clamp (as opposed to wrap) wrap mode
        texture.SetPixels(colorMap);
        // set the pixels of the texture
        texture.Apply();
        // apply it
        if (removeExisting) { 
            foreach(GameObject chunk in chunks) { DestroyImmediate(chunk); }
            chunks.Clear();
        }
        // remove any existing chunks if desired (used for editing in scene mode)
        GameObject newChunk = Instantiate(mapPrefab, center, Quaternion.identity);
        // instantiate a new chunk gameobject
        newChunk.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, chunkSize, chunkSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        // create a sprite from the chunk
        chunks.Add(newChunk);
        // add it to the list
        return newChunk;
	}

	public GameObject CoralChunkTest(Vector2 center) {
        hueMap = Noise.GenerateHueMap(chunkSize, seed, noiseScale, hueFrequency, center);
        ditherMap = Noise.GenerateDitherMap(chunkSize, seed, center, ditherStrength);
		float[,] noiseMap = Noise.GenerateCoralMap(chunkSize, hueMap, seed, coralScale, coralFrequency, center);
        Color[] colorMap = new Color[chunkSize * chunkSize];
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                colorMap[y * chunkSize + x] = Color.Lerp(Color.black, Color.white, noiseMap[x,y]);
            }
        }
        Texture2D texture = new Texture2D(chunkSize, chunkSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        foreach(GameObject chunk in chunks) { DestroyImmediate(chunk); }
        chunks.Clear();
        GameObject newChunk = Instantiate(mapPrefab, center, Quaternion.identity);
        newChunk.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, chunkSize, chunkSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        chunks.Add(newChunk);
        return newChunk;
	}

	void OnValidate() {
        // called every time an inspector variable is updated
		if (chunkSize < 1) { chunkSize = 1; }
		if (lacunarity < 1) { lacunarity = 1; }
		if (octaves < 0) { octaves = 0; }
        // limit some variables
	}
}

[System.Serializable] // make sure it can be seen through the inspector
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
    // create a new structure used for assigning colors
}

// public static class TextureGenerator {
//     public static Texture2D TextureFromColorMap(Color[] colorMap, int size) {
//         Texture2D texture = new Texture2D(size, size);
//         // create a new texture
//         texture.filterMode = FilterMode.Point;
//         // point, as opposed to bilinear, filtering
//         texture.wrapMode = TextureWrapMode.Clamp;
//         // clamp, as opposed to wrap, wrap mode
//         texture.SetPixels(colorMap);
//         // set the pixels of the texture
//         texture.Apply();
//         // apply it
//         return texture;
//         // return it
//     }

//     public static Texture2D TextureFromHeightMap(float[,] heightMap) {
//         int size = heightMap.GetLength(0);
//         // get the width of the map based on the dimensions of the float[,]
//         Color[] colorMap = new Color[size * size];
//         // create a new one-dimensional array of colors from the width and height
//         for (int y = 0; y < size; y++) {
//             for (int x = 0; x < size; x++) {
//                 colorMap[y * size + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
//                 // at the given index, get the color (from black to white) based on the noisemap point at (x,y)
//             }
//         }
//         return TextureFromColorMap(colorMap, size);
// }