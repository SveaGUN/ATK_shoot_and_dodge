using System;
using UnityEngine;

public class FireSystem<T> where T : BaseBullet
{
    private readonly Transform _target = null;
    private readonly Transform _firePoint = null;
    private readonly Transform _self = null;
    private readonly IBulletPool<T> _pool = null;

    public FireSystem(Transform self, Transform target, Transform firePoint, IBulletPool<T> pool)
    {
        _self = self;
        _target = target;
        _firePoint = firePoint;
        _pool = pool;
    }

    //===========ヘルパー===========
    private Vector2 GetDirectionToTarget()
    {
        //ターゲットへの方向ベクトル
        var tp = _target.position - _self.position;

        return (Vector2)tp;
    }

    private Vector2 AngleToDirection(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    private float DirectionToAngle(Vector2 direction) => Mathf.Atan2(direction.y, direction.x) * Mathf.Deg2Rad;

    private void Fire(Vector2 direction, float speed)
    {
        var bullet = _pool.GetBullet();
        bullet.transform.position = _firePoint.position;
        //bullet.SetDirection(direction); // ← Bullet側もVector2受け取りに変更する想定
        bullet.SetSpeed(speed);
    }
    //===========ヘルパー===========

    /// <summary>
    /// 角度をつけて弾を発射する
    /// </summary>
    /// <param name="angle">角度(常に変化する値)</param>
    /// <param name="angleOffset">発射時に、この角度分だけずらす(定数)</param>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireAtAngle(float angle, float angleOffset = 0, float speed = 5f) => Fire(AngleToDirection(angle + angleOffset), speed);

    /// <summary>
    /// プレイヤーに向けて弾を発射する
    /// </summary>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireToTarget(float speed = 5f) => Fire(GetDirectionToTarget(), speed);

    /// <summary>
    /// 円形に弾を発射する
    /// </summary>
    /// <param name="bulletCount">一方向の発射弾数</param>
    /// <param name="directionCount">何方向に飛ばすか</param>
    /// <param name="angleStep">弾の間隔角度(bulletCountに対応)</param>
    /// <param name="angleOffset">何度ずらすか(directionCOuntに対応)</param>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireBurstCircle(int bulletCount, int directionCount, float angleStep = 5f, float angleOffset = 0f, float speed = 5f)
    {
        var baseAngle = 360 / directionCount;

        for (int i = 0; i < bulletCount; i++)
        {
            //一方向
            var radialAngle = angleStep * i + angleOffset;

            for (int j = 0; j < directionCount; j++)
            {
                var fireDegree = baseAngle * j + radialAngle;
                Fire(AngleToDirection(fireDegree), speed);
            }
        }
    }

    /// <summary>
    /// プレイヤーに向けて拡散する弾を発射する
    /// </summary>
    /// <param name="bulletCount">一方向の発射弾数</param>
    /// <param name="angleStep">弾の間隔角度</param>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireSpreadToTarget(int bulletCount = 5, float angleStep = 5f, float speed = 5f)
    {
        var centerAngle = DirectionToAngle(GetDirectionToTarget());
        var half = bulletCount >> 1;
        var startAngle = centerAngle - angleStep * half;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Fire(AngleToDirection(angle), speed);
        }
    }
}
