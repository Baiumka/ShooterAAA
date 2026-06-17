
using System;
using System.Collections.Generic;

public delegate void VoidHandler();
public delegate void PlayerHandler(Player player);
public delegate void EnemyHandler(Enemy player);
public delegate void EnemyPlayerHandler(Enemy enemy, Player player);
public delegate void WeaponHandler(Weapon weapon);

public class GameManager
{
    private Player player;
    private List<Weapon> weaponsModels;
    private List<Enemy> enemiesList;
    private Vec3? playerSpawnPoint;
    private List<Vec3> enemySpawnPoint;
    private int enemyPointCounter;
    
    public PlayerHandler onPlayerSpawned;
    public EnemyHandler onEnemySpawned;


    public void GenerateEnemySpawnPoints(List<Vec3> spawnPoints)
    {
        enemySpawnPoint = spawnPoints;
    }

    public void GeneratePlayerSpawnPoint(Vec3? spawnPoint)
    {
        playerSpawnPoint = spawnPoint;
    }


    public void GenerateWeapons()
    {
        Weapon pistol = new Weapon(WeaponName.PISTOL, 7, 7, 0.15f, false);
        weaponsModels = new List<Weapon>();
        weaponsModels.Add(pistol);
    }

    public void SpawnPlayer()
    {
        if(!playerSpawnPoint.HasValue)
        {
            throw new Exception("Have no player`s spawn point.");
        }

        player = new Player(1, playerSpawnPoint.Value);
        onPlayerSpawned?.Invoke(player);

        player.GiveWeapon(weaponsModels[0].Clone());
    }

    public void SpawnEnemies(int count)
    {
        if (enemySpawnPoint == null)
        {
            throw new Exception("Have no enemy`s spawn point.");
        }
        if (enemiesList == null) enemiesList = new List<Enemy>();

        for (int i = 0; i < count; i++)
        {
            Vec3 pos = enemySpawnPoint[enemyPointCounter];
            enemyPointCounter++;
            if(enemyPointCounter > enemySpawnPoint.Count -1)
            {
                enemyPointCounter = 0;
            }
            Enemy newEnemy = new Enemy(enemiesList.Count, pos.x, pos.y, pos.z);
            enemiesList.Add(newEnemy);
            onEnemySpawned?.Invoke(newEnemy);
            newEnemy.GiveWeapon(weaponsModels[0].Clone());
        }
    }


}
