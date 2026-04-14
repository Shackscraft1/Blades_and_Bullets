using UnityEngine;

[CreateAssetMenu(fileName = "BulletMovementSO", menuName = "Scriptable Objects/BulletMovementSO")]
public abstract class BulletMovementSO : ScriptableObject
{
    public abstract IMovementType CreateMovement();
}
