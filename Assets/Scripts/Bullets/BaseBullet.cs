using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    //攻撃力
    [SerializeField]
    protected int bulleAtk = 1;
    //移動速度
    [SerializeField]
    protected float bulletSpeed = 3f;

    private Vector3 dir = new Vector3(0, 0, 0);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //弾が当たった相手がIDamagableを持っている場合は、ダメージを与える
        if (collision.TryGetComponent<IDamagable>(out var d))
        {
            d.TakeDamage(bulleAtk);
        }
    }

    //弾の移動メソッド
    protected virtual void Move()
    {
        var p = transform.position;
        p += bulletSpeed * Time.deltaTime * dir;
        transform.position = p;
    }

    private void Update()
    {
        Move();
    }

    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
    }

    public void SetDirection()
    {
        dir = transform.rotation * new Vector3(1, 0, 0);
    }

    protected void ResetDirection()
    {
        dir = new Vector3(0, 0, 0);
    }
}
