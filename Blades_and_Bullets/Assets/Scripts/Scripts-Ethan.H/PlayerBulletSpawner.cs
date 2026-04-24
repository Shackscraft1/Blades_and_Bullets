using UnityEngine;

public class PlayerBulletSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private BulletPool bulletPool;

    private const string PLAYER_BULLET = "Player Bullet";

    public void Init(BulletPool pool)
    {
        bulletPool = pool;

    }




    public void Fire(BulletTypeSO bulletTypeSO, Vector2 direction)

    {
        Bullet bullet = bulletPool.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        //We change it later once we get enemies at main scene.
       // bullet.gameObject.layer = 2;
        bullet.Init(bulletTypeSO, direction.normalized);

    }
}
