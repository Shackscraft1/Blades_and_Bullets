using UnityEngine;

[CreateAssetMenu(fileName = "SpiralMovementSO", menuName = "BulletMovement/SpiralMovementSO")]
public class SpiralMovementSO : BulletMovementSO 
{
    [SerializeField]
    private float angleSpeed;
    [SerializeField]
    private float tightness;
    public override IMovementType CreateMovement()
    {
        
        return new SpiralBulletMovement(angleSpeed, tightness);
    }


   

    
}
