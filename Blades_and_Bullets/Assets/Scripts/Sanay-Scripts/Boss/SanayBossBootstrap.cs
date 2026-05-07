using System.Collections;
using UnityEngine;

public class SanayBossBootstrap : MonoBehaviour
{
    public static SanayBossBootstrap Instance { get; private set; }

    [SerializeField] private Vector3 bossSpawnPosition = new Vector3(-3f, 3.4f, 0.8f);
    [SerializeField] private float bossEntryHeight = 3.2f;
    [SerializeField] private bool disableWavesObject = true;
    [SerializeField] private bool spawnOnStartForTesting = false;
    [SerializeField] private string wavesObjectName = "Waves";
    [SerializeField] private string enemyContainerName = "EnemyContainer";
    [SerializeField] private string bossName = "SANAYBOSS_Runtime";

    private GameObject activeBoss;
    private bool hasSpawnedBoss;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        if (spawnOnStartForTesting)
        {
            StartBossEncounter();
        }
    }

    public void StartBossEncounter()
    {
        if (hasSpawnedBoss)
        {
            return;
        }

        hasSpawnedBoss = true;
        StartCoroutine(SetupBossPrototype());
    }

    private IEnumerator SetupBossPrototype()
    {
        GameObject existingBoss = GameObject.Find(bossName);
        if (existingBoss != null)
        {
            Destroy(existingBoss);
        }

        Transform parent = GameObject.Find(enemyContainerName)?.transform ?? transform;
        Transform playerTarget = Player.Instance != null ? Player.Instance.transform : FindObjectOfType<Player>()?.transform;

        GameObject bossRoot = new GameObject(bossName);
        bossRoot.transform.SetParent(parent, false);
        bossRoot.transform.position = bossSpawnPosition + Vector3.up * bossEntryHeight;
        bossRoot.transform.localScale = new Vector3(1.6f, 1.6f, 1f);

        SpriteRenderer bossRenderer = bossRoot.AddComponent<SpriteRenderer>();
        bossRenderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
        bossRenderer.color = new Color(0.8f, 0.1f, 0.2f, 1f);
        bossRenderer.sortingOrder = 20;

        BoxCollider2D bossCollider = bossRoot.AddComponent<BoxCollider2D>();
        bossCollider.isTrigger = true;
        bossCollider.size = new Vector2(1f, 1f);

        BossHealth health = bossRoot.AddComponent<BossHealth>();
        health.maxHealth = 300f;

        Transform missileLeft = CreatePoint("MissileLeft", bossRoot.transform, new Vector3(-0.8f, 0.15f, 0f));
        Transform missileRight = CreatePoint("MissileRight", bossRoot.transform, new Vector3(0.8f, 0.15f, 0f));
        Transform laserPoint = CreatePoint("LaserPoint", bossRoot.transform, new Vector3(0f, -0.05f, 0f));
        Transform machineGunPoint = CreatePoint("MachineGunPoint", bossRoot.transform, new Vector3(0f, -0.35f, 0f));

        BossAttackMissiles missiles = bossRoot.AddComponent<BossAttackMissiles>();
        missiles.launchPoints = new[] { missileLeft, missileRight };
        missiles.missilesPerVolley = 4;
        missiles.delayBetweenMissiles = 0.4f;
        missiles.missileSpeed = 4.5f;
        missiles.turnSpeedDegrees = 220f;
        missiles.trackingDuration = 2.2f;
        missiles.explosionRadius = 0.9f;
        missiles.explosionDuration = 0.45f;
        missiles.visualScale = 0.45f;

        BossAttackLaser laser = bossRoot.AddComponent<BossAttackLaser>();
        laser.firePoint = laserPoint;
        laser.beamLength = 11f;
        laser.chargeDuration = 1.1f;
        laser.fireDuration = 0.8f;
        laser.warningWidth = 0.15f;
        laser.beamWidth = 0.42f;

        BossAttackMachineGun machineGun = bossRoot.AddComponent<BossAttackMachineGun>();
        machineGun.firePoint = machineGunPoint;
        machineGun.bulletsPerBurst = 14;
        machineGun.shotInterval = 0.08f;
        machineGun.bulletSpeed = 8f;
        machineGun.spreadAngle = 7f;
        machineGun.bulletLifetime = 4f;
        machineGun.bulletScale = 0.22f;

        BossController controller = bossRoot.AddComponent<BossController>();
        controller.Initialize(
            playerTarget,
            missiles,
            laser,
            machineGun,
            bossSpawnPosition,
            bossSpawnPosition + Vector3.up * bossEntryHeight
        );

        activeBoss = bossRoot;

        yield return null;

        if (disableWavesObject)
        {
            GameObject waves = GameObject.Find(wavesObjectName);
            if (waves != null)
            {
                waves.SetActive(false);
            }
        }
    }

    private Transform CreatePoint(string pointName, Transform parent, Vector3 localPosition)
    {
        GameObject point = new GameObject(pointName);
        point.transform.SetParent(parent, false);
        point.transform.localPosition = localPosition;
        return point.transform;
    }
}

