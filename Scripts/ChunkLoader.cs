using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour {
    public ChunkGenerator chunkGenerator;
    // mapgenerator object
    public float chunkUpdateDelay = 1f;
    // delay between chunk updates
    private WaitForSeconds waitTime;
    // assigned at runtime
    private Player player;
    // the player
    private List<Vector2> testPositions = new List<Vector2>();
    // lists used for checking if we need to spawn a chunk at a location 
    private float lastPlayerX;
    private float lastPlayerY;
    // floats telling us where the player was last, used to help performance

    private void Awake() {
        chunkGenerator.GenerateChunkAt(new Vector2(25.5f, 25.5f));
        chunkGenerator.GenerateChunkAt(new Vector2(-24.5f, 25.5f));
        chunkGenerator.GenerateChunkAt(new Vector2(-24.5f, -24.5f));
        chunkGenerator.GenerateChunkAt(new Vector2(25.5f, -24.5f));
        // generate chunks in a square around the player. use .5 so that player is aligned with the center rather than at the side
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
            // get the player's last location
            yield return waitTime;
            if (lastPlayerX != player.transform.position.x || lastPlayerY != player.transform.position.y) {
                // first check if player has moved since last check, only then do we update
                GenerateNewTestPositions();
                // get a list of the chunk positions
                for (int i = 0; i < testPositions.Count; i++) {
                    // for every test position (for i faster than foreach) 
                    if (!chunkGenerator.chunkPositions.Contains(testPositions[i]) && player.PointInViewDist(testPositions[i])) {
                        // if the test position is not already loaded and is within the player's view distance
                        GameObject createdChunk = chunkGenerator.GenerateChunkAt(testPositions[i]);
                        // create the chunk at that point
                    }
                }
            }
        }
    }
    
    private void GenerateNewTestPositions() {
        // makes a list of potential positions to spawn chunks in
        testPositions.Clear();
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x - 50f, chunkGenerator.centerChunk.transform.position.y + 50f, 0f));
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x, chunkGenerator.centerChunk.transform.position.y + 50f, 0f));
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x + 50f, chunkGenerator.centerChunk.transform.position.y + 50f, 0f));
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x - 50f, chunkGenerator.centerChunk.transform.position.y, 0f));
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x + 50f, chunkGenerator.centerChunk.transform.position.y, 0f));
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x - 50f, chunkGenerator.centerChunk.transform.position.y - 50f, 0f));
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x, chunkGenerator.centerChunk.transform.position.y - 50f, 0f));
        testPositions.Add(new Vector3(chunkGenerator.centerChunk.transform.position.x + 50f, chunkGenerator.centerChunk.transform.position.y - 50f, 0f));
    }
}
