using UnityEngine;

public class PlayerBullet : BaseBullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //弾が当たった相手がIDamagableを持っている場合は、ダメージを与える
        if (collision.TryGetComponent<IDamagable>(out var d))
        {
            d.TakeDamage(_bulleAtk);
        }

        ResetDirection();
        PBulletPool.Instance.ReleaseBullet(this);
    }

    public override void Init(Vector2 direction, float speed = 3)
    {
        _direction = direction;
    }
}
