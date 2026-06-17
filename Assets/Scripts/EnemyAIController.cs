using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    private Enemy enemy;
    private PlayerReplica playerReplica;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyReplica replica;
    private Vector3 patrolPoint;
    private float searchTimer;
    private float searchDuration = 3f;
    private float lookSpeed = 8f;
    private float meleeCooldown = 1.2f;
    private float meleeTimer;
    private int meleeDamage = 25;
    private float nextShotTime;
    private bool shotDelayInitialized;

    public void Init(Enemy enemy, PlayerReplica playerReplica)
    {
        this.enemy = enemy;
        this.playerReplica = playerReplica;
    }

    private void Update()
    {
        if (enemy == null || playerReplica == null) return;

        float dist = Vector3.Distance(replica.transform.position, playerReplica.Position);

        UpdateState(dist);
        ExecuteState();

        bool seesPlayer = CanSeePlayer();

        if (seesPlayer)
        {
            enemy.lastKnownPlayerPos = Utils.ToModel(playerReplica.Position);
            enemy.lastSeenPlayerTime = Time.time;

            enemy.state = EnemyState.Chase;
        }
    }

    private void UpdateState(float dist)
    {
        bool seesPlayer = dist <= enemy.visionRange;
        bool inAttackRange = dist <= enemy.attackRange;
        bool inMeleeRange = dist <= enemy.meleeRange;

        switch (enemy.state)
        {
            case EnemyState.Idle:
            case EnemyState.Patrol:

                if (seesPlayer)
                    enemy.state = EnemyState.Chase;

                break;

            case EnemyState.Chase:

                if (!seesPlayer)
                    enemy.state = EnemyState.Search;

                if (inMeleeRange)
                    enemy.state = EnemyState.Melee;
                else if (inAttackRange)
                    enemy.state = EnemyState.Attack;

                break;
            case EnemyState.Reload:
                if (enemy.weapon.ammo > 0) enemy.state = EnemyState.Chase;
                break;

            case EnemyState.Attack:

                if (!inAttackRange)
                    enemy.state = EnemyState.Chase;

                if (inMeleeRange)
                    enemy.state = EnemyState.Melee;

                break;

            case EnemyState.Melee:

                if (!inMeleeRange)
                    enemy.state = EnemyState.Chase;

                break;

            case EnemyState.Search:

                if (seesPlayer)
                    enemy.state = EnemyState.Chase;

                break;
        }
    }

    private void ExecuteState()
    {
        switch (enemy.state)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                break;

            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                agent.isStopped = false;
                agent.SetDestination(playerReplica.Position);
                break;

            case EnemyState.Search:
                SearchLastKnownPosition();
                break;

            case EnemyState.Attack:

                agent.isStopped = true;
                LookAtPlayer();

                if (!shotDelayInitialized)
                {
                    nextShotTime = Time.time + UnityEngine.Random.Range(0.15f, 0.35f);
                    shotDelayInitialized = true;
                }
                if (Time.time < nextShotTime)
                    break;
                shotDelayInitialized = false;
                if (enemy.weapon.ammo <= 0)
                {
                    enemy.state = EnemyState.Reload;
                }
                else
                {
                    enemy.DoShot();
                }
                break;

            case EnemyState.Melee:
                agent.isStopped = true;
                LookAtPlayer();
                MeleeAttack();
                break;

            case EnemyState.Reload:
                agent.isStopped = true;                
                enemy.ReloadWeapon();
                break;
        }

       
    }
    private void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 1f)
        {
            patrolPoint = RandomNavmeshPoint(10f);
            agent.SetDestination(patrolPoint);
        }
    }

    private Vector3 RandomNavmeshPoint(float radius)
    {
        Vector3 random = Random.insideUnitSphere * radius + transform.position;
        NavMeshHit hit;

        NavMesh.SamplePosition(random, out hit, radius, NavMesh.AllAreas);

        return hit.position;
    }


    private void SearchLastKnownPosition()
    {
        agent.isStopped = false;
        agent.SetDestination(Utils.ToUnity(enemy.lastKnownPlayerPos));

        float distance = Vector3.Distance(transform.position, Utils.ToUnity(enemy.lastKnownPlayerPos));

        // дошЄл до точки
        if (distance < 1.5f)
        {
            searchTimer += Time.deltaTime;

            // имитаци€ "осмотра"
            transform.Rotate(0, 120f * Time.deltaTime, 0);

            if (searchTimer >= searchDuration)
            {
                searchTimer = 0f;
                enemy.state = EnemyState.Patrol;
            }
        }
        else
        {
            searchTimer = 0f;
        }
    }

    

    private void LookAtPlayer()
    {
        Vector3 dir = playerReplica.Position - transform.position;
        dir.y = 0f; // не наклон€емс€ вверх/вниз

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * lookSpeed
        );
    }

    private void MeleeAttack()
    {
        float dist = Vector3.Distance(transform.position, playerReplica.Position);

        if (dist > enemy.meleeRange + 0.5f)
        {
            enemy.state = EnemyState.Chase;
            return;
        }

        meleeTimer += Time.deltaTime;

        if (meleeTimer < meleeCooldown)
            return;

        meleeTimer = 0f;

        // поворот к игроку перед ударом
        LookAtPlayer();

        // урон
        playerReplica.TakeDamage(meleeDamage);

        // можно триггер анимации
        // animator.SetTrigger("Attack");
    }

    private bool CanSeePlayer()
    {
        Vector3 enemyPos = transform.position;
        Vector3 playerPos = playerReplica.Position;

        Vector3 dir = playerPos - enemyPos;
        float distance = dir.magnitude;

        // 1. проверка дистанции зрени€
        if (distance > enemy.visionRange)
            return false;

        // 2. проверка угла обзора (опционально, но полезно)
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(forward, dir);

        if (angle > 90f) // FOV 180 градусов (можешь уменьшить до 60-70)
            return false;

        // 3. проверка преп€тствий (самое важное)
        if (Physics.Raycast(enemyPos + Vector3.up * 1.6f, dir.normalized, distance))
        {
            return false; // есть преп€тствие
        }

        return true;
    }
}