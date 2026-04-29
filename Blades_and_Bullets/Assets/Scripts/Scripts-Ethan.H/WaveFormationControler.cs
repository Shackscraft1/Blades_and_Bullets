using System.Collections.Generic;
using UnityEngine;

public class WaveFormationControler : MonoBehaviour
{
    [SerializeField]
    private Transform formationCenter;
    public Transform center => formationCenter;

    public Vector3 GetVSlotOffset(int index, int totalCount, float spacing, float depth)
    {
        float centeredIndex = index - (totalCount - 1) / 2f;
        float xOffset = centeredIndex * spacing;
        float yOffset = -Mathf.Abs(centeredIndex) * depth;
        return new Vector3(xOffset, yOffset, 0f);
    }

    public IEnumerable<Vector3> GetVSlots(int totalCount, float spacing, float depth)
    {
        for (int i = 0; i < totalCount; i++)
        {
            yield return GetVSlotOffset(i, totalCount, spacing, depth);
        }
    }
}



