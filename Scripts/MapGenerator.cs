using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
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
	public bool autoUpdate;
    // whether or not to automatically update the map every time something changes
    public bool applyHueRegions;
    // whether or not to apply the hue (green/blue) regions onto the map
    public float hueStrength;
    // the strength of the hue to apply, bigger number is smaller effect
    public TerrainType[] regions;
    // array of structure which manages the colors applied to varying heights
    private float[,] hueMap;
    // float array representing the hue map
    private Vector2 origin = new Vector2(0.5f, 0.5f);
    // pivot point to give sprites a pivot at their center
    public Vector2 center;
    // temp
    public List<GameObject> chunks;
    // list of created chunks

	public void GenerateChunkAt(Vector2 center) {
        if (applyHueRegions) { hueMap = Noise.GenerateHueMap(chunkSize, seed, noiseScale, hueFrequency, center); }
        // print("finished hue");
		float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, seed, noiseScale, octaves, persistence, lacunarity, center);
        // print("finished noise map");
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
                        colorMap[y * chunkSize + x] = newColor;
                        // assign the color at given point to the colormap
                        break;
                        // no need to check other regions, so break out
                    }
                }
            }
        }
        // print("finished coloring");
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
        GameObject newChunk = Instantiate(mapPrefab, center, Quaternion.identity);
        // instantiate a new chunk gameobject
        // foreach(GameObject chunk in chunks) { DestroyImmediate(chunk); }
        // chunks.Clear();
        newChunk.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, chunkSize, chunkSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        chunks.Add(newChunk);
        // create a sprite from the chunk
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