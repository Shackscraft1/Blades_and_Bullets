using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum DropType
    {
        Points,
        Power
    }

    public DropType dropType;
    public float scatterForce = 3f;
    public float lifeTime = 6f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Scatter();

        Destroy(gameObject, lifeTime);
    }

    void Scatter()
    {
        if (rb == null) return;

        Vector2 randomDir = Random.insideUnitCircle.normalized;

        rb.linearVelocity = randomDir * scatterForce;
    }
}