using UnityEngine;

public class Instant : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        FindObjectOfType<ChunkGenerator>().centerChunk = other.gameObject;
    }
}
