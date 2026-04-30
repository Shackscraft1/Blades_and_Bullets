using System.Collections.Generic;
using UnityEngine;
using static WaveController;

public class WaveFormationControler : MonoBehaviour
{
    [SerializeField]
    private Transform formationCenter;
    public Transform center => formationCenter;

    public enum FormationType
    {
        V,
        Line,
        Slope,
        InvertedV
    }


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

    public Vector3 GetSlotOffset(
       int index,
       int total,
       FormationType formationType,
       float spacing,
       float depth)
    {
        float centeredIndex = index - (total - 1) / 2f;

        switch (formationType)
        {
            case FormationType.Line:
                return new Vector3(centeredIndex * spacing, 0f, 0f);

            case FormationType.Slope:
                return new Vector3(centeredIndex * spacing, centeredIndex * depth, 0f);

            case FormationType.InvertedV:
                return new Vector3(centeredIndex * spacing, Mathf.Abs(centeredIndex) * depth, 0f);

            case FormationType.V:
            default:
                return new Vector3(centeredIndex * spacing, -Mathf.Abs(centeredIndex) * depth, 0f);
        }
    }

}



