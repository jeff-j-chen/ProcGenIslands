using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
    public enum DrawMode { NoiseMap, ColorMap, FalloffMap, HueMap };
    public DrawMode drawMode;
	public int mapSize;
	public float noiseScale;
	public int octaves = 7;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;
    public float hueFrequency;
	public int seed;
	public Vector2 offset;
	public bool autoUpdate;
    public bool applyFalloffMap;
    public bool applyHueRegions;
    public bool generateNewOctaves;
    public List<int> octaveXs;
    public List<int> octaveYs;
    public float hueStrength;
    public TerrainType[] regions;
    private float[,] falloffMap;
    private float[,] hueMap;

	public void GenerateMap() {
        if (generateNewOctaves) {
            octaveXs.Clear();
            octaveYs.Clear();
            System.Random prng = new System.Random(seed);
            for (int i = 0; i < octaves; i++) {
                octaveYs.Add(prng.Next(-10000, 10000));
                octaveXs.Add(prng.Next(-10000, 10000));
            }
        }
        // print("started generation");
        if (applyFalloffMap) { falloffMap = FalloffGenerator.GenerateFalloffMap(mapSize); }
        // print("finished falloff");
        if (applyHueRegions) { hueMap = Noise.GenerateHueMap(mapSize, seed, noiseScale, hueFrequency, offset); }
        // print("finished hue");
		float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset, octaveXs, octaveYs);
        // print("finished noise map");
        // generate the noise map with the given variables
        Color[] colorMap = new Color[mapSize * mapSize];
        // make a new colormap to apply colors to
        float H, S, V;
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                // for every coordinate
                if (applyFalloffMap) {
                    noiseMap[x,y] -= falloffMap[x,y];
                }
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
                            newColor = Color.HSVToRGB(H - hueMap[x, y]/hueStrength, S, V);
                        }
                        colorMap[y * mapSize + x] = newColor;
                        // assign the color at given point to the colormap
                        break;
                        // no need to check other regions, so break out
                    }
                }
            }
        }
        // print("finished coloring");
        // return the created float array
		MapDisplay display = FindObjectOfType<MapDisplay>();
        // get the display script
        if (drawMode == DrawMode.NoiseMap) { display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); }
        else if (drawMode == DrawMode.ColorMap) { display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapSize)); }
        else if (drawMode == DrawMode.FalloffMap) { TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapSize)); }
        else if (drawMode == DrawMode.HueMap) { display.DrawTexture(TextureGenerator.TextureFromHeightMap(Noise.GenerateHueMap(mapSize, seed, noiseScale, hueFrequency, offset))); }
        // print("finished applying texture");
	}

	void OnValidate() {
        // called every time an inspector variable is updated
		if (mapSize < 1) { mapSize = 1; }
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