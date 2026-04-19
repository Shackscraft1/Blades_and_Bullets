using UnityEngine;

public class BezierPath : MonoBehaviour
{
    public Transform[] points;

    public Vector3 GetPoint(float t)
    {
        int numSegments = (points.Length - 1) / 3;

        int segmentIndex = Mathf.Min(Mathf.FloorToInt(t * numSegments), numSegments - 1);

        float localT = (t * numSegments) - segmentIndex;

        int i = segmentIndex * 3;

        return CalculateBezierPoint(
            localT,
            points[i].position,
            points[i + 1].position,
            points[i + 2].position,
            points[i + 3].position
        );
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;

        return u * u * u * p0 +
               3 * u * u * t * p1 +
               3 * u * t * t * p2 +
               t * t * t * p3;
    }

    void OnDrawGizmos()
    {
        if (points == null || points.Length < 4) return;

        Vector3 prevPoint = points[0].position;

        for (float t = 0; t <= 1f; t += 0.02f)
        {
            Vector3 point = GetPoint(t);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}