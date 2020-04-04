using UnityEngine;

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
    // chunkgenerator script
    private Player player;
    // player script
    private Rect mapRect;
    // so we don't have to create a new rectangle every time
    public float zoomLevel;
    // the zoom of the map
    public float zoomIncrease;
    // how much to increase/decrease the camera's orthographic size by
    public GameObject chunkParent;
    // gameobject with all the chunks childed to it

    private void Awake() {
        mainCamera.enabled = true;
        minimapCamera.enabled = false;
        // set the main camera on
        chunkGenerator = FindObjectOfType<ChunkGenerator>();
        // get the chunk generator
        player = FindObjectOfType<Player>();
        // get the player
        mapRect = new Rect(0, 0, mapSize, mapSize);
        // create the rect
        seed = chunkGenerator.seed;
        // get the seed
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) && !FindObjectOfType<PauseCode>().gameIsPaused) {
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
                // on scrolling down
                if (minimapCamera.orthographicSize > zoomIncrease) {
                    // if we aren't zooming into the negatives
                    zoomLevel -= zoomIncrease;
                    // decrement zoom
                    minimapCamera.orthographicSize = zoomLevel;
                    // set the camera's orthographic size
                }
                player.transform.localScale = new Vector3(zoomLevel / 10f, zoomLevel / 10f, 1f);
                // change the player's scale based on the minimap
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                // on scrolling up
                zoomLevel += zoomIncrease;
                // increment zoom
                minimapCamera.orthographicSize = zoomLevel;
                // set the camera's orthographic size
                player.transform.localScale = new Vector3(zoomLevel / 10f, zoomLevel / 10f, 1f);
                // change the player's scaled based on the minimap
            }
            // zoom the minimaps and players in and out
        }
    }

    private void FixedUpdate() {
        // ~50 times per second, no matter the machine
        Vector2 center = new Vector2(Mathf.RoundToInt(player.transform.position.x * (mapScale / chunkGenerator.noiseScale)), Mathf.Round(player.transform.position.y * (mapScale / chunkGenerator.noiseScale)));
        // get the position of the player from the actual map relative to the minimap
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, seed, mapScale, octaves, persistence, lacunarity, center);
        float[,] biomeHueMap = Noise.GenerateNoiseMap(mapSize, seed + 1, 5 * mapScale, 3, 0.5f, 1.5f, center);
		float[,] biomeValueMap = Noise.GenerateNoiseMap(mapSize, seed + 2, 5 * mapScale, 3, 0.5f, 1.5f, center);
		float[,] biomeSaturationMap = Noise.GenerateNoiseMap(mapSize, seed + 3, 5 * mapScale, 3, 0.5f, 1.5f, center);
        float[,] hueMap = Noise.GenerateHueMap(mapSize, seed - 2, mapScale, 0.25f, center);        
        // create the necessary heightmaps
        Color[] colorMap = new Color[mapSize * mapSize];
        // create an array to place colors into
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                for (int i = 0; i < chunkGenerator.regions.Length; i++) {
                    float currentHeight = noiseMap[x,y];
                    float H, S, V;
                    if (currentHeight <= chunkGenerator.regions[i].height) {
                        Color newColor = chunkGenerator.regions[i].color;
                        if (currentHeight <= 0.2 && hueMap[x, y] > 0) {
                            Color.RGBToHSV(newColor, out H, out S, out V);
                            newColor = Color.HSVToRGB(H - hueMap[x, y] / 12f, S, V);
                        }
                        else if (currentHeight > 0.2) {
                            Color.RGBToHSV(newColor, out H, out S, out V);
                            newColor = Color.HSVToRGB(
                                H + ((biomeHueMap[x,y] + 1) / 2) / 2.333333f,
                                S + (biomeSaturationMap[x,y] - 0.2f) / 1f,
                                V + (biomeValueMap[x,y] - 0.4f) / 4f
                            );
                        }
                        colorMap[y * mapSize + x] = newColor;
                        break;
                    }
                }
            }
        }
        // same loop as in chunkgenerator, just without the dithering and hard coded in variables
        Texture2D texture = new Texture2D(mapSize, mapSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, mapSize, mapSize), origin, 1f, 0u, SpriteMeshType.FullRect);
        // same as chunkgenerator
    }
}
