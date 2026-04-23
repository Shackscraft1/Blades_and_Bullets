using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBulletMovementSO", menuName = "Scriptable Objects/PlayerBulletMovementSO")]
public class PlayerBulletMovementSO : BulletMovementSO
{
    [SerializeField]
    private float speed;

    

    public override IMovementType CreateMovement()
    {
        return new PlayerBulletMovement(speed);

    }


}
