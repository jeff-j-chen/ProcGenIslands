using UnityEngine;

public static class Noise {
	public static float[,] GenerateNoiseMap(int mapSize, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 center) {
        float[,] noiseMap = new float[mapSize, mapSize];
        // start off with a noise map with the set dimensions
        if (scale <= 0) {
            scale = 0.0001f;
            // clamp the scale, so no division by 0 or negative problems
        }
		System.Random prng = new System.Random(seed);
        // pseudo random number generator with given seed
		Vector2[] octavecenters = new Vector2[octaves];
        // want each octave to be taken from a different location
		for (int i = 0; i < octaves; i++) {
            // for each octave count
			float centerX = prng.Next(-10000, 10000) + center.x;
			float centerY = prng.Next(-10000, 10000) + center.y;
            // get a new location
			octavecenters[i] = new Vector2(centerX, centerY);
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
					float sampleX = (((x - halfSize + octavecenters[i].x) / scale) * frequency);
					float sampleY = (((y - halfSize + octavecenters[i].y) / scale) * frequency);
                    // alter coords based on the octave center
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
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

	public static float[,] GenerateHueMap(int mapSize, int seed, float scale, float frequency, Vector2 center) {
        float[,] noiseMap = new float[mapSize, mapSize];
        // create a new map
        if (scale <= 0) {
            scale = 0.0001f;
        }
        // limit the scale
		System.Random prng = new System.Random(seed * 2);
        float centerX = prng.Next(-10000, 10000) + center.x;
        float centerY = prng.Next(-10000, 10000) + center.y;
        float halfSize = mapSize / 2f;
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                float sampleX = (((x - halfSize + centerX) / scale) * frequency);
                float sampleY = (((y - halfSize + centerY) / scale) * frequency);
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
                noiseMap[x,y] = perlinValue;
			}
		}
        return noiseMap;
	}

    public static float[,] GenerateDitherMap(int mapSize, int seed, Vector2 center, float ditherStrength) {
        float[,] noiseMap = new float[mapSize, mapSize];
		System.Random prng = new System.Random(seed / 2);
        float centerX = prng.Next(-10000, 10000) + center.x;
        float centerY = prng.Next(-10000, 10000) + center.y;
        float halfSize = mapSize / 2f;
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                noiseMap[x,y] = ((float)prng.NextDouble() * 2f + 1f)/ditherStrength;
			}
		}
        return noiseMap;
	}
}