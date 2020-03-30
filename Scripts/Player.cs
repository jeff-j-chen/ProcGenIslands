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
    public float _particleLifetime;
    // how long (in seconds) each particle should live
    private WaitForSeconds particleLifetime;
    // waitforseconds for the particleLifetime
    public float randOffset;
    // how much to offset the float particles by
    public float particleSpeed;
    // how fast to send the particles backwards
    public float spawnChance;
    // the chance for a particle to spawn
    public float limiter;
    // when to clamp the movement velocity to 0
    public GameObject minimapIcon;
    // the player's icon on the minimap
    public TextMeshProUGUI coordinates;
    // textmeshpro object representing the player's coordinates
    public Camera minimapCamera;

    private void Start() {
        particleLifetime = new WaitForSeconds(_particleLifetime);
        // create the waitforseconds
    }
   
    private void Update() {
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

    private void FixedUpdate() {
        // called a certain # of times per second, rather than per frame. makes it so that movement is fair between computers that run the game at different framerates
        if (!minimapCamera.enabled) {
            // only move if the minimap is off
            // increase the movement velocity when the up/down key is held, up to a cap
            if (Input.GetKey(KeyCode.UpArrow)) {
                moveVel += 0.1f * moveIncreaseRate;
                moveVel = moveVel > maxSpeed ? maxSpeed : moveVel;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                moveVel -= 0.075f * moveIncreaseRate;
                moveVel = moveVel < -maxSpeed / 3 ? -maxSpeed / 3 : moveVel;
            }
            if (-limiter <= moveVel && moveVel <= limiter) { moveVel = 0f; }
            // instantly set movement velocity to 0 if within the limits (+- 0.1 works well)
            else {
                // don't clamp, so decrease it down to 0
                if (moveVel > 0) { moveVel -= 0.05f * moveIncreaseRate; }
                else { moveVel += 0.05f * moveIncreaseRate; }
            }

            Vector3 newRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + curRotSpeed);
            // create a new euler angle based on the rotation input
            transform.eulerAngles = newRotation;
            minimapIcon.transform.eulerAngles = newRotation;
            // assign the rotation to the player and the player's minimap icon
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveVel * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180f), moveVel * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180f));
            // create a vector based on the movement velocity and angle, then apply it to the rigidbody
            if (Random.Range(0, spawnChance) <= Mathf.Abs(Mathf.RoundToInt(moveVel)) + 1) {
                InstantiateParticle();
            }
            // instantiate a particle based on player speed
            coordinates.text = $"x: {Mathf.Round(transform.position.x)}\ny: {Mathf.Round(transform.position.y)}";
            // update the coordinate text 
        }
        else {
            // if minimap camera is on, stop movement immediately
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }
    
    public void InstantiateParticle() {
        GameObject createdParticle = Instantiate(waterParticle, new Vector2(transform.position.x + (Random.value * 2 - 1) * randOffset, transform.position.y + (Random.value * 2 - 1) * randOffset), Quaternion.identity);
        StartCoroutine(DestroyParticle(createdParticle));
        if (moveVel != 0f) {
            createdParticle.GetComponent<Rigidbody2D>().velocity = new Vector2(-particleSpeed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180f  + (Random.value * 2 - 1) * randOffset), -particleSpeed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180f + (Random.value * 2 - 1) * randOffset));
        }
    }

    public IEnumerator DestroyParticle(GameObject particle) {
        yield return particleLifetime;
        Destroy(particle);
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
