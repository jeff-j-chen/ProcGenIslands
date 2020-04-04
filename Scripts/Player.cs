using UnityEngine;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour {
    public int viewDist;
    // how far away to load chunks
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
    // camera showing the entire map
    public bool isOnLand = false;
    // whether or not the player is on land
    private ChunkGenerator chunkGenerator;
    // chunkgenerator script
    public Sprite landSprite;
    // sprite used for the player on land
    public Sprite waterSprite;
    // sprite used for the player on water
    public float _REQUIREDDELAY;
    private WaitForSeconds REQUIREDDELAY;
    // DO NOT DELETE
    private AudioSource[] sfx;
    // wind/water audiosources
    public SoundManager soundManager;
    // soundmanager script
    public float _actionTimer;
    // float for how long the player needs to wait before making another move (land only)
    private WaitForSeconds actionTimer;
    // waitforseconds of _actiontimer, used to save memory
    public bool canMove = true;
    // whether or not the player can move
    public GameObject footstep;
    // the gameobject representing the player's footstep (prefab)

    private void Start() {
        particleLifetime = new WaitForSeconds(_particleLifetime);
        REQUIREDDELAY = new WaitForSeconds(_REQUIREDDELAY);
        actionTimer = new WaitForSeconds(_actionTimer);
        // create the waitforsecond objects
        chunkGenerator = FindObjectOfType<ChunkGenerator>();
        soundManager = FindObjectOfType<SoundManager>();
        // get necessary scripts
        sfx = GetComponents<AudioSource>();
        // get the audiosources
    }
   
    private void Update() {
        if (!isOnLand) {
            // in water
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
        else {
            // on land
            if (!minimapCamera.enabled && canMove) {
                // can't move with the minimap on
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    Instantiate(footstep, new Vector2(transform.position.x + (Random.value - 0.5f) / 5f, transform.position.y + (Random.value - 0.5f) / 5f), Quaternion.identity);
                    transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                    coordinates.text = $"x: {Mathf.Round(transform.position.x)}\ny: {Mathf.Round(transform.position.y)}";
                    soundManager.PlayClip($"walking{Random.Range(0, 4)}");
                    StartCoroutine(LockActions());
                    transform.eulerAngles = new Vector3(0f, 0f, 90f);
                    minimapIcon.transform.eulerAngles = new Vector3(0f, 0f, 90f);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    Instantiate(footstep, new Vector2(transform.position.x + (Random.value - 0.5f) / 5f, transform.position.y + (Random.value - 0.5f) / 5f), Quaternion.identity);
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
                    coordinates.text = $"x: {Mathf.Round(transform.position.x)}\ny: {Mathf.Round(transform.position.y)}";
                    soundManager.PlayClip($"walking{Random.Range(0, 4)}");
                    StartCoroutine(LockActions());
                    transform.eulerAngles = new Vector3(0f, 0f, -90f);
                    minimapIcon.transform.eulerAngles = new Vector3(0f, 0f, -90f);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    Instantiate(footstep, new Vector2(transform.position.x + (Random.value - 0.5f) / 5f, transform.position.y + (Random.value - 0.5f) / 5f), Quaternion.identity);
                    transform.position = new Vector2(transform.position.x - 1f, transform.position.y);
                    coordinates.text = $"x: {Mathf.Round(transform.position.x)}\ny: {Mathf.Round(transform.position.y)}";
                    soundManager.PlayClip($"walking{Random.Range(0, 4)}");
                    StartCoroutine(LockActions());
                    transform.eulerAngles = new Vector3(0f, 0f, 180f);
                    minimapIcon.transform.eulerAngles = new Vector3(0f, 0f, 180f);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    Instantiate(footstep, new Vector2(transform.position.x + (Random.value - 0.5f) / 5f, transform.position.y + (Random.value - 0.5f) / 5f), Quaternion.identity);
                    transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
                    coordinates.text = $"x: {Mathf.Round(transform.position.x)}\ny: {Mathf.Round(transform.position.y)}";
                    soundManager.PlayClip($"walking{Random.Range(0, 4)}");
                    StartCoroutine(LockActions());
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    minimapIcon.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }
                // leave behind a footstep at the player's location (with a slight offset)
                // move 1 unit based on player input
                // update coordinate text
                // play a random walking sound clip
                // start coroutine to lock the player's actions
                // set the player and minimap icon's rotations based on the player's input
            }
        }
        StartCoroutine(SetMovementMethod());
        // begin setting the movement method and sprite (water/land)
        SetSfxVolume();
        // set the volume of the sound effects based on the player's speed and height
    }

    private IEnumerator LockActions() {
        canMove = false;
        // prevent player from moving
        yield return actionTimer;
        // wait until the timer is done
        canMove = true;
        // player can move again
    }

    private void FixedUpdate() {
        // called a certain # of times per second, rather than per frame. makes it so that movement is fair between computers that run the game at different framerates (if i remember correctly)
        if (!minimapCamera.enabled) {
            // only move if the minimap is off
            if (!isOnLand) {
                // if the player is in the water
                if (Input.GetKey(KeyCode.UpArrow)) {
                    moveVel += 0.1f * moveIncreaseRate;
                    moveVel = moveVel > maxSpeed ? maxSpeed : moveVel;
                }
                else if (Input.GetKey(KeyCode.DownArrow)) {
                    moveVel -= 0.075f * moveIncreaseRate;
                    moveVel = moveVel < -maxSpeed / 3 ? -maxSpeed / 3 : moveVel;
                }
                // increase the movement velocity when the up/down key is held, up to a cap
                if (-limiter <= moveVel && moveVel <= limiter) { moveVel = 0f; }
                // instantly set movement velocity to 0 if within the limits (+- 0.1 works well)
                else {
                    // don't clamp, so decrease it down to 0
                    if (moveVel > 0) { moveVel -= 0.05f * moveIncreaseRate; }
                    else { moveVel += 0.05f * moveIncreaseRate; }
                }

                Vector3 newRotation = new Vector3(0f, 0f, transform.eulerAngles.z + curRotSpeed);
                // create a new euler angle based on the rotation input
                transform.eulerAngles = newRotation;
                minimapIcon.transform.eulerAngles = newRotation;
                // assign the rotation to the player and the player's minimap icon
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveVel * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180f), moveVel * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180f));
                // create a vector based on the movement velocity and angle, then apply it to the rigidbody
                if (UnityEngine.Random.Range(0, spawnChance) <= Mathf.Abs(Mathf.RoundToInt(moveVel)) + 1) {
                    InstantiateParticle();
                }
                // instantiate a particle based on player speed
                coordinates.text = $"x: {Mathf.Round(transform.position.x)}\ny: {Mathf.Round(transform.position.y)}";
                // update the coordinate text 
            }
        }
        else {
            // if minimap camera is on
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            // stop movement immediately
        }
    }

    private float GetHeightAtPos() {
        int x;
        // blank integers
        int roundedX = Mathf.RoundToInt(transform.position.x - 1);
        // get the player's position, rounded to the nearest position (and taking off one)
        int y;
        // blank integers
        int roundedY = Mathf.RoundToInt(transform.position.y - 1);
        // get the player's position, rounded to the nearest position (and taking off one)
        if (roundedX > 0) { x = roundedX % 50; }
        else if (roundedX == 0) { x = 0; }
        else { x = 49 + ((roundedX + 1) % 50); }
        // get the player's x position within a chunk based on the current position
        if (roundedY > 0) { y = roundedY % 50; }
        else if (roundedY == 0) { y = 0; }
        else { y = 49 + ((roundedY + 1) % 50); }
        // get the player's y position within a chunk based on the current position
        return chunkGenerator.centerChunk.GetComponent<Chunk>().noiseMap[x, y];
        // get the noisemap height at that point
    }

    private IEnumerator SetMovementMethod() {
        float height = GetHeightAtPos();
        // get the height at the position
        yield return REQUIREDDELAY;
        // WAIT so there isn't a bug
        if (height == GetHeightAtPos()) {
            // check that the height has not bugged
            if (height >= 0.2f) {
                // if the player is now on land
                if (!isOnLand) {
                    // if the player was not previously on land
                    transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                    // set the player to be on the land
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    // remove all rigidbody velocity
                    moveVel = 0;
                    curRotSpeed = 0;
                    // remove all player velocity
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    minimapIcon.transform.eulerAngles = new Vector3(0, 0, 0);
                    // reset transform
                    GetComponent<SpriteRenderer>().sprite = landSprite;
                    // set the player's sprite based on location
                }
                isOnLand = true;
                // change bool to represent where the player is
            }
            else {
                if (isOnLand) {
                    transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                    GetComponent<SpriteRenderer>().sprite = waterSprite;
                    // set the player's sprite based on location
                }
                // player is in water
                isOnLand = false;
                // set bool
            }
        }
    }

    private void SetSfxVolume() {
        if (isOnLand) {
            sfx[0].volume = 0f; // water
            sfx[1].volume = 0.2f; // wind
        }
        else {
            sfx[0].volume = (moveVel / maxSpeed) / 10f + 0.025f; // water
            sfx[1].volume = (moveVel / maxSpeed) / 5f + 0.2f; // wind
        }
    }
    
    public void InstantiateParticle() {
        GameObject createdParticle = Instantiate(waterParticle, new Vector2(transform.position.x + (UnityEngine.Random.value * 2 - 1) * randOffset, transform.position.y + (UnityEngine.Random.value * 2 - 1) * randOffset), Quaternion.identity);
        // create a particle at the player's position, with a slight offset
        StartCoroutine(DestroyParticle(createdParticle));
        // destroy the particle after a delay
        if (moveVel != 0f) {
            // if the player is not still
            createdParticle.GetComponent<Rigidbody2D>().velocity = new Vector2(-particleSpeed * Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180f  + (UnityEngine.Random.value * 2 - 1) * randOffset), -particleSpeed * Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180f + (UnityEngine.Random.value * 2 - 1) * randOffset));
            // give the particle a little bit of movement in the opposite direction (with slight directional offset)
        }
    }

    public IEnumerator DestroyParticle(GameObject particle) {
        yield return particleLifetime;
        // wait how long each particle should last
        Destroy(particle);
        // then destroy it
    }

    public bool PointInViewDist(Vector2 point) {
        return (Mathf.Sqrt(Mathf.Pow(transform.position.x - point.x, 2) + Mathf.Pow(transform.position.y - point.y, 2)) < viewDist);
        // returns whether or not a point is within the player's view (render) distance
    }
}
