
using System.Collections.Generic;

public delegate void VoidHandler();
public delegate void PlayerHandler(Player player);
public delegate void WeaponHandler(Weapon weapon);

public class GameManager
{
    private Player player;
    private List<Weapon> weaponsModels;
    public PlayerHandler onPlayerSpawned;

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

        player.GiveWeapon(weaponsModels[0]);
    }
}
