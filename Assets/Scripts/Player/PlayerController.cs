using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;
using static Enemy;

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
    private float pSpeedTimeLimit = 0.5f;
    private float pSpeed;
    private float pSpeedTimer;
    private float pSpeedTimerDecrement;
    public bool isBig = false; //Will change if player is big or not, used for powerups


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

        //Setting all "Ground" as layermask
        isGroundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {


        if (GameManager.isPaused)
            return; //Should ignore all other update related functions if game is paused

        if (GameManager.endOfLevel)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("pSpeed", false);
            anim.SetBool("isGrounded", true);
            return; //Should remove control if level is over
        }

        //For animation stuff, gettinfo from base layer of animations, which we only have 0
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float hInput = Input.GetAxis("Horizontal");

        float run = Input.GetAxis("Fire3");

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


        if (checkIsRunning() && checkIswalking())
        {
            if (pSpeed >= 4)
            {
                anim.SetBool("pSpeed", true);
            }

            else
            {
                anim.SetBool("pSpeed", false);
            }
            if (speed <= 11)
            {
                //If speed is less than 9, build up speed
                if (speed < 9)
                {
                    speed += 0.1f;
                }
                anim.SetBool("IsRunning", (checkIsRunning()));
                //If anim bool "isGrounded is true, will build up p momentum
                if (anim.GetBool("isGrounded"))
                {
                    pSpeedTimer += Time.deltaTime;
                }
                //If speed is less than or equal to 8, and is grounded, and p speed timer is greater or equal to the pSpeedTime needed, will increase speed and pSpeed
                if (speed >= 8 && anim.GetBool("isGrounded") && pSpeedTimer >= pSpeedTimeLimit)
                {
                    Debug.Log("Resetting Timer");
                    pSpeedTimer = 0;
                    //If pSpeed is less than or equal to 8, keep incrementing
                    if (pSpeed <= 8)
                    {
                        Debug.Log("Gaining pSpeed");
                        pSpeed += 1;
                    }
                    //If pSpeed is greater than 4, and Speed is less than 10, increase speed by 1
                    if (pSpeed >= 4 && speed < 10)
                    {
                        speed += 1f;
                    }

                }
            }

           
        }

        

       

        else
        {
            //If speed is still greater than 6, but not running, decrement speed, but keep running animation
            if (speed > 6f)
            {
                speed -= 0.1f;
                
                anim.SetBool("IsRunning", (checkIsRunning()));
            }
            //If not running, starts losing pSpeed momentum
            if (pSpeedTimer > 0)
            {
                pSpeedTimer -= Time.deltaTime;
            }
            
            
            
            if (pSpeed < 4)
            {
                anim.SetBool("pSpeed", false);
            }

        }
        //If off ground starts decrementing pSpeed
        if (anim.GetBool("isGrounded") == false)
        {
            pSpeedTimerDecrement += Time.deltaTime;
            if (pSpeed >= 4)
            {
                
                if (pSpeedTimerDecrement >= 4f && pSpeed > 0)
                {
                    pSpeedTimerDecrement = 0;
                    pSpeed -= 1;
                }
            }

            else
            {
                if (pSpeedTimerDecrement >= 1f && pSpeed > 0)
                {
                    pSpeedTimerDecrement = 0;
                    pSpeed -= 1;
                }
            }
        }

        else
        {
            //If speed is less than 7, starts decrementing pSpeed
            if (speed < 7 && pSpeed > 0)
            {
                pSpeedTimerDecrement += Time.deltaTime;
                if (pSpeedTimerDecrement >= pSpeedTimeLimit)
                {
                    pSpeedTimerDecrement = 0;
                    pSpeed -= 1;
                }
            }
        }

        //Checks if walking and returns true or false
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

        //Checks if Running and returns true or false
        bool checkIsRunning()
        {
            if (run > 0f)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

    }


        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Squish") && rb.linearVelocityY < 0)
            {
                collision.enabled = false;
                collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(1, DamageType.JumpedOn);
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            }

        }

    }