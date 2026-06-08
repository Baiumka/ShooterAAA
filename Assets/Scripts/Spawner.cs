using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {         
        if(Instance != null) Destroy(Instance);
        Instance = this;
        GameController.instance.onPlayerSpawned += SpawnPlayer;
    }

    private void OnDestroy()
    {
        GameController.instance.onPlayerSpawned -= SpawnPlayer;
    }

    public void SpawnPlayer(Player player)
    {
        GameObject playerObj = GameObject.Instantiate(playerPrefab);
        PlayerReplica playerReplica = playerObj.GetComponent<PlayerReplica>();
        playerReplica.Init(player);
    }
}
