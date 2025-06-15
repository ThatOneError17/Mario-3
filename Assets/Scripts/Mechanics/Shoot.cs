using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]

public class Shoot : MonoBehaviour
{

    private SpriteRenderer sr;
    private Animator anim;


    [SerializeField] private Vector2 initShotVelocity = Vector2.zero;
    [SerializeField] private Transform throwRight;
    [SerializeField] private Transform throwLeft;
    [SerializeField] private Projectile boomerangPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (initShotVelocity == Vector2.zero)
        {
            initShotVelocity = new Vector2(5f, 0f); // Default shot velocity
        }
    }

    public void Fire()
    {

        Projectile curProjectile;

        if (sr.flipX)
        {

            curProjectile = Instantiate(boomerangPrefab, throwRight.position, Quaternion.identity);
            curProjectile.SetVelocity(initShotVelocity);
        }
        else
        {
            curProjectile = Instantiate(boomerangPrefab, throwLeft.position, Quaternion.identity);
            Vector2 flippedVelocity = new Vector2(-initShotVelocity.x, initShotVelocity.y);
            curProjectile.SetVelocity(flippedVelocity);
        }
        anim.SetTrigger("noThrow");
    }
}
