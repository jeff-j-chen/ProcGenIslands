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

    // public static int[,] GenerateCoralMap(int chunkSize, float[,] hueMap, int seed, int spawnChance, Vector2 center) {
    //     int[,] noiseMap = new int[chunkSize, chunkSize];
    //     System.Random prng = new System.Random(seed - 2);
    //     float centerX = prng.Next(-10000, 10000) + center.x;
    //     float centerY = prng.Next(-10000, 10000) + center.y;
    //     float halfSize = chunkSize / 2f;
    //     for (int y = 0; y < chunkSize; y++) {
    //         for (int x = 0; x < chunkSize; x++) {
    //             if (prng.Next(1000) <= spawnChance * (hueMap[x,y] - 0.25f)) {
    //                 noiseMap = GenerateCoralShape(noiseMap, x, y, seed, prng);
    //                 // make clusters
    //                 // perlin noise the clusters but have a much heigher tolerance 
    //             }
    //         }
    //     }
    //     return noiseMap;
    // }

    // private static int[,] GenerateCoralShape(int[,] noiseMap, int x, int y, int seed, System.Random prng) {
    //     int rand = prng.Next(18);
    //     int colorToUse = prng.Next(1, 6);
    //     try { 
    //         if (rand == 0) {
    //             noiseMap[x,y] = colorToUse;
    //             // 1 dot
    //         }
    //         else if (rand == 1) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x-1,y+1] = colorToUse;
    //             // left diagonal 2
    //         }
    //         else if (rand == 2) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+1,y+1] = colorToUse;
    //             // diagonal 2
    //         }
    //         else if (rand == 3) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x-1,y+1] = colorToUse;
    //             noiseMap[x-1,y-1] = colorToUse;
    //             // left c 
    //         }
    //         else if (rand == 4) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+1,y+1] = colorToUse;
    //             noiseMap[x+1,y-1] = colorToUse;
    //             // right c 
    //         }
    //         else if (rand == 5) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x-1,y+1] = colorToUse;
    //             noiseMap[x+1,y+1] = colorToUse;
    //             // up c 
    //         }
    //         else if (rand == 6) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+1,y-1] = colorToUse;
    //             noiseMap[x-1,y-1] = colorToUse;
    //             // down c 
    //         }
    //         else if (rand == 7) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+1,y] = colorToUse;
    //             // horizontal 2
    //         }
    //         else if (rand == 8) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+1,y] = colorToUse;
    //             noiseMap[x+2,y] = colorToUse;
    //             // horizontal 3
    //         }
    //         else if (rand == 9) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+1,y] = colorToUse;
    //             noiseMap[x+2,y] = colorToUse;
    //             noiseMap[x+3,y] = colorToUse;
    //             // horizontal 4
    //         }
    //         else if (rand == 10) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+1,y] = colorToUse;
    //             noiseMap[x+1,y+1] = colorToUse;
    //             noiseMap[x+2,y] = colorToUse;
    //             noiseMap[x+2,y-1] = colorToUse;
    //             noiseMap[x+3,y] = colorToUse;
    //             // fancy horizontal 4
    //         }
    //         else if (rand == 11) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x,y+1] = colorToUse;
    //             // vertical 2
    //         }
    //         else if (rand == 12) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x,y+1] = colorToUse;
    //             noiseMap[x,y+2] = colorToUse;
    //             // vertical 3
    //         }
    //         else if (rand == 13) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x,y+1] = colorToUse;
    //             noiseMap[x,y+2] = colorToUse;
    //             noiseMap[x,y+3] = colorToUse;
    //             // vertical 4
    //         }
    //         else if (rand == 14) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x,y+1] = colorToUse;
    //             noiseMap[x+1,y+1] = colorToUse;
    //             noiseMap[x,y+2] = colorToUse;
    //             noiseMap[x-1,y+2] = colorToUse;
    //             noiseMap[x,y+3] = colorToUse;
    //             // fancy vertical 4
    //         }
    //         else if (rand == 15) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x,y+1] = colorToUse;
    //             noiseMap[x+1,y] = colorToUse;
    //             noiseMap[x+1,y+1] = colorToUse;
    //             // 2x2 square
    //         }
    //         else if (rand == 16) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x+2,y] = colorToUse;
    //             noiseMap[x,y+2] = colorToUse;
    //             noiseMap[x+2,y+2] = colorToUse;
    //             // 4 corners
    //         }
    //         else if (rand == 17) {
    //             noiseMap[x,y] = colorToUse;
    //             noiseMap[x,y+1] = colorToUse;
    //             noiseMap[x+1,y+2] = colorToUse;
    //             noiseMap[x+2,y+2] = colorToUse;
    //             noiseMap[x+3,y+1] = colorToUse;
    //             noiseMap[x+3,y] = colorToUse;
    //             noiseMap[x+2,y-1] = colorToUse;
    //             noiseMap[x+1,y-1] = colorToUse;
    //             // cutout
    //         }
    //     }
    //     catch {}
    //     return noiseMap;
    // }
}