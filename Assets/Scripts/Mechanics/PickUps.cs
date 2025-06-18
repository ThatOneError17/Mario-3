using UnityEngine;

public class PickUps : MonoBehaviour
{

    [SerializeField] private AudioClip pickupSound;

    public enum PickupType
    {
        Life,
        Mushroom,
        Leaf,
        Coin,

    }

    private PlayerController pc;

    public PickupType type;

    void Start()
    {
        pc = GameManager.Instance.PlayerInstance;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (pickupSound)
            {
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
            }

            switch (type)
           
            {
                case PickupType.Life:
                    GameManager.Instance.Lives++;
                    break;

                case PickupType.Mushroom:
                    pc.isBig = true;

                    Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
                    Mushroom mushroom = transform.parent.GetComponent<Mushroom>();
                    mushroom.sr.enabled = false;
                    mushroom.GetComponent<Collider2D>().enabled = false;
                    Destroy(transform.parent.gameObject, pickupSound.length);

                    break;

                case PickupType.Leaf:
                    break;

                case PickupType.Coin:
                    // Increment the player's coin count
                    GameManager.Instance.Coins++;

                    Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
                    GetComponent<SpriteRenderer>().enabled = false;
                    Destroy(gameObject, pickupSound.length);
                    break;

            }

            
        }
    }
}
