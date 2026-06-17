using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class SpawnPoint : MonoBehaviour
{
    [Header("Visual")]    
    private float radius = 0.5f;
    [SerializeField] private int id = 0;
    [SerializeField] private SpawnType type;

    public SpawnType Type { get => type; }
    public int Id { get => id; }
    public Vec3 Vec { get => new Vec3(transform.position.x, transform.position.y, transform.position.z); }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = type == SpawnType.ENEMY_SPAWN ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, radius * 1.2f);
        Handles.color = Color.white;
        Handles.Label(transform.position + Vector3.up * (radius + 0.2f), type == SpawnType.ENEMY_SPAWN ? $"Spawn {id}" : "Player Spawn");
#endif
    }
}