using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] PickUpPreFabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int prefabIndex = 0;
    public bool canSpawn = true;
    private AudioSource audioSource;

    public AudioClip powerupSpawn;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void spawnPowerUp()
    {
        Instantiate(PickUpPreFabs[prefabIndex], spawnPoint.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canSpawn)
        {
            spawnPowerUp();
            audioSource.PlayOneShot(powerupSpawn);
            canSpawn = false; // Prevent spawning again until reset
        }
    }
}
