using UnityEngine;

[CreateAssetMenu(fileName = "SplitPatternCircle", menuName = "Scriptable Objects/SplitPatternCircle")]
public class SplitPatternCircle : SplitBulletPatternSO
{
    [SerializeField]
    private int splitCount = 9;

    public override void Split(Bullet bullet)
    {
        for (int i = 0; i < splitCount; i++)
        {
            float angle = i * (360f / splitCount);
            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
            bullet.SpawnChild(dir);
        }
    }
    
}
