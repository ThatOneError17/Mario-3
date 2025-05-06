using UnityEngine;


public class Coin : MonoBehaviour
{

    private int coins;


public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log(coins);
            coins++;
            Debug.Log(coins);
        }
    }

}

