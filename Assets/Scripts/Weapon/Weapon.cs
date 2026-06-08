using UnityEngine;

public class Weapon 
{
   

    public WeaponName name;
    public int ammo;
    public int maxAmmo;
    public float delay;
    public bool auto;
    public float maxDistance;
    public float buleltSpeed;

    public VoidHandler onShot;

    public Weapon(WeaponName name, int ammo, int maxAmmo, float delay, bool auto)
    {
        this.ammo = ammo;
        this.maxAmmo = maxAmmo;
        this.delay = delay;
        this.auto = auto;
        this.name = name;
        this.maxDistance = 1000;
        this.buleltSpeed = 15;
    }

    public bool Shot()
    {
        if (ammo > 0)
        {
            ammo--;
            onShot?.Invoke();
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
