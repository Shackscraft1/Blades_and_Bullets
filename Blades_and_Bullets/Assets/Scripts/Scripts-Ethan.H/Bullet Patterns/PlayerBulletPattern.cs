using UnityEngine;

public class PlayerBulletPattern : BaseBulletPattern
{
    [SerializeField]
    private BulletTypeSO bulletTypeSO;

    public override void FirePattern()
    {
        Vector2 dir = -Vector2.up;
        bulletSpawner.Fire(bulletTypeSO, dir);
    }




}
