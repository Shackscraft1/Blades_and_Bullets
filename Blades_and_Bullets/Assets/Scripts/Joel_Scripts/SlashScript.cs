using System;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public static EventHandler<OnSlashingSomethingArgs> OnSlashingSomething;

    private enum BulletType
    {
        Normal,
        Special
    }
    [SerializeField] private BulletType bulletType;
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
        Enemy enemy = other.GetComponent<Enemy>();
        Component target = null;
        switch (bulletType)
        {
            case BulletType.Normal:
                target = enemy;
                break;

            case BulletType.Special:
                target = bullet != null ? bullet : enemy;
                break;
        }
        if (target != null)
        {
            Destroy(target.gameObject);
            OnSlashingSomething?.Invoke(this, new OnSlashingSomethingArgs
            {
                TargetHit = target.gameObject
            });
        }
    }
}