public class BossController : MonoBehaviour
{
    [SerializeField] private float introDuration = 1.5f;
    [SerializeField] private float spawnDelay = 0.4f;
    [SerializeField] private float pauseBetweenAttacks = 0.8f;
    [SerializeField] private float horizontalRange = 2.35f;
    [SerializeField] private float horizontalSpeed = 0.75f;
    [SerializeField] private float verticalAmplitude = 0.45f;
    [SerializeField] private float verticalSpeed = 1.1f;
    [SerializeField] private float movementLerp = 4.8f;

    private Transform playerTarget;
    private BossAttackMissiles missiles;
    private BossAttackLaser laser;
    private BossAttackMachineGun machineGun;
    private Vector3 movementCenter;
    private Vector3 entryStartPosition;
    private float movementTime;
    private bool movementEnabled;

    public void Initialize(
        Transform target,
        BossAttackMissiles missilesAttack,
        BossAttackLaser laserAttack,
        BossAttackMachineGun machineGunAttack,
        Vector3 centerPosition,
        Vector3 startPosition)
    {
        playerTarget = target;
        missiles = missilesAttack;
        laser = laserAttack;
        machineGun = machineGunAttack;
        movementCenter = centerPosition;
        entryStartPosition = startPosition;
    }

    private void Update()
    {
        if (!movementEnabled)
        {
            return;
        }

        movementTime += Time.deltaTime;
        Vector3 desiredPosition = movementCenter + new Vector3(
            Mathf.Sin(movementTime * horizontalSpeed) * horizontalRange,
            Mathf.Sin(movementTime * verticalSpeed) * verticalAmplitude,
            0f);

        desiredPosition.z = movementCenter.z;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, movementLerp * Time.deltaTime);
    }

    private IEnumerator Start()
    {
        transform.position = entryStartPosition;
        yield return EnterScene();

        movementEnabled = true;
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            if (playerTarget == null)
            {
                playerTarget = Player.Instance != null ? Player.Instance.transform : FindObjectOfType<Player>()?.transform;
            }

            yield return missiles.Execute(playerTarget);
            yield return new WaitForSeconds(pauseBetweenAttacks);

            yield return laser.Execute(playerTarget);
            yield return new WaitForSeconds(pauseBetweenAttacks);

            yield return machineGun.Execute(playerTarget);
            yield return new WaitForSeconds(pauseBetweenAttacks);
        }
    }

    private IEnumerator EnterScene()
    {
        float elapsed = 0f;
        Vector3 start = entryStartPosition;

        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / introDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            transform.position = Vector3.Lerp(start, movementCenter, eased);
            yield return null;
        }

        transform.position = movementCenter;
    }
}

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 300f;
    public float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}

public class BossAttackMissiles : MonoBehaviour
{
    public Transform[] launchPoints;
    public int missilesPerVolley = 4;
    public float delayBetweenMissiles = 0.4f;
    public float missileSpeed = 4.5f;
    public float turnSpeedDegrees = 220f;
    public float trackingDuration = 2.2f;
    public float explosionRadius = 0.9f;
    public float explosionDuration = 0.45f;
    public float visualScale = 0.45f;

    public IEnumerator Execute(Transform target)
    {
        if (launchPoints == null || launchPoints.Length == 0)
        {
            yield break;
        }

        for (int i = 0; i < missilesPerVolley; i++)
        {
            Transform spawn = launchPoints[i % launchPoints.Length];
            GameObject missileObject = new GameObject($"BossMissile_{i}");
            missileObject.transform.position = spawn.position;
            missileObject.transform.localScale = Vector3.one * visualScale;

            SpriteRenderer renderer = missileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
            renderer.color = new Color(1f, 0.55f, 0.1f, 1f);
            renderer.sortingOrder = 18;

            BoxCollider2D collider2D = missileObject.AddComponent<BoxCollider2D>();
            collider2D.isTrigger = true;

            BossMissile missile = missileObject.AddComponent<BossMissile>();
            missile.Initialize(target, missileSpeed, turnSpeedDegrees, trackingDuration, explosionRadius, explosionDuration);

            yield return new WaitForSeconds(delayBetweenMissiles);
        }
    }
}

