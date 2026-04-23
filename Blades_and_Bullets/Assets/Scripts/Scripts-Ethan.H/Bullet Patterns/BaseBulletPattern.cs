using UnityEngine;

public abstract class BaseBulletPattern : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    [SerializeField]
   protected BulletSpawner bulletSpawner;
    [SerializeField]
    protected PlayerBulletSpawner playerBulletSpawner;

    public abstract void FirePattern();

}
