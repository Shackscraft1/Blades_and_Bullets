using UnityEngine;

public class MouseEnemyKiller : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(enemy.maxHP);
                }
            }
        }
    }
}