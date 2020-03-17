using UnityEngine;
using System.Collections;

public static class Noise {

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        // start off with a noise map with the set dimensions
        if (scale <= 0) {
            scale = 0.0001f;
            // clamp the scale, so no division by 0 or negative problems
        }
		System.Random prng = new System.Random(seed);
        // pseudo random number generator
		Vector2[] octaveOffsets = new Vector2[octaves];
        // want each octave to be taken from a different location
		for (int i = 0; i < octaves; i++) {
            // for each octave count
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
            // get a new location
			octaveOffsets[i] = new Vector2 (offsetX, offsetY);
            // add a place for the octave to be created
		}
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;
        // min and max heights, used for normalization
		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;
        // variables, used for zooming in to center
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                // for every coordate (x, y)
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                // variables to be used
				for (int i = 0; i < octaves; i++) {
                    // for every octave
					float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;
                    // alter coords based on the octave offset, zoom in to the center with half width/height
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    // use builtin perlin noise generation, but change it from 0-1 to -1 to 1
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                    // modify the variables, watch sebastian lague's 1st terrain generation if you forget
				}
                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                // get the min and max noise heights 
                noiseMap[x,y] = noiseHeight;
                // at [x,y] in the array, set the value to be that we just generated
			}
		}

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                // return 0 to 1 based on the noise map height (normalzed it)
            }
        }
        return noiseMap;
        // return the created float array
	}

}