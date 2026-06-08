using UnityEngine;

public class Weapon 
{
    public WeaponName name;
    public int ammo;
    public int maxAmmo;
    public float delay;
    public bool auto;

    public Weapon(WeaponName name, int ammo, int maxAmmo, float delay, bool auto)
    {
        this.ammo = ammo;
        this.maxAmmo = maxAmmo;
        this.delay = delay;
        this.auto = auto;
        this.name = name;
    }

    public bool Shot()
    {
        if (ammo > 0)
        {
            ammo--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reload()
    {
        ammo = maxAmmo;
    }
}
