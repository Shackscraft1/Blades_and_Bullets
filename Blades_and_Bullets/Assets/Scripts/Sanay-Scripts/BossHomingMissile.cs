using UnityEngine;

public class BossHomingMissile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float turnSpeedDegrees;
    private float trackingDuration;
    private float explosionRadius;
    private float explosionDuration;

    private float trackingTimer;
    private Vector3 moveDirection = Vector3.down;

    public void Initialize(
        Transform targetTransform,
        float moveSpeed,
        float turnSpeed,
        float trackingTime,
        float radius,
        float duration)
    {
        target = targetTransform;
        speed = moveSpeed;
        turnSpeedDegrees = turnSpeed;
        trackingDuration = trackingTime;
        explosionRadius = radius;
        explosionDuration = duration;

        Destroy(gameObject, trackingDuration + 4f);
    }

    private void Update()
    {
        trackingTimer += Time.deltaTime;

        if (target != null && trackingTimer <= trackingDuration)
        {
            Vector3 desiredDirection = target.position - transform.position;
            desiredDirection.z = 0f;

            if (desiredDirection.sqrMagnitude > 0.001f)
            {
                moveDirection = Vector3.RotateTowards(
                    moveDirection,
                    desiredDirection.normalized,
                    turnSpeedDegrees * Mathf.Deg2Rad * Time.deltaTime,
                    0f
                );
            }
        }

        transform.position += moveDirection.normalized * speed * Time.deltaTime;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject explosion = new GameObject("Boss_Missile_Explosion");
        explosion.transform.position = transform.position;
        explosion.transform.localScale = Vector3.one * explosionRadius;

        SpriteRenderer renderer = explosion.AddComponent<SpriteRenderer>();
        renderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
        renderer.color = new Color(1f, 0.45f, 0f, 0.6f);
        renderer.sortingOrder = 28;

        Destroy(explosion, explosionDuration);
        Destroy(gameObject);
    }
}