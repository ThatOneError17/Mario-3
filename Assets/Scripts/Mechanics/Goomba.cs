using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]

public class Goomba : Enemy
{

    private Rigidbody2D rb;
    [SerializeField] private float xVel = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xVel <= 0) xVel = 2f;
    }

    public override void TakeDamage(int damageValue, DamageType damageType = DamageType.Default)
    {
        if (damageType == DamageType.JumpedOn)
        {
            xVel = 0f; // Stop movement on squish
            anim.SetTrigger("Squish");

            Destroy(gameObject, 0.5f);

            return;
        }

        base.TakeDamage(damageValue, damageType);

    }

    private void OnTriggerEnter2D(Collider2D collision) //For touching invisble walls
    {
        if (collision.gameObject.CompareTag("LevelBarrier"))
        {
            sr.flipX = !sr.flipX;
        }

        if (collision.gameObject.CompareTag("Barrier"))
        {
            sr.flipX = !sr.flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //For touching physical Objects
    {
        if (collision.gameObject.CompareTag("Objects") || collision.gameObject.CompareTag("Squish"))
        {
            sr.flipX = !sr.flipX;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (sr.flipX) rb.linearVelocity = new Vector2(xVel, rb.linearVelocity.y);
        else rb.linearVelocity = new Vector2(-xVel, rb.linearVelocity.y);

    }
}
