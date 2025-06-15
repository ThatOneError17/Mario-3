using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float lifetime = 5.0f;
    [SerializeField] private float flightDistance = 5f; // Max distance it travels before returning

    private PlayerController pc; // Reference to the PlayerController script
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector3 startPos;
    private Rigidbody2D rb;
    private float travelDistance = 0f;
    private bool returning = false;
    private Vector2 originalVelocity;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        Destroy(gameObject, lifetime); // Destroy the boomerang after lifetime
    }

    public void SetVelocity(Vector2 velocity)
    {
        originalVelocity = velocity;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = velocity;
    }

    private void Update()
    {
        travelDistance = Vector2.Distance(startPos, transform.position);

        if (!returning && travelDistance >= flightDistance)
        {
            rb.linearVelocity = new Vector2(-originalVelocity.x, originalVelocity.y);
            returning = true;
        }
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile collides with an object tagged as "Player"
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);

            //if (pc.isBig)
            //    pc.isBig = false;

            //else
            //{
                GameManager.Instance.Lives--;
                GameManager.Instance.Respawn();
            //}
        }
    }
}

