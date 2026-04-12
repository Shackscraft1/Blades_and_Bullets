using UnityEngine;
using static UnityEngine.Rigidbody2D;

[CreateAssetMenu(fileName = "BulletTypeSO", menuName = "Scriptable Objects/BulletTypeSO")]
public class BulletTypeSO : ScriptableObject
{

    public enum MovementType
    {
        Straight,
        Sine,
        Homing,
        Spiral,

    }

    public MovementType movementType;

    public string bulletName;
    public Sprite sprite;
    public float speed;
    public float scale;
    public float lifetime = 20f;


    [Header("Sine")]
    public float amplitude;
    public float frequency;

    [Header("Homing")]
    public float turnSpeed;

    [Header("Spiral")]
    public float rotationSpeed;

    [Header("Split")]
    public bool canSplit;
    public float splitTime;
    public int splitMaxCount;
    public float splitAngleOffset;
    public BulletTypeSO splitedBullet;
    public SplitBulletPatternSO splitBulletPattern;

    
    public IMovementType CreateMovement()
    {
        switch (movementType)
        {
            case MovementType.Straight:
                return new StraightBulletMovement(speed);
            default:
                return new StraightBulletMovement(speed);

        }


    }






}
