using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    //攻撃力
    [SerializeField]
    protected int _bulleAtk = 1;
    //移動速度
    [SerializeField]
    protected float _bulletSpeed = 3f;

    protected Vector3 _direction = new Vector3(0, 0, 0);

    private void Update()
    {
        Move();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //弾が当たった相手がIDamagableを持っている場合は、ダメージを与える
        if (collision.TryGetComponent<IDamagable>(out var d))
        {
            d.TakeDamage(_bulleAtk);
        }
    }

    public virtual void Init(Vector2 direction, float speed)
    {
        _bulletSpeed = speed;
        _direction = direction;
    }

    public virtual void Init(Vector2 direction) => _direction = direction;

    //弾の移動メソッド
    protected virtual void Move()
    {
        var p = transform.position;
        p += _bulletSpeed * Time.deltaTime * _direction;
        transform.position = p;
    }

    protected void ResetDirection() => _direction = new Vector3(0, 0, 0);
}
