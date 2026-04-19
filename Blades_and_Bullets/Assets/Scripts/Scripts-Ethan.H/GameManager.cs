using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BulletPool pool;
    public GameObject enemy;



    private void Start()
    {
        enemy.GetComponentInChildren<BulletSpawner>().Init(pool);
        
    }
    void Update()
    {
        
    }
}
