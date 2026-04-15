using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float speed;
    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}
