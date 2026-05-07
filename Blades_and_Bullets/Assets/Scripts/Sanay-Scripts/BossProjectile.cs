using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifetime;

    public void Initialize(Vector3 moveDirection, float moveSpeed, float projectileLifetime)
    {
        direction = moveDirection.normalized;
        speed = moveSpeed;
        lifetime = projectileLifetime;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            Destroy(gameObject);
        }
    }
}