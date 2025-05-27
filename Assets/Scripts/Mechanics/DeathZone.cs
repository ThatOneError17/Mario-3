using UnityEngine;

public class DeathZone : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.Lives--;
        GameManager.Instance.Respawn();
    }
}
