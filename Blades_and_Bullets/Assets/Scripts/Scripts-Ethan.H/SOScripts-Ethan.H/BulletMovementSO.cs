using UnityEngine;


public abstract class BulletMovementSO : ScriptableObject
{
    public abstract IMovementType CreateMovement();
}
