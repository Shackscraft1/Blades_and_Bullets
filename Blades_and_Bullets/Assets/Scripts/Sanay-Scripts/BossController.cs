using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float introDuration = 1.5f;
    [SerializeField] private float spawnDelay = 0.4f;
    [SerializeField] private float pauseBetweenAttacks = 0.8f;

    [Header("Movement")]
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

    private IEnumerator Start()
    {
        transform.position = entryStartPosition;

        yield return new WaitForSeconds(spawnDelay);

        Vector3 start = transform.position;
        float timer = 0f;

        while (timer < introDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / introDuration);
            transform.position = Vector3.Lerp(start, movementCenter, t);
            yield return null;
        }

        transform.position = movementCenter;
        movementEnabled = true;

        StartCoroutine(AttackLoop());
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
            0f
        );

        desiredPosition.z = movementCenter.z;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, movementLerp * Time.deltaTime);
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (missiles != null)
            {
                yield return missiles.Fire(playerTarget);
                yield return new WaitForSeconds(pauseBetweenAttacks);
            }

            if (laser != null)
            {
                yield return laser.Fire(playerTarget);
                yield return new WaitForSeconds(pauseBetweenAttacks);
            }

            if (machineGun != null)
            {
                yield return machineGun.Fire(playerTarget);
                yield return new WaitForSeconds(pauseBetweenAttacks);
            }
        }
    }
}