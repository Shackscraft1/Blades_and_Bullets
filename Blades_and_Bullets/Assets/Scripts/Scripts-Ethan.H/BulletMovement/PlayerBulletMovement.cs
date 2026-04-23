using UnityEngine;

public class PlayerBulletMovement : IMovementType
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private readonly float speed;

    public void Init(Bullet bullet)
    {

    }

    public void Tick(Bullet bullet)
    {
        bullet.transform.position -= (Vector3)(bullet.Direction * speed * Time.deltaTime);

    }

    public PlayerBulletMovement(float speed)
    {
        this.speed = speed;
    }
}
