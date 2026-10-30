using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Update()
    {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) horizontal -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) horizontal += 1f;
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.Space)) && grounded)
        {
            Jump(jumpForce);
        }

        if (transform.position.y < -8f) Respawn();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                grounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) => grounded = false;

    public void Jump(float force)
    {
        grounded = false;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    public void Bounce(float force) => Jump(force);

    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = startPosition;
    }
}
