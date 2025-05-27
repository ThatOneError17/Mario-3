using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate PlayerController PlayerControllerDelegate(PlayerController playerInstance);
    public event PlayerControllerDelegate OnPlayerControllerCreated;

    #region Singleton Pattern
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    #region Player Controller Info
    [SerializeField] private PlayerController playerPrefab;
    private PlayerController playerInstance;
    public PlayerController PlayerInstance => playerInstance;
    private Vector3 levelStart;
    #endregion

    #region Game Stats
    public int maxLives = 3;
    private int lives = 3;
    private int coins = 0;
    public int Coins
    {
        get 
        { 
            return coins; 
        }

        set
        {
            coins = value;
            Debug.Log("Coins have been set to: " + coins);

            if (coins >= 100)
            {
                // Increment lives when coins reach 100
                Lives++;
                coins -= 100; // Reset coins after gaining a life
                Debug.Log("Gained a life! Total lives: " + lives);
            }
        }

        

    }

    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            if (value < 0)
            {
               //GameOver();
                return;
            }
            if (lives > value)
            {
                Respawn();
            }
            lives = value;
            Debug.Log("Lives have been set to: " + lives);
        }
    }
    #endregion

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (maxLives <= 0)
        {
            Debug.LogError("Max lives must be greater than 0.");
            maxLives = 3; // Default value
        }

        Lives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantatePlayer(Vector3 spawnPos)
    {
        playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        levelStart = spawnPos;
        OnPlayerControllerCreated?.Invoke(playerInstance);
    }

    public void Respawn()
    {
        playerInstance.transform.position = levelStart;
    }
}

