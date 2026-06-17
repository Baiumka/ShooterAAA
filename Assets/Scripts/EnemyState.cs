public enum EnemyState
{
    Idle,        // стоит / патруль
    Patrol,      // движение по NavMesh
    Chase,       // преследование игрока
    Attack,      // стрельба
    Melee,       // нож
    Reload,      // перезарядка
    Search       // потерял игрока
}