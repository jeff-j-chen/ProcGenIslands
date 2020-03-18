using UnityEngine;

public static class Noise {
	public static float[,] GenerateNoiseMap(int mapSize, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] noiseMap = new float[mapSize, mapSize];
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
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
            // add a place for the octave to be created
		}
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                // for every coordate (x, y)
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                // variables to be used
				for (int i = 0; i < octaves; i++) {
                    // for every octave
					float sampleX = x / scale * frequency + octaveOffsets[i].x;
					float sampleY = y / scale * frequency + octaveOffsets[i].y;
                    // alter coords based on the octave offset
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    // use builtin perlin noise generation, but change it from 0-1 to -1 to 1
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                    // modify the variables, watch sebastian lague's 1st terrain generation if you forget
				}
                noiseHeight = noiseHeight > 1 ? 1 : noiseHeight;
                // force noiseheight to be 1 or less
                noiseMap[x,y] = noiseHeight;
                // at [x,y] in the array, set the value to be that we just generated
			}
		}
        return noiseMap;
        // return the created float array
	}

	public static float[,] GenerateHueMap(int mapSize, int seed, float scale, float frequency, Vector2 offset) {
        float[,] noiseMap = new float[mapSize, mapSize];
        if (scale <= 0) {
            scale = 0.0001f;
        }
		System.Random prng = new System.Random(seed + 1);
        // use the seed of 1 more, because we want it to be different but still predictable (regions are generated off the previous seed so we want a different region for ocean colors)
        float offsetX = prng.Next(-100000, 100000) + offset.x;
        float offsetY = prng.Next(-100000, 100000) + offset.y;
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                float sampleX = x / scale * frequency + offsetX;
                float sampleY = y / scale * frequency + offsetY;
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                noiseMap[x,y] = perlinValue;
			}
		}
        return noiseMap;
	}
}