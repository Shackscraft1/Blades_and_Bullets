using System;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public static EventHandler<OnSlashingSomethingArgs> OnSlashingSomething;

    public class OnSlashingSomethingArgs : EventArgs
    {
        public GameObject TargetHit;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponentInParent<Bullet>();
        Enemy enemy = other.GetComponent<Enemy>();

        Component target = bullet != null ? bullet : enemy;

        if (target != null)
        {
            OnSlashingSomething?.Invoke(this, new OnSlashingSomethingArgs
            {
                TargetHit = target.gameObject
            });
        }
    }
}
