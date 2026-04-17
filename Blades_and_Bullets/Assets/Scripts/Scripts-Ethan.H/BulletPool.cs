using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private int bulletPoolStartingSize = 100;

    private readonly Queue<Bullet>bulletPool = new Queue<Bullet>();

    private void Awake()
    {
        for (int i = 0; i < bulletPoolStartingSize; i++)
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform);
        bullet.gameObject.SetActive(false);
        bullet.SetPool(this);
        bulletPool.Enqueue(bullet);

    }

    public Bullet GetBullet()
    {
        if(bulletPool.Count == 0)
        {
            CreateBullet();
        }
        Bullet bullet = bulletPool.Dequeue();
        bullet.gameObject.SetActive(true);
        return bullet;




    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);

    }


}
