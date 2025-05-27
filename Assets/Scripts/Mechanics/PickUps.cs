using UnityEngine;

public class PickUps : MonoBehaviour
{
    public enum PickupType
    {
        Life,
        Mushroom,
        Leaf,
        Coin,

    }

    public PickupType type;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case PickupType.Life:
                    GameManager.Instance.Lives++;
                    break;

                case PickupType.Mushroom:
                    break;

                case PickupType.Leaf:
                    break;

                case PickupType.Coin:
                    // Increment the player's coin count
                    GameManager.Instance.Coins++;
                    break;

            }

            Destroy(gameObject);

        }
    }
}
