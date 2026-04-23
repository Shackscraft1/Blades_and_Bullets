using UnityEngine;

[CreateAssetMenu(fileName = "StraightBulletMovementSO", menuName = "Scriptable Objects/StraightBulletMovementSO")]
public class StraightBulletMovementSO : BulletMovementSO
{
    [SerializeField]
    private float speed = 5f;

    public override IMovementType CreateMovement()
    {
        return new StraightBulletMovement(speed);
    }


}
