using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate PlayerController PlayerControllerDelegate(PlayerController playerInstance);
    public event PlayerControllerDelegate OnPlayerControllerCreated;

    public static bool isPaused = false; //Will change if game is paused
    public static bool endOfLevel = false; //Will chnage if end of level is reached

    public CanvasManager cm; //Reference to the CanvasManager for UI handling
    public PlayerController pc; //Reference to the PlayerController script

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
    private bool isBig = false;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string loadedSceneName;

            if (currentSceneName == "Game")
            {

                Debug.Log("Exiting to Title Scene");
                // If we are in the Game scene, we exit to the Title scene
                loadedSceneName = "Title";
                SceneManager.LoadScene(loadedSceneName);

            }
        }

        else if (Input.GetKeyDown(KeyCode.E))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string loadedSceneName;

            if (currentSceneName == "Title")
            {
                Debug.Log("Entering Game");
                loadedSceneName = "Game";
                isPaused = false; //When changing scenes will make sure the game is no longer paused
                endOfLevel = false;
                pc.isBig = false; //Resetting the player size
                coins = 0;
                Time.timeScale = 1; //Will also set timescale back to 1 

                SceneManager.LoadScene(loadedSceneName);

            }
        }
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

