using UnityEngine;

public class HitBoxEvent : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BulletTypeSO>())
        {
            
        }
    }
    
}
