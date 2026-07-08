using UnityEngine;

public class EnemyBullet : BaseBullet
{
    public override void Init(Vector2 direction, float speed)
    {
        base.Init(direction, speed);
        Rotate();
    }

    public override void Init(Vector2 direction)
    {
        base.Init(direction);
        Rotate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //弾が当たった相手がIDamagableを持っている場合は、ダメージを与える
        if (collision.CompareTag("PlayerCollider"))
        {
            collision.GetComponentInParent<IDamagable>()?.TakeDamage(_bulleAtk);
        }

        ResetDirection();
        EnemyBulletPool.Instance.ReleaseBullet(this);
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg, Vector3.forward);
    }
}
