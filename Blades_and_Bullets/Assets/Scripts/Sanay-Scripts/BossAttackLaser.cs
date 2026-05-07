using System.Collections;
using UnityEngine;

public class BossAttackLaser : MonoBehaviour
{
    [Header("Fire Point")]
    public Transform firePoint;

    [Header("Laser Settings")]
    public float beamLength = 11f;
    public float chargeDuration = 1.1f;
    public float fireDuration = 0.8f;
    public float warningWidth = 0.15f;
    public float beamWidth = 0.42f;

    public IEnumerator Fire(Transform target)
    {
        if (firePoint == null)
        {
            Debug.LogWarning("BossAttackLaser: Fire point is not assigned.");
            yield break;
        }

        Vector3 direction = GetDirectionToTarget(target);

        LineRenderer warningLine = CreateLine("Laser_Warning", Color.red, warningWidth);
        SetLine(warningLine, firePoint.position, firePoint.position + direction * beamLength);

        yield return new WaitForSeconds(chargeDuration);

        if (warningLine != null)
        {
            Destroy(warningLine.gameObject);
        }

        LineRenderer beamLine = CreateLine("Laser_Beam", Color.magenta, beamWidth);
        SetLine(beamLine, firePoint.position, firePoint.position + direction * beamLength);

        yield return new WaitForSeconds(fireDuration);

        if (beamLine != null)
        {
            Destroy(beamLine.gameObject);
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

    private LineRenderer CreateLine(string name, Color color, float width)
    {
        GameObject lineObject = new GameObject(name);

        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = width;
        line.endWidth = width;
        line.material = BossPrototypeVisuals.DefaultLineMaterial;
        line.startColor = color;
        line.endColor = color;
        line.sortingOrder = 30;

        return line;
    }

    private void SetLine(LineRenderer line, Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}