using System;
using UnityEngine;

public class HitBoxEvent : MonoBehaviour
{
    public static EventHandler PlayerGetsHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            PlayerGetsHit?.Invoke(this, EventArgs.Empty);
        }
    }
    
}