public class BossMissile : MonoBehaviour
{
    private Transform playerTarget;
    private float moveSpeed;
    private float turnSpeedDegrees;
    private float trackingDuration;
    private float explosionRadius;
    private float explosionDuration;
    private Vector2 moveDirection = Vector2.down;
    private float age;
    private bool exploded;

    public void Initialize(Transform target, float speed, float turnSpeed, float trackTime, float radius, float duration)
    {
        playerTarget = target;
        moveSpeed = speed;
        turnSpeedDegrees = turnSpeed;
        trackingDuration = trackTime;
        explosionRadius = radius;
        explosionDuration = duration;
    }

    private void Update()
    {
        if (exploded)
        {
            return;
        }

        age += Time.deltaTime;

        if (age < trackingDuration && playerTarget != null)
        {
            Vector2 toTarget = ((Vector2)playerTarget.position - (Vector2)transform.position).normalized;
            float maxRadians = turnSpeedDegrees * Mathf.Deg2Rad * Time.deltaTime;
            moveDirection = Vector2.Lerp(moveDirection, toTarget, Mathf.Clamp01(maxRadians)).normalized;
        }

        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        if (age >= trackingDuration)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded)
        {
            return;
        }

        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            player.Death();
            Explode();
        }
    }

    private void Explode()
    {
        if (exploded)
        {
            return;
        }

        exploded = true;

        GameObject explosionObject = new GameObject("BossMissileExplosion");
        explosionObject.transform.position = transform.position;
        BossExplosion explosion = explosionObject.AddComponent<BossExplosion>();
        explosion.Initialize(explosionRadius, explosionDuration);

        Destroy(gameObject);
    }
}

public class BossExplosion : MonoBehaviour
{
    private float radius;
    private float duration;
    private float age;
    private bool didHitPlayer;
    private SpriteRenderer spriteRenderer;

    public void Initialize(float explosionRadius, float explosionDuration)
    {
        radius = explosionRadius;
        duration = explosionDuration;

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
        spriteRenderer.color = new Color(1f, 0.25f, 0.15f, 0.8f);
        spriteRenderer.sortingOrder = 16;

        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        age += Time.deltaTime;
        float t = Mathf.Clamp01(age / duration);
        float scale = Mathf.Lerp(0.1f, radius * 2f, t);
        transform.localScale = new Vector3(scale, scale, 1f);

        if (!didHitPlayer)
        {
            Player player = Player.Instance != null ? Player.Instance : FindObjectOfType<Player>();
            if (player != null && Vector2.Distance(player.transform.position, transform.position) <= radius)
            {
                player.Death();
                didHitPlayer = true;
            }
        }

        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(0.8f, 0f, t);
        spriteRenderer.color = color;

        if (age >= duration)
        {
            Destroy(gameObject);
        }
    }
}

public class BossAttackLaser : MonoBehaviour
{
    public Transform firePoint;
    public float beamLength = 11f;
    public float chargeDuration = 1.1f;
    public float fireDuration = 0.8f;
    public float warningWidth = 0.15f;
    public float beamWidth = 0.42f;

    public IEnumerator Execute(Transform target)
    {
        if (firePoint == null)
        {
            yield break;
        }

        Vector2 origin = firePoint.position;
        Vector2 lockedTarget = target != null ? (Vector2)target.position : origin + Vector2.down;
        Vector2 direction = lockedTarget - origin;
        if (direction.sqrMagnitude <= 0.001f)
        {
            direction = Vector2.down;
        }
        direction.Normalize();

        Vector2 beamEnd = origin + direction * beamLength;

        GameObject warning = CreateBeamObject(
            "LaserWarning",
            origin,
            beamEnd,
            warningWidth,
            new Color(1f, 0.92f, 0.2f, 0.65f),
            14);

        yield return new WaitForSeconds(chargeDuration);

        if (warning != null)
        {
            Destroy(warning);
        }

        GameObject beam = CreateBeamObject(
            "LaserBeam",
            origin,
            beamEnd,
            beamWidth,
            new Color(1f, 0.2f, 0.3f, 0.92f),
            19);

        float elapsed = 0f;
        while (elapsed < fireDuration)
        {
            elapsed += Time.deltaTime;

            Player player = Player.Instance != null ? Player.Instance : FindObjectOfType<Player>();
            if (player != null)
            {
                float distanceToBeam = DistancePointToSegment(player.transform.position, origin, beamEnd);
                if (distanceToBeam <= beamWidth * 0.5f)
                {
                    player.Death();
                }
            }

            yield return null;
        }

        if (beam != null)
        {
            Destroy(beam);
        }
    }

