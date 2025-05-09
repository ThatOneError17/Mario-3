using UnityEngine;

public class DeathZone : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
