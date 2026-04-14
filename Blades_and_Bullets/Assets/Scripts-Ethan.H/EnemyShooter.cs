using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private int fireRate;
    [SerializeField]
    private BaseBulletPattern bulletPattern;

   
    private float timer;

    

    // Update is called once per frame
    private void Update()
    {
       

        if (bulletPattern == null)
        {
         
            return;
        }

        timer += Time.deltaTime;

        if (timer >= fireRate) {
            
            timer = 0f;
            
            bulletPattern.FirePattern();
        }

        
    }
}
