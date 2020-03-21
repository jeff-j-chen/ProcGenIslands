using UnityEngine;

public class Player : MonoBehaviour {
    public int viewDist;
    Vector2 movement;
    public float maxSpeed;
    public float moveIncreaseRate;
    public float moveVel;
    public float rotSpeed;
    public float curRotSpeed;
    public float rotVelDecay;
   
    private void Update() {
        // increase the movement velocity when the up/down key is held, up to a cap
        if (Input.GetKey(KeyCode.UpArrow)) {
            moveVel += 0.1f * moveIncreaseRate;
            moveVel = moveVel > maxSpeed ? maxSpeed : moveVel;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            moveVel -= 0.075f * moveIncreaseRate;
            moveVel = moveVel < -maxSpeed / 3 ? -maxSpeed / 3 : moveVel;
        }
        if (moveVel > 0f) { moveVel -= 0.05f * moveIncreaseRate; }
        else { moveVel += 0.05f * moveIncreaseRate; }
        // decay movement velocity towards 0
        if (-0.02f <= moveVel && moveVel <= 0.02f) { moveVel = 0f; }
        // and round it off to 0 when it gets close enough
        // set the rotation speed when the left/right key is held, and decay movement velocity
        if (Input.GetKey(KeyCode.RightArrow)) {
            curRotSpeed = -rotSpeed;
            if (moveVel > 0f) { moveVel -= rotVelDecay * moveIncreaseRate; }
            else { moveVel += rotVelDecay * moveIncreaseRate; }
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            curRotSpeed = rotSpeed;
            if (moveVel > 0f) { moveVel -= rotVelDecay * moveIncreaseRate; }
            else { moveVel += rotVelDecay * moveIncreaseRate; }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            curRotSpeed = 0f;
            // upon release of left/right, remove all rotational speed
        }
    }

    private void FixedUpdate() {
        // called a certain # of times per second, rather than per frame
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + curRotSpeed);
        // create a new euler angle based on the rotation input
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(moveVel * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180f), moveVel * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180f));
        // create a vector based on the movement velocity and angle, then apply it to the rigidbody
    }

    public bool PointInViewDist(Vector2 point) {
        return (Mathf.Sqrt(Mathf.Pow(transform.position.x - point.x, 2) + Mathf.Pow(transform.position.y - point.y, 2)) < viewDist);
        // returns whether or not a point is within the player's view (render) distance
    }

    private void OnTriggerEnter2D(Collider2D other) {
        FindObjectOfType<ChunkGenerator>().centerChunk = other.gameObject;
        // called whenever the player moves into a new chunk
    }
}
