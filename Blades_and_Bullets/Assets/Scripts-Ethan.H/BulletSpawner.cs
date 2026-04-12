using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private BulletPool bulletPool;
    [SerializeField]
    private BulletTypeSO defaultBulletType;


    public void Fire(Vector2 direction)
    {
        Fire(defaultBulletType, direction);

    }

    public void Fire(BulletTypeSO bulletTypeSO, Vector2 direction) 
    
    {
        Bullet bullet = bulletPool.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.Init(bulletTypeSO, direction);

    }




}
