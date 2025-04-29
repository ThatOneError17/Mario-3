using UnityEditor.Tilemaps;
using UnityEngine;

//RequireComponent can only have 3
[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(Collider2D))]

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
    private Animator anim;

    GroundCheck groundCheck;

    //private Vector2 groundCheckPos => new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y);
    //private Transform groundCheckTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        groundCheck = new GroundCheck(LayerMask.GetMask("Ground"), GetComponent<Collider2D>(), rb, ref groundCheckRadius);


        //Setting all "Ground" as a layermask
        //isGroundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        //For animation stuff, gettinfo from base layer of animations, which we only have 0
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float hInput = Input.GetAxis("Horizontal");

        groundCheck.CheckIsGrounded();

      

        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);

        //Flips if hInput is less than 0, and does not equal 0, therefore it won't automatically flip to default when no input is detected
        if (hInput != 0) spriteRenderer.flipX = (hInput < 0);


        anim.SetBool("isWalking", checkIswalking());
       

        anim.SetBool("isGrounded", groundCheck.IsGrounded);


        if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }



        //Checks if "running" and returns true or false
        bool checkIswalking()
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