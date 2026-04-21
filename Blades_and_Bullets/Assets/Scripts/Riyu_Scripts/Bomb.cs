using UnityEngine;

public class Bomb : MonoBehaviour
{

    void Update()
    {
        if (transform.localScale.x < 200)
        {
            transform.localScale += new Vector3(.4f, 0f, .4f);
        } else
        {
            Destroy(gameObject);
        }



    }
}
