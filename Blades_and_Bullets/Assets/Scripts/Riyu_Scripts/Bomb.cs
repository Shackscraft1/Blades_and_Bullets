using UnityEngine;

public class Bomb : MonoBehaviour
{

    void Update()
    {
        if (transform.localScale.x < 100)
        {
            transform.localScale += new Vector3(.4f, .4f, 0f);
        } else
        {
            Destroy(gameObject);
        } 

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Enemy enemy = other.GetComponent<Enemy>();
        // Bullet bullet = other.GetComponent<Bullet>();
        
        // if (enemy != null)
        // {
        //     Destroy(enemy.gameObject);
        // } 

        // if (bullet != null)
        // {
        //     Destroy(bullet.gameObject);
        // }

        // if (other.CompareTag("EnemyBullet"))
        // {
        //     Destroy(other.gameObject);
        // }

        Bullet bullet = other.GetComponentInParent<Bullet>();
        if (bullet != null)
        {
            bullet.DespawnBullet();
        }

    }
}
