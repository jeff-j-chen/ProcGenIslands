using UnityEngine;

public class Minimap : MonoBehaviour {
    public int mapSize;
    // how large of a map to genrerate
    private int seed;
    // the seed to use (DO NOT ASSIGN)
    public int mapScale;
    // the scale of the map to create
    public int octaves;
    public float persistence;
    public float lacunarity;
    // same as in noise.cs
    private Vector2 origin = new Vector2(0.5f, 0.5f);
    private ChunkGenerator chunkGenerator;
    private Rect mapRect;
    private Player player;
    // pretty self explanatory

    private void Start() {
        chunkGenerator = FindObjectOfType<ChunkGenerator>();
        player = FindObjectOfType<Player>();
        mapRect = new Rect(0, 0, mapSize, mapSize);
        seed = chunkGenerator.seed;
        // assign various things needed later on
    }

    private void FixedUpdate() {
        // ~50 times per second, no matter the machine
        Vector2 center = new Vector2(Mathf.RoundToInt(player.transform.position.x * (mapScale / chunkGenerator.noiseScale)), Mathf.Round(player.transform.position.y * (mapScale / chunkGenerator.noiseScale)));
        // get the position of the player from the actual map relative to the minimap
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, seed, mapScale, octaves, persistence, lacunarity, center);
        // create the maps
        Color[] colorMap = new Color[mapSize * mapSize];
        // create an array to place colors into
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                // for every point (x,y)
                for (int i = 0; i < chunkGenerator.regions.Length; i++) {
                    // for every region
                    if (noiseMap[x, y] <= chunkGenerator.regions[i].height) {
                        // found the region that the point belongs to 
                        colorMap[y * mapSize + x] = chunkGenerator.regions[i].color;
                        // assign the color at given point to the colormap
                        break;
                        // no need to check other regions, so break out
                    }
                }
            }
        }
        Texture2D texture = new Texture2D(mapSize, mapSize);
        // create a new texture
        texture.filterMode = FilterMode.Point;
        // point (as opposed to bilinear) filtering
        texture.wrapMode = TextureWrapMode.Clamp;
        // clamp (as opposed to wrap) wrap mode
        texture.SetPixels(colorMap);
        // set the pixels of the texture
        texture.Apply();
        // apply it
        // GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, mapRect, origin, 1f, 0u, SpriteMeshType.FullRect);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, mapSize, mapSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        // create a sprite from the chunk
    }
}
