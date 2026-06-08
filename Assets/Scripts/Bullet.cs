using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifeTime = 5f;
    private float timer;

    public void Init(Vector3 target, float bulletSpeed)
    {
        speed = bulletSpeed;
        direction = (target - transform.position).normalized;
        timer = lifeTime;        
        transform.forward = direction;
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;

        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}