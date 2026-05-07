using UnityEngine;

public class SanayBossBootstrap : MonoBehaviour
{
    public static SanayBossBootstrap Instance { get; private set; }

    [Header("Boss Prefab")]
    [SerializeField] private GameObject bossPrefab;

    [Header("Spawn")]
    [SerializeField] private Vector3 bossSpawnPosition = new Vector3(-3f, 3.4f, 0.8f);
    [SerializeField] private float bossEntryHeight = 3.2f;

    [Header("Scene")]
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

        if (bossPrefab == null)
        {
            Debug.LogError("SanayBossBootstrap: Boss Prefab is not assigned.");
            return;
        }

        hasSpawnedBoss = true;

        GameObject existingBoss = GameObject.Find(bossName);
        if (existingBoss != null)
        {
            Destroy(existingBoss);
        }

        Transform parent = GameObject.Find(enemyContainerName)?.transform ?? transform;

        Transform playerTarget = Player.Instance != null
            ? Player.Instance.transform
            : FindObjectOfType<Player>()?.transform;

        Vector3 entryPosition = bossSpawnPosition + Vector3.up * bossEntryHeight;

        activeBoss = Instantiate(bossPrefab, entryPosition, Quaternion.identity, parent);
        activeBoss.name = bossName;

        BossAttackMissiles missiles = activeBoss.GetComponent<BossAttackMissiles>();
        BossAttackLaser laser = activeBoss.GetComponent<BossAttackLaser>();
        BossAttackMachineGun machineGun = activeBoss.GetComponent<BossAttackMachineGun>();
        BossController controller = activeBoss.GetComponent<BossController>();

        if (controller != null)
        {
            controller.Initialize(
                playerTarget,
                missiles,
                laser,
                machineGun,
                bossSpawnPosition,
                entryPosition
            );
        }

        if (disableWavesObject)
        {
            GameObject waves = GameObject.Find(wavesObjectName);
            if (waves != null)
            {
                waves.SetActive(false);
            }
        }
    }
}