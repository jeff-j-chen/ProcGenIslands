using UnityEngine;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour {
    public int viewDist;
    // how fat away to load chunks
    public float maxSpeed;
    // the maximum speed of the player
    public float moveIncreaseRate;
    // how much to increase the player's movement velocity by
    public float moveVel;
    // the player's current movement velocity
    public float rotSpeed;
    // how fast the player should turn
    public float curRotSpeed;
    // the player's current rotation speed
    public float rotVelDecay;
    // how much the player's movement velocity should decay from moving
    public GameObject waterParticle;
    // the water particle prefab to spawn in on player move
    public float particleLifetime;
    // how long (in seconds) each particle should live
    private WaitForSeconds lifetime;
    // waitforseconds for the lifetime
    public float randOffset;
    // how much to offset the float particles by
    public float particleSpeed;
    // how fast to send the particles backwards
    public float spawnChance;
    // the chance for a particle to spawn
    public GameObject minimapIcon;
    public TextMeshProUGUI coordinates;

    private void Start() {
        lifetime = new WaitForSeconds(particleLifetime);
    }
   
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
            moveVel /= rotVelDecay;
            // set rotation and decay movement velocity by a %
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            curRotSpeed = rotSpeed;
            moveVel /= rotVelDecay;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            curRotSpeed = 0f;
            // upon release of left/right, remove all rotational speed
        }
    }

    public void InstaniateParticle() {
        GameObject createdParticle = Instantiate(waterParticle, new Vector2(transform.position.x + (Random.value * 2 - 1) * randOffset, transform.position.y + (Random.value * 2 - 1) * randOffset), Quaternion.identity);
        StartCoroutine(DestroyParticle(createdParticle));
        createdParticle.GetComponent<Rigidbody2D>().velocity = new Vector2(-particleSpeed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180f  + (Random.value * 2 - 1) * randOffset), -particleSpeed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180f + (Random.value * 2 - 1) * randOffset));
    }

    public IEnumerator DestroyParticle(GameObject particle) {
        yield return lifetime;
        Destroy(particle);
    }
    // reduce speed by % on rotation rather than by flat number

    private void FixedUpdate() {
        // called a certain # of times per second, rather than per frame
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + curRotSpeed);
        transform.eulerAngles = newRotation;
        minimapIcon.transform.eulerAngles = newRotation;
        // create a new euler angle based on the rotation input
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveVel * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180f), moveVel * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180f));
        // create a vector based on the movement velocity and angle, then apply it to the rigidbody
        if (UnityEngine.Random.Range(1, spawnChance) <= Mathf.Abs(Mathf.RoundToInt(moveVel))) {
            InstaniateParticle();
        }
        coordinates.text = $"x: {Mathf.Round(transform.position.x)}\ny: {Mathf.Round(transform.position.y)}";
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
