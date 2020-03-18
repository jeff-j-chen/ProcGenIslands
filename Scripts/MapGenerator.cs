using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
    public enum DrawMode { NoiseMap, ColorMap };
    public DrawMode drawMode;
	public int mapWidth;
	public int mapHeight;
	public float noiseScale;
	public int octaves;
	[Range(0,1)]
	public float persistence;
	public float lacunarity;
	public int seed;
	public Vector2 offset;
	public bool autoUpdate;
    public TerrainType[] regions;

	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);
        // generate the noise map with the given variables
        Color[] colorMap = new Color[mapWidth * mapHeight];
        // make a new colormap to apply colors to
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                // for every coordinate
                float currentHeight = noiseMap[x, y];
                // get the current height at (x, y)
                for (int i = 0; i < regions.Length; i++) {
                    // for every region
                    if (currentHeight <= regions[i].height) {
                        // found the region that the point belongs to 
                        colorMap[y * mapWidth + x] = regions[i].color;
                        // assign the color at given point to the colormap
                        break;
                        // no need to check other regions, so break out
                    }
                }
            }
        }
        // return the created float array
		MapDisplay display = FindObjectOfType<MapDisplay>();
        // get the display script
        if (drawMode == DrawMode.NoiseMap) { display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); }
        else if (drawMode == DrawMode.ColorMap) { display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight)); }
        // draw the noise or color map on the plane, based on which one we want
	}

	void OnValidate() {
        // called every time an inspector variable is updated
		if (mapWidth < 1) { mapWidth = 1; }
		if (mapHeight < 1) { mapHeight = 1; }
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