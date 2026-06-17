
public class Enemy: Target
{
    public Vec3 SpawnPoint { get; private set; }

    public float visionRange = 15f;
    public float attackRange = 10f;
    public float meleeRange = 2f;

    public float reactionTime = 0.3f;

    public EnemyState state = EnemyState.Patrol;

    public float lastSeenPlayerTime;
    public Vec3 lastKnownPlayerPos;

    public Enemy(int id, float spawnX, float spawnY, float spawnZ)
    {
        SpawnPoint = new Vec3(spawnX, spawnY, spawnZ);
        Id = id;
        MaxHealth = 100;
        Health = 50;
        Name = GenerateName();
    }


}
