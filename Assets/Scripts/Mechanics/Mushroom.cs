using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private float speed = 2.0f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    private int moveDirection = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LevelBarrier") || collision.gameObject.CompareTag("Objects") || collision.gameObject.CompareTag("Enemy"))
        {
            if (moveDirection == 1)
            {
                moveDirection = -1; // Change direction to left
            }
            else
            {
                moveDirection = 1; // Change direction to right
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            if (moveDirection == 1)
            {
                moveDirection = -1; // Change direction to left
            }
            else
            {
                moveDirection = 1; // Change direction to right
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);

    }

    
}