    private GameObject CreateBeamObject(string objectName, Vector2 start, Vector2 end, float width, Color color, int sortingOrder)
    {
        GameObject beamObject = new GameObject(objectName);
        Vector2 delta = end - start;
        float length = delta.magnitude;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        Vector2 midpoint = (start + end) * 0.5f;

        beamObject.transform.position = new Vector3(midpoint.x, midpoint.y, 0.8f);
        beamObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        beamObject.transform.localScale = new Vector3(length, width, 1f);

        SpriteRenderer renderer = beamObject.AddComponent<SpriteRenderer>();
        renderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
        renderer.color = color;
        renderer.sortingOrder = sortingOrder;

        return beamObject;
    }

    private float DistancePointToSegment(Vector2 point, Vector2 start, Vector2 end)
    {
        Vector2 segment = end - start;
        float segmentLengthSquared = segment.sqrMagnitude;
        if (segmentLengthSquared <= Mathf.Epsilon)
        {
            return Vector2.Distance(point, start);
        }

        float t = Vector2.Dot(point - start, segment) / segmentLengthSquared;
        t = Mathf.Clamp01(t);
        Vector2 projection = start + segment * t;
        return Vector2.Distance(point, projection);
    }
}

public class BossAttackMachineGun : MonoBehaviour
{
    public Transform firePoint;
    public int bulletsPerBurst = 14;
    public float shotInterval = 0.08f;
    public float bulletSpeed = 8f;
    public float spreadAngle = 7f;
    public float bulletLifetime = 4f;
    public float bulletScale = 0.22f;

    public IEnumerator Execute(Transform target)
    {
        if (firePoint == null)
        {
            yield break;
        }

        Vector2 aimPoint = target != null ? (Vector2)target.position : (Vector2)firePoint.position + Vector2.down;
        Vector2 baseDirection = (aimPoint - (Vector2)firePoint.position).normalized;
        if (baseDirection == Vector2.zero)
        {
            baseDirection = Vector2.down;
        }

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            float angle = Random.Range(-spreadAngle, spreadAngle);
            Vector2 shotDirection = Quaternion.Euler(0f, 0f, angle) * baseDirection;

            GameObject bulletObject = new GameObject($"BossMachineGunBullet_{i}");
            bulletObject.transform.position = firePoint.position;
            bulletObject.transform.localScale = Vector3.one * bulletScale;

            SpriteRenderer renderer = bulletObject.AddComponent<SpriteRenderer>();
            renderer.sprite = BossPrototypeVisuals.PlaceholderSprite;
            renderer.color = new Color(0.25f, 1f, 1f, 1f);
            renderer.sortingOrder = 17;

            BoxCollider2D collider2D = bulletObject.AddComponent<BoxCollider2D>();
            collider2D.isTrigger = true;

            BossMachineGunBullet bullet = bulletObject.AddComponent<BossMachineGunBullet>();
            bullet.Initialize(shotDirection, bulletSpeed, bulletLifetime);

            yield return new WaitForSeconds(shotInterval);
        }
    }
}

public class BossMachineGunBullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private float lifetime;
    private float age;

    public void Initialize(Vector2 direction, float speed, float maxLifetime)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        lifetime = maxLifetime;
    }

    private void Update()
    {
        age += Time.deltaTime;
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        Player player = Player.Instance != null ? Player.Instance : FindObjectOfType<Player>();
        if (player != null && Vector2.Distance(player.transform.position, transform.position) <= 0.35f)
        {
            player.Death();
            Destroy(gameObject);
            return;
        }

        if (age >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            player.Death();
            Destroy(gameObject);
        }
    }
}

internal static class BossPrototypeVisuals
{
    private static Sprite cachedSprite;

    public static Sprite PlaceholderSprite
    {
        get
        {
            if (cachedSprite == null)
            {
                Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                texture.filterMode = FilterMode.Point;
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                cachedSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            }

            return cachedSprite;
        }
    }
}