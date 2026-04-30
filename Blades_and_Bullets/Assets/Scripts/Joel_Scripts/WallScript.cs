using System;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    
    public static EventHandler<OnWallHitArgs> OnWallHit;

    public class OnWallHitArgs : EventArgs
    {
        public GameObject wallHitGameObjectType;
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            OnWallHit?.Invoke(this, new OnWallHitArgs { wallHitGameObjectType = collision.gameObject });
        }


    }
    
}
