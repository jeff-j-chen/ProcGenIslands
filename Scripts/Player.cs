using UnityEngine;

public class Player : MonoBehaviour {
    public int viewDist;
    public float playerSpeed;

    private void Start() {}

    private void FixedUpdate() {
        // rudimentary player to test movement
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position = new Vector2(transform.position.x, transform.position.y + playerSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.position = new Vector2(transform.position.x, transform.position.y - playerSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.position = new Vector2(transform.position.x - playerSpeed * Time.fixedDeltaTime, transform.position.y);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position = new Vector2(transform.position.x + playerSpeed * Time.fixedDeltaTime, transform.position.y);
        }
    }

    public bool PointInViewDist(Vector2 point) {
        return (Mathf.Sqrt(Mathf.Pow(transform.position.x - point.x, 2) + Mathf.Pow(transform.position.y - point.y, 2)) < viewDist);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        FindObjectOfType<MapGenerator>().centerChunk = other.gameObject;
    }
}
