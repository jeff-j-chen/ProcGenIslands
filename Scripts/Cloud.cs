using UnityEngine;

public class Cloud : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        int posRand = Random.Range(1, 5);
        if (posRand == 1) { 
            // north
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + Random.Range(-17, 17), FindObjectOfType<CloudGenerator>().transform.position.y + 14f);
        }
        else if (posRand == 2) { 
            // east
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + 17f, FindObjectOfType<CloudGenerator>().transform.position.y + Random.Range(-14f, 14f));
        }
        else if (posRand == 3) { 
            // south
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + Random.Range(-17, 17), FindObjectOfType<CloudGenerator>().transform.position.y + -14f);
        }
        else { 
            // west
            transform.position = new Vector2(FindObjectOfType<CloudGenerator>().transform.position.x + -17f, FindObjectOfType<CloudGenerator>().transform.position.y + Random.Range(-14f, 14f));
        }
    }
}
