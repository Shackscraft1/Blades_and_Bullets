using System.Collections;
using UnityEngine;

public class BossAttackMachineGun : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Fire Point")]
    public Transform firePoint;

    [Header("Bullet Settings")]
    public int bulletsPerBurst = 14;
    public float shotInterval = 0.08f;
    public float bulletSpeed = 8f;
    public float spreadAngle = 7f;
    public float bulletLifetime = 4f;
    public float bulletScale = 0.22f;

    public IEnumerator Fire(Transform target)
    {
        if (firePoint == null)
        {
            Debug.LogWarning("BossAttackMachineGun: Fire point is not assigned.");
            yield break;
        }

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            Vector3 direction = GetDirectionToTarget(target);
            direction = Quaternion.Euler(0f, 0f, Random.Range(-spreadAngle, spreadAngle)) * direction;

            if (bulletPrefab != null)
            {
                GameObject bulletObject = Instantiate(
                    bulletPrefab,
                    firePoint.position,
                    Quaternion.identity
                );

                BossProjectile projectile = bulletObject.GetComponent<BossProjectile>();
                if (projectile != null)
                {
                    projectile.Initialize(direction, bulletSpeed, bulletLifetime);
                }
            }
            else
            {
                CreateFallbackBullet(direction);
            }

            yield return new WaitForSeconds(shotInterval);
        }
    }

    private Vector3 GetDirectionToTarget(Transform target)
    {
        if (target == null)
        {
            return Vector3.down;
        }

        Vector3 direction = target.position - firePoint.position;
        direction.z = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return Vector3.down;
        }

        return direction.normalized;
    }

    private void CreateFallbackBullet(Vector3 direction)
    {
        GameObject bulletObject = new GameObject("Boss_Bullet");
        bulletObject.transform.position = firePoint.position;
        bulletObject.transform.localScale = Vector3.one * bulletScale;

        SpriteRenderer renderer = bulletObject.AddComponent<SpriteRenderer>();
        renderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
        renderer.color = Color.white;
        renderer.sortingOrder = 25;

        CircleCollider2D collider = bulletObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.2f;

        BossProjectile projectile = bulletObject.AddComponent<BossProjectile>();
        projectile.Initialize(direction, bulletSpeed, bulletLifetime);
    }
}