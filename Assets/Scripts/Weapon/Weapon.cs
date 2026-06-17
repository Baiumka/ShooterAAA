using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Android.AndroidGame;

public class Weapon 
{
    private Stopwatch shotTimer = Stopwatch.StartNew();

    public WeaponName name;
    public int ammo;
    public int maxAmmo;
    public float delay;
    public bool auto;
    public float maxDistance;
    public float buleltSpeed;
    public int reloadDelay;

    public bool isReload;

    public VoidHandler onShot;
    public VoidHandler onStartLoad;
    public VoidHandler onEndLoad;

    public Weapon(WeaponName name, int ammo, int maxAmmo, float delay, bool auto)
    {
        this.ammo = ammo;
        this.maxAmmo = maxAmmo;
        this.delay = delay;
        this.auto = auto;
        this.name = name;
        this.maxDistance = 1000;
        this.buleltSpeed = 15;
        this.reloadDelay = 2000;
    }

    public bool Shot()
    {
        if (isReload) return false;

        if (shotTimer.Elapsed.TotalSeconds < delay)
            return false;

        if (ammo > 0)
        {
            ammo--;
            shotTimer.Restart();

            onShot?.Invoke();
            return true;
        }

        return false;
    }

    public async void Reload()
    {
        await ReloadAsync();
    }

    public async Task ReloadAsync()
    {
        if (isReload || ammo == maxAmmo)
            return;
        isReload = true;
        onStartLoad?.Invoke();
        await Task.Delay(reloadDelay);
        ammo = maxAmmo;
        isReload = false;
        onEndLoad?.Invoke();
    }

    public Weapon Clone()
    {
        return new Weapon(name, ammo, maxAmmo, delay, auto);
    }
}
