using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    private void OnDestroy()
    {
        GameController.instance.onPlayerSpawned -= SpawnPlayer;
        GameController.instance.onEnemySpawned -= SpawnEnemy;
    }

    private void Awake()
    {         
        if(Instance != null) Destroy(Instance);
        Instance = this;
        GameController.instance.onPlayerSpawned += SpawnPlayer;
        GameController.instance.onEnemySpawned += SpawnEnemy;
    }

    private void SpawnEnemy(Enemy enemy)
    {
        Vec3 spawn = enemy.SpawnPoint;
        GameObject enemyObj = GameObject.Instantiate(enemyPrefab, new Vector3(spawn.x, spawn.y, spawn.z), new Quaternion());
        EnemyReplica enemyReplica = enemyObj.GetComponent<EnemyReplica>();
        enemyReplica.Init(enemy);
    }

    
    public void SpawnPlayer(Player player)
    {
        GameObject playerObj = GameObject.Instantiate(playerPrefab);
        PlayerReplica playerReplica = playerObj.GetComponent<PlayerReplica>();
        playerReplica.Init(player);
    }
}
