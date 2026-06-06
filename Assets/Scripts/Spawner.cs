using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {           
        Instance = this;           
    }

    public void SpawnPlayer(Player player)
    {
        GameObject playerObj = GameObject.Instantiate(playerPrefab);
        PlayerReplica playerReplica = playerObj.GetComponent<PlayerReplica>();
        playerReplica.Init(player);
    }
}
