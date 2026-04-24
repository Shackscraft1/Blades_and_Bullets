using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    [SerializeField]private float speed;
    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Hit");
        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            Destroy(enemy.gameObject);
            Destroy(gameObject); 
        }
    }
}
