using UnityEngine;
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
    [Header("Biome")] 
    public float biomeHueStrength;
    // the strength of the hue to apply, bigger number is smaller effect
    public float biomeValueStrength;
    // the strength of the value to apply, bigger number is smaller effect
    public float biomeValueOffset;
    // how much to offset the initial value by
    public float biomeSaturationStrength;
    // the strength of the saturation to apply, bigger number is smaller effect
    public float biomeSaturationOffset;
    // how much to offset the initial saturation by
    public int biomeNoiseScale;
    // how large to generate the noise
    public int biomeOctaves;
    [Range(0,1)] public float biomePersistence;
    public float biomeLacunarity;
    // noise variables that apply to biomes

    [Header("Hue")]
    public float hueStrength;
    // the strength of the hue to apply, bigger number is smaller effect
    [Header("Dither")]
    public float ditherStrength;
    // the strength of the dither to apply, bigger number is smaller effect
    // float array representing the dither map
    [Header("Other")]
    public List<GameObject> chunks = new List<GameObject>();
    // list of created chunks
    public List<Vector2> chunkPositions = new List<Vector2>();
    public GameObject centerChunk;
    // pink purple yellow red blue
    private Vector2 origin = new Vector2(0.5f, 0.5f);
    // pivot point to give sprites a pivot at their center
    float[,] hueMap;
    float[,] biomeHueMap;
    float[,] biomeValueMap;
    float[,] biomeSaturationMap;
    float[,] ditherMap;
    float[,] noiseMap;
    // 2d float arrays for heightmaps we create
    public GameObject chunkParent;
    // the gameobject to child all the chunks to
    
    private void Awake() {
        chunks = new List<GameObject>();
        // clear the chunk array
    }

	public GameObject GenerateChunkAt(Vector2 center, bool removeExisting=false) {
        hueMap = Noise.GenerateHueMap(chunkSize, seed - 2, noiseScale, hueFrequency, center);
        // hue map for oceans
        ditherMap = Noise.GenerateDitherMap(chunkSize, seed - 1, center, ditherStrength);
        // universal dither map
		noiseMap = Noise.GenerateNoiseMap(chunkSize, seed, noiseScale, octaves, persistence, lacunarity, center);
        // basic height map
		biomeHueMap = Noise.GenerateNoiseMap(chunkSize, seed + 1, biomeNoiseScale, biomeOctaves, biomePersistence, biomeLacunarity, center);
        // hue map for biomes
		biomeValueMap = Noise.GenerateNoiseMap(chunkSize, seed + 2, biomeNoiseScale, biomeOctaves, biomePersistence, biomeLacunarity, center);
        // value map for biomes
		biomeSaturationMap = Noise.GenerateNoiseMap(chunkSize, seed + 3, biomeNoiseScale, biomeOctaves, biomePersistence, biomeLacunarity, center);
        // saturation map for biomes
        Color[] colorMap = new Color[chunkSize * chunkSize];
        // make a new colormap to apply colors to
        float H, S, V;
        // variables used for manipulating colors
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                // for every coordinate
                float currentHeight = noiseMap[x, y];
                // get the current height at (x, y)
                for (int i = 0; i < regions.Length; i++) {
                    // for every region
                    if (currentHeight <= regions[i].height) {
                        // found the region that the point belongs to 
                        Color newColor = regions[i].color;
                        // get the color of that region
                        if (currentHeight <= 0.2 && hueMap[x, y] > 0) {
                            // if (x,y) is water and we want to apply a hue there (based on the heightmap)
                            Color.RGBToHSV(newColor, out H, out S, out V);
                            // get the HSV variables from the color
                            newColor = Color.HSVToRGB(H - hueMap[x, y] / hueStrength, S, V);
                            // use the hsv variables to create a new color, but with modified hue (make it more green or blue), as well as adding a dither effect
                        }
                        else if (currentHeight > 0.2) {
                            // else is on land
                            Color.RGBToHSV(newColor, out H, out S, out V);
                            // get the HSV variables from the color
                            newColor = Color.HSVToRGB(
                                H + ((biomeHueMap[x,y] + 1) / 2) / biomeHueStrength, 
                                S + (biomeSaturationMap[x,y] - biomeSaturationOffset) / biomeSaturationStrength, 
                                V + (biomeValueMap[x,y] - biomeValueOffset) / biomeValueStrength
                            );
                            // generate a new color from manipulating the hue, saturation, and value based on the 3 created heightmaps, as well as adding a dither effect
                        }
                        Color.RGBToHSV(newColor, out H, out S, out V);
                        // get the HSV variables from the color
                        newColor = Color.HSVToRGB(H, S + ditherMap[x,y] / 2, V + ditherMap[x,y]);
                        // apply dither, DO NOT COMBINE WITH HUE SETTINGS
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
        newChunk.GetComponent<Chunk>().noiseMap = noiseMap;
        // assign the noisemap variable of the chunk
        newChunk.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, chunkSize, chunkSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        // create a sprite from the chunk
        chunks.Add(newChunk);
        chunkPositions.Add(center);
        // add it to the list
        newChunk.transform.parent = chunkParent.transform;
        // child the chunk to a gameobject
        newChunk.layer = 0;
        return newChunk;
        // return it
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