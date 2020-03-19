using UnityEngine;

public static class Noise {
	public static float[,] GenerateNoiseMap(int mapSize, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset) {
        float[,] noiseMap = new float[mapSize, mapSize];
        // start off with a noise map with the set dimensions
        if (scale <= 0) {
            scale = 0.0001f;
            // clamp the scale, so no division by 0 or negative problems
        }
		System.Random prng = new System.Random(seed);
        // pseudo random number generator with given seed
		Vector2[] octaveOffsets = new Vector2[octaves];
        // want each octave to be taken from a different location
		for (int i = 0; i < octaves; i++) {
            // for each octave count
			float offsetX = prng.Next(-10000, 10000) + offset.x;
			float offsetY = prng.Next(-10000, 10000) + offset.y;
            // get a new location
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
            // add a place for the octave to be created
		}
        float halfSize = mapSize / 2f;
        // used for centering the map
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                // for every coordinate (x, y)
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                // variables to be used, altered over every octave
				for (int i = 0; i < octaves; i++) {
                    // for every octave
					float sampleX = (((x - halfSize + octaveOffsets[i].x) / scale) * frequency);
					float sampleY = (((y - halfSize + octaveOffsets[i].y) / scale) * frequency);
                    // alter coords based on the octave offset
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    // use builtin perlin noise generation, but change it from 0-1 to -1 to 1
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                    // modify the variables, watch sebastian lague's 1st terrain generation if you forget
				}
                noiseMap[x,y] = noiseHeight;
                // at [x,y] in the array, set the value to be that we just generated
			}
		}
        return noiseMap;
        // return the created float array
	}

	public static float[,] GenerateHueMap(int mapSize, int seed, float scale, float frequency, Vector2 offset) {
        float[,] noiseMap = new float[mapSize, mapSize];
        // create a new map
        if (scale <= 0) {
            scale = 0.0001f;
        }
        // limit the scale
		System.Random prng = new System.Random(seed * 2);
        float offsetX = prng.Next(-10000, 10000) + offset.x;
        float offsetY = prng.Next(-10000, 10000) + offset.y;
        float halfSize = mapSize / 2f;
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                float sampleX = (((x - halfSize + offsetX) / scale) * frequency);
                float sampleY = (((y - halfSize + offsetY) / scale) * frequency);
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                noiseMap[x,y] = perlinValue;
			}
		}
        return noiseMap;
	}
}