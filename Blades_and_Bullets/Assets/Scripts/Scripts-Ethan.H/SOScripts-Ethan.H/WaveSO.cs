using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO", menuName = "WaveData/WaveSO")]
public class WaveSO : ScriptableObject
{
    [Header("Enemy")]
    public GameObject enemyPrefab;
    public int enemyCount = 6;

    [Header("Entry")]
    public float speed = 6f;
    public float entryGap = 0.35f;

    [Header("Exit")]
    public WaveEnemy.EndBehavior endBehavior;

    [Header("Formation Shape")]
    public bool spawnInFormation;
    public WaveFormationControler.FormationType formationType;
    public float slotSpacing = 0.3f;
    public float verticalDepth = 0.18f;

    [Header("Formation Movement")]

  
    public Vector3 middleOffset = Vector3.zero;
    public Vector3 leftOffset = new Vector3(-2f, 0f, 0f);
    public Vector3 rightOffset = new Vector3(2f, 0f, 0f);
    public float formationTravelSpeed = 2f;
    public float waitAtLeft = 0.5f;
    public float waitAtRight = 0.5f;
    public float waitAtMiddle = 0.5f;
}





