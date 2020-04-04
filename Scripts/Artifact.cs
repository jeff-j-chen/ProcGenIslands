using UnityEngine;

public class Artifact : MonoBehaviour {
    public int artifactNum;
    // artifacts: increase max player speed, increase movement increase rate, increase rotation speed, decrease rotational decay
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            if (artifactNum == 0) {
                other.gameObject.GetComponent<Player>().maxSpeed += 5f;
                FindObjectOfType<ArtifactGenerator>().SetStatusText("max speed up!");
            }
            else if (artifactNum == 1) {
                other.gameObject.GetComponent<Player>().moveIncreaseRate += 0.25f;
                FindObjectOfType<ArtifactGenerator>().SetStatusText("acceleration up!");
            }
            else if (artifactNum == 2) {
                other.gameObject.GetComponent<Player>().rotSpeed += 1f;
                FindObjectOfType<ArtifactGenerator>().SetStatusText("rotational speed up!");
            }
            else if (artifactNum == 3) {
                other.gameObject.GetComponent<Player>().rotVelDecay -= 0.001f;
                if (other.gameObject.GetComponent<Player>().rotVelDecay < 0) { other.gameObject.GetComponent<Player>().rotVelDecay = 0; }
                FindObjectOfType<ArtifactGenerator>().SetStatusText("rotational penalty down!");
            }
            FindObjectOfType<SoundManager>().PlayClip("artifactPickup");
            FindObjectOfType<ArtifactGenerator>().GenerateArtifact(0);
            Destroy(gameObject);
        }
    }
}
