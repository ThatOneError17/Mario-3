using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System;
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
    public bool isAlive = true; //Will change if player is alive or not, used for death and respawn
    private bool isDying = false; //Should make it so player can't die multiple times in a row, used for death and respawn


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private AudioSource audioSource;
    private AudioSource musicSource; //Audio source for music, should be on main camera


    [Header("Audio Clips")]
    public AudioClip deathSound;
    public AudioClip stompSound;
    public AudioClip pSpeedSound;
    public AudioClip jumpSound;
    public AudioClip finish;


    GroundCheck groundCheck;

    //private Vector2 groundCheckPos => new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y);
    //private Transform groundCheckTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        musicSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();



        groundCheck = new GroundCheck(LayerMask.GetMask("Ground"), GetComponent<Collider2D>(), rb, ref groundCheckRadius);

        //Setting all "Ground" as layermask
        isGroundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
            anim.SetBool("Alive", true); //Sets alive to true if player is alive, used for death and respawn
        else
            anim.SetBool("Alive", false); //Sets alive to false if player is dead, used for death and respawn

        if (GameManager.isPaused)
            return; //Should ignore all other update related functions if game is paused

        if (GameManager.endOfLevel)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
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

        if (isBig)
            anim.SetBool("isBig", true);
        else
            anim.SetBool("isBig", false);


        if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpSound);
        }

        if(anim.GetBool("pSpeed"))
        {
            audioSource.PlayOneShot(pSpeedSound, 0.2f); //Plays pSpeed sound if pSpeed is true
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
            if (collision.CompareTag("Squish") && rb.linearVelocityY < 0 && isAlive)
            {
                if(isDying) return; //If player is already dying they shouldn't bounce on enemies
                audioSource.PlayOneShot(stompSound);
                collision.enabled = false;
                collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(1, DamageType.JumpedOn);
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            }

            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (isBig)
                {
                    isBig = false; //If player is big, will shrink
                }
                else
                {
                    Death();
                }
            }

            if (collision.gameObject.CompareTag("DeathZone"))
                isBig = false; //If player falls into death zone, will shrink

        if (collision.CompareTag("Finish"))
        {
            musicSource.Pause(); //Pauses music when player reaches finish
            audioSource.PlayOneShot(finish);
            StartCoroutine(DelayedEndOfLevel()); //Starts coroutine to wait before ending level

        }

    }

        public void OnCollisionEnter2D(Collision2D collision)  //Loses big Mario if big, or dies otherwise
        {
            if (collision.gameObject.CompareTag("Enemy") && isAlive)
            {
                if (isBig)
                {
                    isBig = false; //If player is big, will shrink
                }
                else
                {
                    Death();
                }
            }
        }


    private void Death()
    {
        if (isDying) return; //If player is already dying, will not die again
        isAlive = false; //Sets isAlive to false, so player can't kill enemies
        isDying = true; //Sets isDying to true, so player can't die again until respawned
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GameManager.endOfLevel = true; //Sets end of level to true, so player can't move anymore
        rb.linearVelocity = new Vector2(0f, 4f);
        anim.SetTrigger("Death");
        audioSource.PlayOneShot(deathSound);

        StartCoroutine(DelayedRespawn());

    }

    public void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = true;
        isAlive = true; //Sets isAlive to true, so player can kill things again
        isDying = false; //Sets isDying to false, so player can die again
        GameManager.Instance.Respawn();
    }

    private IEnumerator DelayedRespawn() //Should be used to wait for stuff, will come in handy more later
    {
        yield return new WaitForSeconds(3.2f); // Wait before respawn

        Respawn();
    }

    private IEnumerator DelayedEndOfLevel() 
    {
        yield return new WaitForSeconds(3.2f); // Wait before ending level
        levelEnd();
    }   

    private void levelEnd()
    {
        GameManager.Instance.GameOver();
    }

}