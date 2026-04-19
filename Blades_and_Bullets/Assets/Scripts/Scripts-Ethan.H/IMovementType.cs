using UnityEngine;

public interface IMovementType 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Init(Bullet bullet);

    void Tick(Bullet bullet);
}
