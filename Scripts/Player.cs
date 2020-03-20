using UnityEngine;

public class Player : MonoBehaviour {
    public int viewDist;
    public float playerSpeed;
    public bool touchingWater = false;
    public Rigidbody2D rb;
    Vector2 movement;
    public float maxSpeed;
   
    private void FixedUpdate() {
        // rudimentary player to test movement

        if (touchingWater == false)
        {
            rb.velocity = new Vector2(movement.x * playerSpeed, rb.velocity.y);
            rb.MovePosition(rb.position + movement * playerSpeed * Time.fixedDeltaTime);
        }
        if (touchingWater == true)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                maxSpeed = maxSpeed < 50 ? maxSpeed + 25 : maxSpeed;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(maxSpeed, 0f));
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                maxSpeed = maxSpeed < 50 ? maxSpeed + 25 : maxSpeed;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-maxSpeed, 0f));
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                maxSpeed = maxSpeed < 50 ? maxSpeed + 25 : maxSpeed;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, maxSpeed));
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                maxSpeed = maxSpeed < 50 ? maxSpeed + 25 : maxSpeed;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -maxSpeed));
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
    }

    public bool PointInViewDist(Vector2 point) {
        return (Mathf.Sqrt(Mathf.Pow(transform.position.x - point.x, 2) + Mathf.Pow(transform.position.y - point.y, 2)) < viewDist);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        FindObjectOfType<ChunkGenerator>().centerChunk = other.gameObject;
    }
}
