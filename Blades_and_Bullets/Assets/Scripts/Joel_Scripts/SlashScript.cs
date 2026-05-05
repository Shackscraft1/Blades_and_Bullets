using System;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public static EventHandler<OnSlashingSomethingArgs> OnSlashingSomething;

    private float damage;

    public enum BulletType
    {
        Normal,
        Special,
        Bomb
    }
    [SerializeField] public BulletType bulletType;
    public class OnSlashingSomethingArgs : EventArgs
    {
        public GameObject TargetHit;
    }
    private float speed = 50f;
    private void Update()
    {
        if (bulletType == BulletType.Normal)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        } 
    }
    private void Start()
    {

        if (bulletType == BulletType.Normal)
        {
            Destroy(gameObject, .5f);
        } 
    }

 
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        Bullet bullet = other.GetComponentInParent<Bullet>();
        WaveEnemy enemy = other.GetComponent<WaveEnemy>();
        switch (bulletType)
        {
            case BulletType.Normal:
                if (enemy != null)
                {   
                    enemy.TakeDamage(damage);
                    OnSlashingSomething?.Invoke(this, new OnSlashingSomethingArgs
                    {
                        TargetHit = enemy.gameObject
                    });
                    Destroy(this.gameObject);
                }
                
                break;

            case BulletType.Special:
                if (enemy != null)
                {
                    enemy.TakeDamage(damage * 4f);
                    OnSlashingSomething?.Invoke(this, new OnSlashingSomethingArgs
                    {
                        TargetHit = enemy.gameObject
                    });

                }
                if (bullet != null)
                {
                    OnSlashingSomething?.Invoke(this, new OnSlashingSomethingArgs
                    {
                        TargetHit = bullet.gameObject
                    });
                    bullet.DespawnBullet();
                    
                }
                break;
            case BulletType.Bomb:
                if (enemy != null)
                {
                    enemy.TakeDamage(1000);
                }else if (bullet != null)
                {
                    OnSlashingSomething?.Invoke(this, new OnSlashingSomethingArgs
                    {
                        TargetHit = bullet.gameObject
                    });
                    bullet.DespawnBullet();
                    
                }
                break;
        }

    }
    public BulletType GetBulletType()
    {
        return bulletType;
    }

    public void SetDamage(float num)
    {
        damage = num;
    }
}