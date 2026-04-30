using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Phase
    {
        EntryPath,
        MoveToFormation,
        Formation,
        MoveToExitStart,
        ExitPath
    }

    [Header("Entry Path")]
    public BezierPath entryPath;
    public float entryDuration = 6f;
    public float entryStartOffset = 0f;
    public float entryToFormationT = 0.75f;

    [Header("Formation")]
    public Transform formationCenter;
    public Vector3 slotOffset;
    public float formationMoveSpeed = 4f;
    public float formationArriveDistance = 0.05f;

    [Header("Exit Path")]
    public BezierPath exitPath;
    public float exitDuration = 4f;
    public float exitStartOffset = 0f;
    public float exitMoveSpeed = 4f;
    public float exitArriveDistance = 0.05f;

    [Header("Health")]
    public int maxHP = 1;
    private int currentHP;

    [Header("Drops")]
    public GameObject pointDropPrefab;
    public GameObject powerDropPrefab;
    public int pointDropCount = 1;
    public int powerDropCount = 1;

    [Header("Shield")]
    public bool spawnShieldInFormation = true;
    public GameObject shieldPrefab;
    public Vector3 shieldLocalOffset = new Vector3(0f, -0.35f, 0f);

    public Phase currentPhase = Phase.EntryPath;

    private float entryTimer;
    private float exitTimer;

    private Vector3 exitStartTarget;
    private bool exitInitialized = false;

    private GameObject activeShield;
    private bool shieldSpawned = false;

    void Start()
    {
        SlashScript.OnSlashingSomething += OnSlashingSomething;
        currentHP = maxHP;
        entryTimer = entryStartOffset;

        if (entryPath != null)
        {
            transform.position = entryPath.GetPoint(0f);
        }
    }

    private void OnSlashingSomething(object sender, SlashScript.OnSlashingSomethingArgs e)
    {
        
        if(e.TargetHit.Equals(gameObject)) Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        SlashScript.OnSlashingSomething -=OnSlashingSomething;
    }

    void Update()
    {
        switch (currentPhase)
        {
            case Phase.EntryPath:
                FollowEntryPath();
                break;

            case Phase.MoveToFormation:
                MoveToFormationSlot();
                break;

            case Phase.Formation:
                HoldFormation();
                TrySpawnShield();
                break;

            case Phase.MoveToExitStart:
                MoveToExitStart();
                break;

            case Phase.ExitPath:
                FollowExitPath();
                break;
        }
    }

    void FollowEntryPath()
    {
        if (entryPath == null) return;

        entryTimer += Time.deltaTime;

        if (entryTimer < 0f)
        {
            transform.position = entryPath.GetPoint(0f);
            return;
        }

        float t = entryTimer / entryDuration;
        float clampedT = Mathf.Clamp01(t);

        transform.position = entryPath.GetPoint(clampedT);

        if (t >= entryToFormationT)
        {
            currentPhase = Phase.MoveToFormation;
        }
    }

    void MoveToFormationSlot()
    {
        if (formationCenter == null) return;

        Vector3 target = formationCenter.position + slotOffset;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            formationMoveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) <= formationArriveDistance)
        {
            currentPhase = Phase.Formation;
        }
    }

    void HoldFormation()
    {
        if (formationCenter == null) return;

        Vector3 target = formationCenter.position + slotOffset;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            formationMoveSpeed * Time.deltaTime
        );
    }

    void TrySpawnShield()
    {
        if (!spawnShieldInFormation) return;
        if (shieldSpawned) return;
        if (shieldPrefab == null) return;

        activeShield = Instantiate(shieldPrefab, transform);
        activeShield.transform.localPosition = shieldLocalOffset;
        activeShield.transform.localRotation = Quaternion.identity;

        shieldSpawned = true;
    }

    void RemoveShield()
    {
        if (activeShield != null)
        {
            Destroy(activeShield);
            activeShield = null;
            shieldSpawned = false;
        }
    }

    void MoveToExitStart()
    {
        // shield turns off when leaving formation
        RemoveShield();

        if (!exitInitialized || exitPath == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            exitStartTarget,
            exitMoveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, exitStartTarget) <= exitArriveDistance)
        {
            exitTimer = exitStartOffset;
            currentPhase = Phase.ExitPath;
        }
    }

    void FollowExitPath()
    {
        if (exitPath == null) return;

        exitTimer += Time.deltaTime;

        if (exitTimer < 0f)
        {
            transform.position = exitPath.GetPoint(0f);
            return;
        }

        float t = exitTimer / exitDuration;
        float clampedT = Mathf.Clamp01(t);

        transform.position = exitPath.GetPoint(clampedT);

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }

    public void StartExit(BezierPath sharedExitPath, float duration, float startOffset)
    {
        exitPath = sharedExitPath;
        exitDuration = duration;
        exitStartOffset = startOffset;

        if (exitPath != null)
        {
            exitStartTarget = exitPath.GetPoint(0f);
            exitInitialized = true;
            currentPhase = Phase.MoveToExitStart;
        }
    }

    public void TakeDamage()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SpawnDrops();
        RemoveShield();
        Destroy(gameObject);
    }

    void SpawnDrops()
    {
        for (int i = 0; i < pointDropCount; i++)
        {
            if (pointDropPrefab != null)
            {
                Instantiate(pointDropPrefab, transform.position, Quaternion.identity);
            }
        }

        for (int i = 0; i < powerDropCount; i++)
        {
            if (powerDropPrefab != null)
            {
                Instantiate(powerDropPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}