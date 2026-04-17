using UnityEngine;

[CreateAssetMenu(fileName = "SplitBulletPatternSO", menuName = "Scriptable Objects/SplitBulletPatternSO")]
public abstract class SplitBulletPatternSO : ScriptableObject
{
    public abstract void Split(Bullet bullet);
}
