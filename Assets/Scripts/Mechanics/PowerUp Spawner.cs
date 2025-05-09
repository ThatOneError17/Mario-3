using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] PickUpPreFabs;

    public void spawnPowerUp(bool Super)
    {
        if (Super)
        {
            Instantiate(PickUpPreFabs[1], transform.position, transform.rotation);
        }

        else
        {
            Instantiate(PickUpPreFabs[0], transform.position, transform.rotation);
        }
    }
}