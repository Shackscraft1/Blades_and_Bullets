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
    
}
