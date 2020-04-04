using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ArtifactGenerator : MonoBehaviour {
    // generate a chunk at a random location (50 * maxchunkoutdist)
    // if there is land, generate on land
    // if not, destroy and try again
    // create an indicator on the draggable minimap, maybe on the normal minimap as well?
    // artifacts: increase max player speed, increase movement increase rate, increase rotation speed, decrease rotational decay
    // 4 total
    public int maxChunksForGeneration;
    private ChunkGenerator chunkGenerator;
    public GameObject artifactPrefab;
    public Sprite[] artifactSprites;
    public TextMeshProUGUI artifactText;
    
    WaitForSeconds waitTime;

    private void Start() {
        chunkGenerator = FindObjectOfType<ChunkGenerator>();
        waitTime = new WaitForSeconds(1f);
        GenerateArtifact(0);
    }

    public void GenerateArtifact(int increment) {
        StartCoroutine(GenerateArtifactCoroutine(increment));
    }

    private IEnumerator GenerateArtifactCoroutine(int increment) {
        if (chunkGenerator.centerChunk != null) {
            Vector2 testPosition = new Vector2(chunkGenerator.centerChunk.transform.position.x + chunkGenerator.chunkSize * Random.Range(-maxChunksForGeneration - increment, maxChunksForGeneration + 1 + increment), chunkGenerator.centerChunk.transform.position.y + chunkGenerator.chunkSize * Random.Range(-maxChunksForGeneration - increment, maxChunksForGeneration + 1 + increment));
            // get a chunk within a set x y chunk limit of the player's position
            if (!chunkGenerator.chunkPositions.Contains(testPosition)) {
                // if the chunk hasn't already been generated
                GameObject newChunk = chunkGenerator.GenerateChunkAt(testPosition);
                // generate it 
                if (LandExistsIn(newChunk)) {
                    float[] foundCoords = GetMaxHeightOfChunk(newChunk);
                    // spawn artifact based on text position + foundcoords;
                    GameObject createdArtifact = Instantiate(artifactPrefab, new Vector2(
                        testPosition.x - 25 + foundCoords[0], 
                        testPosition.y - 25 + foundCoords[1]), 
                        Quaternion.identity);
                    createdArtifact.GetComponent<Artifact>().artifactNum = Random.Range(0, 4);
                    createdArtifact.GetComponent<SpriteRenderer>().sprite = artifactSprites[createdArtifact.GetComponent<Artifact>().artifactNum];
                    FindObjectOfType<Minimap>().CreateArtifactOnMinimap(createdArtifact);
                    print("successfully created artifact");
                }
                else {
                    chunkGenerator.chunks.Remove(newChunk);
                    chunkGenerator.chunkPositions.Remove(testPosition);
                    // remove the chunk from the arrays its data was added to
                    Destroy(newChunk);
                    // destroy it
                    yield return waitTime;
                    GenerateArtifact(increment);
                }
            }
            else {
                yield return waitTime;
                GenerateArtifact(increment + 2);
            }
        }
        else {
            yield return waitTime;
            GenerateArtifact(increment);
        }
    }
    
    private bool LandExistsIn(GameObject chunkGO) {
        float[,] noiseMap = chunkGO.GetComponent<Chunk>().noiseMap;
        // get the noisemap of the chunk
        for (int x = 0; x < chunkGenerator.chunkSize; x++) {
            for (int y = 0; y < chunkGenerator.chunkSize; y++) {
                // for every point (x,y)
                if (noiseMap[x,y] > 0.2) {
                    return true;
                }
                // if land, then return true (break out)
            }
        }
        return false;
        // no land found, so return false
    }

    private float[] GetMaxHeightOfChunk(GameObject chunkGO) {
        float[,] noiseMap = chunkGO.GetComponent<Chunk>().noiseMap;
        // get the noisemap of the chunk
        float max = 0.3f;
        float xCoord = 0;
        float yCoord = 0;
        for (int x = 0; x < chunkGenerator.chunkSize; x++) {
            for (int y = 0; y < chunkGenerator.chunkSize; y++) {
                // for every point (x,y)
                if (noiseMap[x,y] > max) {
                    max = noiseMap[x, y];
                    xCoord = x;
                    yCoord = y;
                }
            }
        }
        return new float[] {xCoord, yCoord};
    }

    public void SetStatusText(string text) {
        StartCoroutine(StatusTextCoroutine(text));
    }

    private IEnumerator StatusTextCoroutine(string text) {
        Color temp = artifactText.color;
        temp.a = 0;
        artifactText.color = temp;
        artifactText.text = text;
        for (int i = 0; i < 40; i++) {
            temp.a += 1f / 40f;
            artifactText.color = temp;
            yield return new WaitForSeconds(1f / 80f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 40; i++) {
            temp.a -= 1f / 40f;
            artifactText.color = temp;
            yield return new WaitForSeconds(1f / 80f);
        }
        artifactText.text = "";
    }
}
