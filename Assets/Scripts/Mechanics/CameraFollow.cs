using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;

    public Transform playerRef;

    void Awake()
    {
        //Subscribing to the GameManager event to get the player reference
        GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;

    }

    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        playerRef = playerInstance.transform;
        return playerInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerRef) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(playerRef.position.x, minXPos, maxXPos);
        transform.position = pos;

        Vector3 posy = transform.position;
        posy.y = Mathf.Clamp(playerRef.position.y, minYPos, maxYPos);
        transform.position = posy;


    }
}
