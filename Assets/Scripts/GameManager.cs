using UnityEngine;

public delegate void VoidHandler();
public delegate void PlayerHandler(Player player);

public class GameManager
{
    private Player player;
    public PlayerHandler onPlayerSpawned;

    public void SpawnPlayer()
    {
        player = new Player(1);
        onPlayerSpawned?.Invoke(player);
    }
}
