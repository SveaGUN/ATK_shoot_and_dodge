using UnityEngine;

public class BossBullet : BaseBullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //弾が当たった相手がIDamagableを持っている場合は、ダメージを与える
        if (collision.CompareTag("PlayerCollider"))
        {
            collision.GetComponentInParent<IDamagable>()?.TakeDamage(bulleAtk);
        }
        
        BBulletPool.Instance.ReleaseBullet(this);
        ResetDirection();
    }
}
