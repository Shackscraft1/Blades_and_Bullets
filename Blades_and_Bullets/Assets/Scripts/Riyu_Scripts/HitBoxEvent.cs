using System;
using UnityEngine;

public class HitboxEvent : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Contact");
        if (other.CompareTag("EnemyBullet"))
        {
            if (Player.Instance != null)
            {
                Player.Instance.Death();
            } 
        }

    }

   
}
