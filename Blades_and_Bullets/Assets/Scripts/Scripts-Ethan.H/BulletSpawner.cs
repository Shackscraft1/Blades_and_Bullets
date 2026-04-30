using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    


    private BulletPool bulletPool;


   public void Init(BulletPool pool)
    {
        bulletPool = pool;

    }




    public void Fire(BulletTypeSO bulletTypeSO, Vector2 direction) 
    
    {
        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle - 90f);

        Fire(bulletTypeSO, direction, rot);

    }

    public void Fire(BulletTypeSO bulletTypeSO, Vector2 direction, Quaternion rotation)
    {
        Bullet bullet = bulletPool.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = rotation;
        bullet.Init(bulletTypeSO, direction.normalized);
    }




}
