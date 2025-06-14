using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;
    [SerializeField] protected int maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 1;

        health = maxHealth;
    }

    public virtual void TakeDamage(int DamageValue, DamageType DamageType = DamageType.Default)
    {
        health -= DamageValue;
        if (health <= 0)
        {

            if (DamageType == DamageType.JumpedOn)
                anim.SetTrigger("Squish");   // Play jumped on death animation

            else
                anim.SetTrigger("Death");   // Play default death animation


            if (transform.parent != null)
                Destroy(transform.parent.gameObject, 0.5f);
            else
                Destroy(gameObject, 0.5f);


        }
    }
    public enum DamageType
    {
        Default,
        JumpedOn,
    }
}
