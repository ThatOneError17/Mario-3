using UnityEngine;

//RequireComponent can only have 3
[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer), typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    [Range(3, 10)]
    public float speed = 6.0f;

    [Range(1, 20)]
    public float jumpForce = 10f;

    [Range(0.01f, 0.2f)]
    public float groundCheckRadius = 0.02f;
    public LayerMask isGroundLayer;
    public bool isGrounded;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;
    private Animator anim;

    private Vector2 groundCheckPos => new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();


        //Setting all "Ground" as a layermask
        isGroundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIsGrounded();

        float hInput = Input.GetAxis("Horizontal");

        float attack = Input.GetAxis("Fire1");



        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);

        //Flips if hInput is less than 0, and does not equal 0, therefore it won't automatically flip to default when no input is detected
        if (hInput != 0) spriteRenderer.flipX = (hInput < 0);



        anim.SetBool("isGrounded", CheckIsGrounded());
        anim.SetBool("isWalking", checkIsWalking());


        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


        //Checks if grounded and returns true or false
        bool CheckIsGrounded()
        {
            //Checks if it's grounded, if false we enter
            if (!isGrounded)
            {
                //if we're not jumping and touching ground, will return a true
                if (rb.linearVelocityY <= 0)
                {
                    return Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, isGroundLayer);
                }

            }
            //Will return a true if grounded
            return isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, isGroundLayer);
        }

        //Checks if walking and returns true or false
        bool checkIsWalking()
        {

            if (Mathf.Abs(hInput) > 0)
            {
                return true;
            }

            else
            {
                return false;
            }

        }
    }
}
