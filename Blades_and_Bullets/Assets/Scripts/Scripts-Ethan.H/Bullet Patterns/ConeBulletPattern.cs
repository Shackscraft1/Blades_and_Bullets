using UnityEngine;

public class ConeBulletPattern : BaseBulletPattern
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private int bulletCount;
    [SerializeField]
    private float spreadAngle;
    [SerializeField]
    private BulletTypeSO bulletType;

    



    public override void FirePattern()
    {
     
        float startingAngle = -spreadAngle * 0.5f;
        float step = bulletCount > 1 ? spreadAngle / (bulletCount - 1) : 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startingAngle + step * i;
            //Note:When not aiming this should be the default direction
            Vector2 dir = Quaternion.Euler(0, 0, angle) * -transform.up;
            float angleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angleDeg);

            bulletSpawner.Fire(bulletType, dir);
            
        }

        
    }
    
}
