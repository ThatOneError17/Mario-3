using UnityEngine;

public class DeathZone : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Destroy(collision.gameObject);
        

        else
        {
            GameManager.Instance.Lives--;
            GameManager.Instance.Respawn();
        }
    }
}
