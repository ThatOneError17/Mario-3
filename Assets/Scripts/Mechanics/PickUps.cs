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
                    break;

                case PickupType.Mushroom:
                    break;

                case PickupType.Leaf:
                    break;

                case PickupType.Coin:
                    break;

            }

            Destroy(gameObject);

        }
    }
}
