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
        float halfSize = chunkSize / 2f;
        // used for centering the chunk
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
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

	public static float[,] GenerateHueMap(int chunkSize, int seed, float scale, float frequency, Vector2 center) {
        float[,] noiseMap = new float[chunkSize, chunkSize];
        // create a new chunk
        if (scale <= 0) {
            scale = 0.0001f;
        }
        // limit the scale
		System.Random prng = new System.Random(seed * 2);
        float centerX = prng.Next(-10000, 10000) + center.x;
        float centerY = prng.Next(-10000, 10000) + center.y;
        float halfSize = chunkSize / 2f;
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                float sampleX = (((x - halfSize + centerX) / scale) * frequency);
                float sampleY = (((y - halfSize + centerY) / scale) * frequency);
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
                noiseMap[x,y] = perlinValue;
			}
		}
        return noiseMap;
	}

    public static float[,] GenerateDitherMap(int chunkSize, int seed, Vector2 center, float ditherStrength) {
        float[,] noiseMap = new float[chunkSize, chunkSize];
		System.Random prng = new System.Random(seed / 2);
        float centerX = prng.Next(-10000, 10000) + center.x;
        float centerY = prng.Next(-10000, 10000) + center.y;
        float halfSize = chunkSize / 2f;
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                noiseMap[x,y] = ((float)prng.NextDouble() * 2f + 1f)/ditherStrength;
			}
		}
        return noiseMap;
	}

    public static float[,] GenerateCoralMap(int chunkSize, float[,] hueMap, int seed, float scale, float frequency, Vector2 center) {
        float[,] noiseMap = new float[chunkSize, chunkSize];
        // generate chunks of coral (squares? circles?) with one color, all across the ocean where the huemap makes it green
        // run completely random filters over the chunks, more filters where there is less green, several times, generating blocks of coral with the same color and increasing density towards the center
        // use the int[,] in chunkgenerator to get the color from the array and apply it to that point
        // apply noise filter to the coral as well?
        System.Random prng = new System.Random(seed - 2);
        float centerX = prng.Next(-10000, 10000) + center.x;
        float centerY = prng.Next(-10000, 10000) + center.y;
        float halfSize = chunkSize / 2f;
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                hueMap[x,y] = hueMap[x,y] < 0 ? 0 : hueMap[x,y];
                if (prng.Next(100) <= 10 * hueMap[x,y]) {
                    noiseMap = GenerateCoralShape(noiseMap, x, y, seed);
                }
                // float sampleX = (((x - halfSize + centerX) / scale) * frequency);
                // float sampleY = (((y - halfSize + centerY) / scale) * frequency);
                // float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                // noiseMap[x,y] = perlinValue;
            }
        }
        return noiseMap;
    }

    private static float[,] GenerateCoralShape(float[,] noiseMap, int x, int y, int seed) {
        System.Random prng = new System.Random(seed + 2);
        int rand = prng.Next(5); // change to all 20 later
        if (rand == 0) {
            noiseMap[x,y] = 1;
        }
        else if (rand == 1) {
            noiseMap[x,y] = 1;
            noiseMap[x-1,y+1] = 1;
        }
        else if (rand == 2) {
            noiseMap[x,y] = 1;
            noiseMap[x+1,y+1] = 1;
        }
        else if (rand == 3) {
            noiseMap[x,y] = 1;
            noiseMap[x+1,y] = 1;
        }
        else if (rand == 4) {
            noiseMap[x,y] = 1;
            noiseMap[x+1,y+1] = 1;
        }
        return noiseMap;
    }
}