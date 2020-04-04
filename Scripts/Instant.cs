using UnityEngine;

public class Instant : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Chunk(Clone)") {
            FindObjectOfType<ChunkGenerator>().centerChunk = other.gameObject;
            // used to instantly set the new center chunk upon collision, used to avoid a very annoying bug
        }
    }
}
