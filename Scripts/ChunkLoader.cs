
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChunkLoader : MonoBehaviour {
    public ChunkGenerator chunkGenerator;
    public CloudGenerator cloudGenerator;
    
    // mapgenerator object
    public float chunkUpdateDelay = 1f;
    // delay between chunk updates
    private WaitForSeconds waitTime;
    // assigned at runtime
    private Player player;
    // the player
    public GameObject cloud;
    public GameObject cloud2;
    private List<Vector2> chunkPositions = new List<Vector2>();
    private List<Vector2> testPositions = new List<Vector2>();
    private float lastPlayerX;
    private float lastPlayerY;
   


    private void Awake() {

        // cloud = cloudGenerator.GenerateCloudAt(new Vector2(0, 0), cloud,1324324,50,1f,0f);
        //cloud2 = cloudGenerator.GenerateCloudAt(new Vector2(0, 0),cloud2,10,50,-.2f,0f);
        // generate a cloud and store it
        chunkGenerator.GenerateChunkAt(new Vector2(0, 0));
        // generate a new chunk at the center (testing purposes only)
        waitTime = new WaitForSeconds(chunkUpdateDelay);
        // create the wait time
        player = FindObjectOfType<Player>();
        // get the player object
        StartCoroutine(UpdateChunks());
        // start updating chunks

    }

    private IEnumerator UpdateChunks() { 
        while (true) {
            // repeat forever
            lastPlayerX = player.transform.position.x;
            lastPlayerY = player.transform.position.y;
            yield return waitTime;
            if (lastPlayerX != player.transform.position.x || lastPlayerY != player.transform.position.y) {
                // first check if playre has moved since last check
                chunkPositions.Clear();
                for (int i = 0; i < chunkGenerator.chunks.Count; i++) {
                    chunkPositions.Add(new Vector2(chunkGenerator.chunks[i].transform.position.x, chunkGenerator.chunks[i].transform.position.y));
                    if (!player.PointInViewDist(chunkPositions[i])) {
                        // if the chunk position is not within the player's viewing radius
                        chunkGenerator.chunks[i].GetComponent<SpriteRenderer>().sprite = null;
                        Destroy(chunkGenerator.chunks[i]);
                        chunkGenerator.chunks.RemoveAt(i);
                        // destroy the chunk at that position
                    }
                } 
                GenerateNewTestPositions();
                // get a list of the chunk positions
                for (int i = 0; i < testPositions.Count; i++) {
                    // for every test position (for i faster than foreach) 
                    if (!chunkPositions.Contains(testPositions[i]) && player.PointInViewDist(testPositions[i])) {
                        // if the test position is not already loaded and is within the player's view distance
                        GameObject createdChunk = chunkGenerator.GenerateChunkAt(testPositions[i]);
                        // create the chunk at that point
                    }
                }
            }
        }
    }

    private bool PlayerIsInChunk(GameObject chunk) {
        return chunk.transform.position.x - 25f < player.transform.position.x && player.transform.position.x < chunk.transform.position.x + 25f && chunk.transform.position.y - 25f < player.transform.position.y && player.transform.position.y < chunk.transform.position.y + 25f;
    }

    private void GenerateNewTestPositions() {
        testPositions.Clear();
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x - 50f, chunkGenerator.centerChunk.transform.position.y + 50f));
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x, chunkGenerator.centerChunk.transform.position.y + 50f));
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x + 50f, chunkGenerator.centerChunk.transform.position.y + 50f));
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x - 50f, chunkGenerator.centerChunk.transform.position.y));
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x + 50f, chunkGenerator.centerChunk.transform.position.y));
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x - 50f, chunkGenerator.centerChunk.transform.position.y - 50f));
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x, chunkGenerator.centerChunk.transform.position.y - 50f));
        testPositions.Add(new Vector2(chunkGenerator.centerChunk.transform.position.x + 50f, chunkGenerator.centerChunk.transform.position.y - 50f));
    }
}
