using UnityEngine;

public class SpiralBulletPattern : BaseBulletPattern
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    [SerializeField]
    private float angle;
    [SerializeField]
    BulletTypeSO bulletType;

    private float currentAngle;

    public override void FirePattern()
    {
        currentAngle += angle;
        float radian = Mathf.Deg2Rad * currentAngle;

        Vector2 dir = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

       spawner.Fire(bulletType, dir);

        
    }
}
