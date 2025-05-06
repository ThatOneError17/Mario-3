using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private RestartGame rg;

    private void Start()
    {
        rg = GetComponent<RestartGame>();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        //restartLevel();
    }
}
