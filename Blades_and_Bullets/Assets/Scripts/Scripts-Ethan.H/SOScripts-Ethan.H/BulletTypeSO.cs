using UnityEngine;
using static UnityEngine.Rigidbody2D;

[CreateAssetMenu(fileName = "BulletTypeSO", menuName = "Scriptable Objects/BulletTypeSO")]
public class BulletTypeSO : ScriptableObject
{


    public enum HurtBoxType
    {
        Circle,
        Capsule

    }
   

    

    public string bulletName;
    public Sprite sprite;
    public float speed;
    public float scale;
    public float lifetime = 20f;
    public HurtBoxType hurtBoxType;

    [Header("Hurtbox")]
    public float radius = 0.5f;
    public Vector2 offset = Vector2.zero;
    public Vector2 sizeCapsule = Vector2.zero;


  

    [Header("Movement")]
    public BulletMovementSO bulletMovement;
    


    [Header("Split")]
    public bool canSplit;
    public float splitTime;
    public int splitMaxCount;
    public float splitAngleOffset;
    public BulletTypeSO splitedBullet;
    public SplitBulletPatternSO splitBulletPattern;

    



    
    






}
