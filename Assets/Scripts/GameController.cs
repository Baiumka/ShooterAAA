using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameManager gameManager;    

    private void Awake()
    {
        gameManager = new GameManager();
        gameManager.onPlayerSpawned += SpawnPlayer;
    }

    private void SpawnPlayer(Player player)
    {
        if(Spawner.Instance == null)
        {
            Debug.LogError("Spawner is not init.");
            return;
        }
        Spawner.Instance.SpawnPlayer(player);
    }

    private void Start()
    {
        gameManager.SpawnPlayer();
    }
}
