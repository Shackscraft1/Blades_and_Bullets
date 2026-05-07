using System.Collections;
using UnityEngine;

public class BossAttackMissiles : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject missilePrefab;

    [Header("Fire Points")]
    public Transform[] launchPoints;

    [Header("Missile Settings")]
    public int missilesPerVolley = 4;
    public float delayBetweenMissiles = 0.4f;
    public float missileSpeed = 4.5f;
    public float turnSpeedDegrees = 220f;
    public float trackingDuration = 2.2f;
    public float explosionRadius = 0.9f;
    public float explosionDuration = 0.45f;
    public float visualScale = 0.45f;

    public IEnumerator Fire(Transform target)
    {
        if (launchPoints == null || launchPoints.Length == 0)
        {
            Debug.LogWarning("BossAttackMissiles: No launch points assigned.");
            yield break;
        }

        for (int i = 0; i < missilesPerVolley; i++)
        {
            Transform launchPoint = launchPoints[i % launchPoints.Length];

            if (missilePrefab != null)
            {
                GameObject missileObject = Instantiate(
                    missilePrefab,
                    launchPoint.position,
                    launchPoint.rotation
                );

                BossHomingMissile missile = missileObject.GetComponent<BossHomingMissile>();
                if (missile != null)
                {
                    missile.Initialize(
                        target,
                        missileSpeed,
                        turnSpeedDegrees,
                        trackingDuration,
                        explosionRadius,
                        explosionDuration
                    );
                }
            }
            else
            {
                CreateFallbackMissile(launchPoint, target);
            }

            yield return new WaitForSeconds(delayBetweenMissiles);
        }
    }

    private void CreateFallbackMissile(Transform launchPoint, Transform target)
    {
        GameObject missileObject = new GameObject("Boss_Missile");
        missileObject.transform.position = launchPoint.position;
        missileObject.transform.localScale = Vector3.one * visualScale;

        SpriteRenderer renderer = missileObject.AddComponent<SpriteRenderer>();
        renderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
        renderer.color = Color.yellow;
        renderer.sortingOrder = 25;

        CircleCollider2D collider = missileObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.25f;

        BossHomingMissile missile = missileObject.AddComponent<BossHomingMissile>();
        missile.Initialize(
            target,
            missileSpeed,
            turnSpeedDegrees,
            trackingDuration,
            explosionRadius,
            explosionDuration
        );
    }
}