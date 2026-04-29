using UnityEngine;

public class SpecialSlash : MonoBehaviour
{
    
    /*
    private float speed = 50f;
    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
    private void Start()
    {
        Destroy(gameObject, .075f);
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

        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            Destroy(enemy.gameObject);
            Destroy(gameObject); 
        }
        */

    

}
