using UnityEngine;

public class SpiralBulletMovement : IMovementType
{
    private readonly float angularSpeed;
    private readonly float tightness;

    private float angle;
    private Vector2 center;

    public SpiralBulletMovement(float angularSpeed, float tightness)
    {
        this.angularSpeed = angularSpeed;
        this.tightness = tightness;
    }

    public void Init(Bullet bullet)
    {
        center = bullet.transform.position;

     
    }

    public void Tick(Bullet bullet)
    {
        angle += angularSpeed * Time.deltaTime;
        float radius = Mathf.Pow(angle,0.6f) * tightness;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;


       
        bullet.transform.position = center + new Vector2(x, y);
    }
}