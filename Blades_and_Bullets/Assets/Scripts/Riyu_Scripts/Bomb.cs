using UnityEngine;

public class Bomb : MonoBehaviour
{

    void Update()
    {
        if (transform.localScale.x < 20f)
        {
            transform.localScale += new Vector3(.3f, .3f, 0f);
        } else
        {
            Destroy(gameObject);
        } 

    }
    
}
