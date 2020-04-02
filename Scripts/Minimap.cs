using UnityEngine;
using System.Collections.Generic;

public class Minimap : MonoBehaviour {
    public Camera mainCamera;
    public Camera minimapCamera;
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
    public float zoomLevel;
    public float zoomIncrease;
    public GameObject chunkParent;
    Vector3 downPoint;


    private void Awake() {
        mainCamera.enabled = true;
        minimapCamera.enabled = false;
        chunkGenerator = FindObjectOfType<ChunkGenerator>();
        player = FindObjectOfType<Player>();
        mapRect = new Rect(0, 0, mapSize, mapSize);
        seed = chunkGenerator.seed;
        // assign various things needed later on
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            // on tab press
            minimapCamera.orthographicSize = zoomLevel;
            // set the camera's size to the zoom level
            minimapCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            // center it around the player
            mainCamera.enabled = !mainCamera.enabled;
            minimapCamera.enabled = !minimapCamera.enabled;
            // swap cameras
            if (minimapCamera.enabled) {
                // if we just enabled the minimap camera
                chunkParent.transform.position = new Vector3(chunkParent.transform.position.x, chunkParent.transform.position.y, -3f);
                // move the chunks forwards
                for (int i = 0; i < chunkGenerator.chunks.Count; i++) {
                    chunkGenerator.chunks[i].GetComponent<SpriteRenderer>().sortingLayerName = "ui";
                    chunkGenerator.chunks[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
                // make each chunk render on top
                player.transform.localScale = new Vector3(zoomLevel / 10f, zoomLevel / 10f, 1f);
                // set the scale for the player
                player.GetComponent<SpriteRenderer>().sortingLayerName = "ui";
                player.GetComponent<SpriteRenderer>().sortingOrder = 4;
                // make the player render on top
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 10f);
                // move the minimap's position back
            }
            else {
                chunkParent.transform.position = new Vector3(chunkParent.transform.position.x, chunkParent.transform.position.y, 0f);
                // move the chunks backwards
                for (int i = 0; i < chunkGenerator.chunks.Count; i++) {
                    chunkGenerator.chunks[i].GetComponent<SpriteRenderer>().sortingLayerName = "default";
                    chunkGenerator.chunks[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
                }
                // make each chunk render at the back
                player.transform.localScale = new Vector3(1f, 1f, 1f);
                // reset the player's size
                player.GetComponent<SpriteRenderer>().sortingLayerName = "default";
                player.GetComponent<SpriteRenderer>().sortingOrder = 2;
                // make the player render on top of the chunks
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 9f);
                // move the minimap's position forwards
            }

        }
        if (minimapCamera.enabled) {
            // if the minimap camera is enabled
            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                if (minimapCamera.orthographicSize > zoomIncrease) { 
                    zoomLevel -= zoomIncrease; 
                    minimapCamera.orthographicSize = zoomLevel;
                }
                player.transform.localScale = new Vector3(zoomLevel / 10f, zoomLevel / 10f, 1f);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                zoomLevel += zoomIncrease; 
                minimapCamera.orthographicSize = zoomLevel;
                player.transform.localScale = new Vector3(zoomLevel / 10f, zoomLevel / 10f, 1f);
            }
            // zoom the minimaps and players in and out
        }
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
