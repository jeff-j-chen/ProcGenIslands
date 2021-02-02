using UnityEngine;

public static class Noise {
	public static float[,] GenerateNoiseMap(int chunkSize, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 center) {
        float[,] noiseMap = new float[chunkSize, chunkSize];
        // start off with a noise chunk with the set dimensions
        if (scale <= 0) {
            scale = 0.0001f;
            // clamp the scale, so no division by 0 or negative problems
        }
		System.Random prng = new System.Random(seed);
        // pseudo random number generator with given seed
		Vector2[] octaveCenters = new Vector2[octaves];
        // want each octave to be taken from a different location
		for (int i = 0; i < octaves; i++) {
            // for each octave count
			float offsetX = prng.Next(-10000, 10000);
			float offsetY = prng.Next(-10000, 10000);
            // get a new location
			octaveCenters[i] = new Vector2(offsetX, offsetY);
            // add a place for the octave to be created
		}
        float halfSize = chunkSize / 2f;
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                // for every coordinate (x, y)
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                // variables to be used, altered over every octave
				for (int i = 0; i < octaves; i++) {
                    // for every octave
					float sampleX = (x - halfSize + center.x) / scale * frequency + octaveCenters[i].x;
					float sampleY = (y - halfSize + center.y) / scale * frequency + octaveCenters[i].y;
                    // alter coords based on the octave center
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
                    // use builtin perlin noise generation, but change it from 0-1 to -1 to 1
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                    // modify the variables, watch sebastian lague's 1st terrain generation if you forget
				}
                noiseMap[x,y] = Mathf.Clamp(noiseHeight, -1, 1);
                // at [x,y] in the array, set the value to be that we just generated
			}
		}
        return noiseMap;
        // return the created float array
	}

	public static float[,] GenerateHueMap(int chunkSize, int seed, float scale, float frequency, Vector2 center) {
        float[,] noiseMap = new float[chunkSize, chunkSize];
        // create a new chunk
        if (scale <= 0) {
            scale = 0.0001f;
        }
        // limit the scale
		System.Random prng = new System.Random(seed);
        // get the seed
        float offsetX = prng.Next(-10000, 10000);
        float offsetY = prng.Next(-10000, 10000);
        // create offsets
        float halfSize = chunkSize / 2f;
        // get the halfsize
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                // for every point (x, y)
                float sampleX = (((x - halfSize + center.x) / scale) * frequency) + offsetX;
                float sampleY = (((y - halfSize + center.y) / scale) * frequency) + offsetY;
                // set the positions, taking into account scale, frequency, center, etc.
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
                // get the perlin value at that poni
                noiseMap[x,y] = perlinValue;
                // assign (x,y)
			}
		}
        return noiseMap;
        // return the created noisemap
	}

    public static float[,] GenerateDitherMap(int chunkSize, int seed, Vector2 center, float ditherStrength) {
        float[,] noiseMap = new float[chunkSize, chunkSize];
        // new float array
		System.Random prng = new System.Random(seed / 2);
        // get the seed
        float centerX = prng.Next(-10000, 10000) + center.x;
        float centerY = prng.Next(-10000, 10000) + center.y;
        // get the centers
        float halfSize = chunkSize / 2f;
        // get the halfsizes
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                // for every point (x, y)
                noiseMap[x,y] = ((float)prng.NextDouble() * 2f + 1f) / ditherStrength;
                // get a random float from -1 to 1, and divide by ditherstrength
			}
		}
        return noiseMap;
        // return the created noisemap
	}
}