using UnityEngine;

public class Cloud : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        // when the cloud collides with the bounding box that follows the camera
        int posRand = Random.Range(1, 5);
        // get a random from 1-4
        if (posRand == 1) { 
            // relocate the cloud to the player's north wall view, based on the current position
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + Random.Range(-17, 17), FindObjectOfType<CloudGenerator>().transform.position.y + 14f);
        }
        else if (posRand == 2) { 
            // relocate the cloud to the player's east wall view, based on the current position
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + 17f, FindObjectOfType<CloudGenerator>().transform.position.y + Random.Range(-14f, 14f));
        }
        else if (posRand == 3) { 
            // relocate the cloud to the player's south wall view, based on the current position
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + Random.Range(-17, 17), FindObjectOfType<CloudGenerator>().transform.position.y + -14f);
        }
        else { 
            // relocate the cloud to the player's west wall view, based on the current position
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + -17f, FindObjectOfType<CloudGenerator>().transform.position.y + Random.Range(-14f, 14f));
        }
    }
}
