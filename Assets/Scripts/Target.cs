public abstract class Target
{
    public int Id;
    public int Health;
    public int MaxHealth;

    public float runSpeed = 9;
    public float croachSpeed = 1;
    public float normalSpeed = 5;
    public float jumpStrength = 2;

    public bool IsAlive => Health > 0;

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health < 0)
            Health = 0;
    }
}