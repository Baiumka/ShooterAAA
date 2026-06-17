using System;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    private PlayerReplica playerReplica; 

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
        EnemyAIController ai = enemyObj.GetComponent<EnemyAIController>();
        ai.Init(enemy, playerReplica);
    }

    
    public void SpawnPlayer(Player player)
    {
        GameObject playerObj = GameObject.Instantiate(playerPrefab, new Vector3(player.SpawnPoint.x, player.SpawnPoint.y, player.SpawnPoint.z), new Quaternion());
        playerReplica = playerObj.GetComponent<PlayerReplica>();
        playerReplica.Init(player);
    }
}
