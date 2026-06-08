using System;
using UnityEngine;

public partial class GameController : MonoBehaviour
{
    private GameManager gameManager;
    private Player player;


    private void Awake()
    {
        gameManager = new GameManager();
        gameManager.onPlayerSpawned += SpawnPlayer;
    }

    private void OnDestroy()
    {
        gameManager.onPlayerSpawned -= SpawnPlayer;
        gameManager = null;
    }

    private void SpawnPlayer(Player player)
    {
        if(Spawner.Instance == null)
        {
            Debug.LogError("Spawner is not init.");
            return;
        }
        this.player = player;
        Spawner.Instance.SpawnPlayer(player);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameManager.GenerateWeapons();
        gameManager.SpawnPlayer();
    }


}
