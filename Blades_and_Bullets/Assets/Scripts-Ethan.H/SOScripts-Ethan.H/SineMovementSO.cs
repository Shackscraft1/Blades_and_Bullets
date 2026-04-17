using UnityEngine;

[CreateAssetMenu(fileName = "SineMovementSO", menuName = "Scriptable Objects/SineMovementSO")]
public class SineMovementSO : BulletMovementSO
{

    [SerializeField]
    private float frequency;
    [SerializeField]
    private float amplitude;
    [SerializeField]
    private float speed;

    public override IMovementType CreateMovement()
    {
        return new SineBulletMovement(frequency, amplitude, speed);
    }
    
}
