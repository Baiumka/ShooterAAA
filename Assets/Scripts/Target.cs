using System;

public abstract class Target
{
    public int Id;
    public int Health;
    public int MaxHealth;

    public Weapon weapon;

    public float runSpeed = 9;
    public float croachSpeed = 1;
    public float normalSpeed = 5;
    public float jumpStrength = 2;

    public bool isRun;
    public bool isCrouch;
    public bool canWalk = true;
    public bool IsAlive => Health > 0;
    public Vec2 InputMove { get; protected set; }

    public WeaponHandler onWeaponEquip;
    public WeaponHandler onWeaponDrop;

    public VoidHandler onStartCrouch;
    public VoidHandler onStopCrouch;
    public VoidHandler onStartRun;
    public VoidHandler onStopRun;
    public VoidHandler onJump;

    public void GiveWeapon(Weapon weapon)
    {
        if (this.weapon != null)
        {
            DropWeapon();
        }
        this.weapon = weapon;
        onWeaponEquip?.Invoke(weapon);
    }

    private void DropWeapon()
    {
        onWeaponDrop?.Invoke(this.weapon);
        this.weapon = null;
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health < 0)
            Health = 0;
    }

    public void ApplyCrouch(bool v)
    {
        if (isCrouch == v) return;
        isCrouch = v;
        if (isCrouch) onStartCrouch?.Invoke();
        else onStopCrouch?.Invoke();
    }

    public void ApplyRun(bool v)
    {
        if (isRun == v) return;
        if (isCrouch) return;
        isRun = v;
        if (isRun) onStartRun?.Invoke();
        else onStopRun?.Invoke();
    }

    public void MakeJump()
    {
        onJump?.Invoke();
    }

    public void DoShot()
    {
        weapon.Shot();
    }

    public void ReloadWeapon()
    {
        if (weapon == null) return;
        weapon.Reload();
    }
}