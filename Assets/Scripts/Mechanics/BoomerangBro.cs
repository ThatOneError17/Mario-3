using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]

public class BoomerangBro : Enemy
{
    private Rigidbody2D rb;
    [SerializeField] private float xVel = 0;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private float jumpInterval = 0f;
    [SerializeField] private float projectileFireRate = 0f;
    public Transform player;
    private float shotTimer;
    private float jumpTimer;
    private float groundCheckRadius = 0.02f;
    private LayerMask isGroundLayer;
    private bool isGrounded;
    private int moveDir = 1; // 1 for right, -1 for left

    GroundCheck groundCheck;


    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        Debug.Log("BoomerangBro received player reference.");
        player = playerInstance.transform;
        return playerInstance;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xVel <= 0f) xVel = 0.5f;
        if (jumpForce <= 0f) jumpForce = 4f;
        if (jumpInterval <= 0f) jumpInterval = 8f;
        if (projectileFireRate <= 0f) projectileFireRate = 8f;

        GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;



        groundCheck = new GroundCheck(LayerMask.GetMask("Ground"), GetComponent<Collider2D>(), rb, ref groundCheckRadius);

        //Setting all "Ground" as layermask
        isGroundLayer = LayerMask.GetMask("Ground");

        if (GameManager.Instance.PlayerInstance != null)
        {
            player = GameManager.Instance.PlayerInstance.transform;
        }

    }

    public override void TakeDamage(int damageValue, DamageType damageType = DamageType.Default)
    {
        if (damageType == DamageType.JumpedOn)
        {
            xVel = 0f; // Stop movement on squish

            Destroy(gameObject);

            return;
        }

        base.TakeDamage(damageValue, damageType);

    }

    private void OnTriggerEnter2D(Collider2D collision) //For touching invisble walls
    {

        if (collision.gameObject.CompareTag("Barrier"))
        {
            if (moveDir == -1)
                moveDir = 1; // Change direction to right

            else
                moveDir = -1; // Change direction to left

        }
    }


    void Throw()
    {
        if (shotTimer >= projectileFireRate)
        {
            anim.SetTrigger("Throw");
            shotTimer = 0;

        }
    }

    // Update is called once per frame
    private void Update()
    {

        groundCheck.CheckIsGrounded();
        isGrounded = groundCheck.IsGrounded;

        rb.linearVelocity = new Vector2(moveDir * xVel, rb.linearVelocity.y);

        jumpTimer += Time.deltaTime;

        shotTimer += Time.deltaTime;

        Throw();

        if (jumpTimer >= jumpInterval && isGrounded) // Jump Logic
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpTimer = 0;
        }


        if (player.position.x > transform.position.x)
        {
            //Debug.Log("Player is to the right of BoomerangBro");
            sr.flipX = true;
        }

        else if (player.position.x < transform.position.x)
        {
            //Debug.Log("Player is to the left of BoomerangBro");
            sr.flipX = false;
        }

    }
}
