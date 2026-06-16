
public class Enemy: Target
{
    public Vec3 SpawnPoint { get; private set; }

    public Enemy(int id, float spawnX, float spawnY, float spawnZ)
    {
        SpawnPoint = new Vec3(spawnX, spawnY, spawnZ);
        Id = id;
        MaxHealth = 100;
        Health = 100;
        Name = GenerateName();
    }
}
