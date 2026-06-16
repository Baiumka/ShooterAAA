
using System;
using System.Collections.Generic;

public delegate void VoidHandler();
public delegate void PlayerHandler(Player player);
public delegate void EnemyHandler(Enemy player);
public delegate void WeaponHandler(Weapon weapon);

public class GameManager
{
    private Player player;
    private List<Weapon> weaponsModels;
    private List<Enemy> enemiesList;
    public PlayerHandler onPlayerSpawned;
    public EnemyHandler onEnemySpawned;

    public void GenerateWeapons()
    {
        Weapon pistol = new Weapon(WeaponName.PISTOL, 7, 7, 1, false);
        weaponsModels = new List<Weapon>();
        weaponsModels.Add(pistol);
    }

    public void SpawnPlayer()
    {
        player = new Player(1);
        onPlayerSpawned?.Invoke(player);

        player.GiveWeapon(weaponsModels[0].Clone());
    }

    public void SpawnEnemies(int count)
    {
        if(enemiesList == null) enemiesList = new List<Enemy>();
        Random rand = new Random();

        float minX = 0f;
        float maxX = 10f;        

        float minZ = 00f;
        float maxZ = 10f;

        for (int i = 0; i < count; i++)
        {
            float x = (float)(rand.NextDouble() * (maxX - minX) + minX);;
            float z = (float)(rand.NextDouble() * (maxZ - minZ) + minZ);

            Enemy newEnemy = new Enemy(enemiesList.Count, x, 1, z);
            enemiesList.Add(newEnemy);
            onEnemySpawned?.Invoke(newEnemy);
            newEnemy.GiveWeapon(weaponsModels[0].Clone());
        }
    }


}
