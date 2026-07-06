using System;
using UnityEngine;

public class FireSystem
{
    private readonly Transform _target = null;
    private readonly Transform _firePoint = null;
    private readonly Transform _self = null;

    public FireSystem(Transform self, Transform target, Transform firePoint)
    {
        _self = self;
        _target = target;
        _firePoint = firePoint;
    }

    /// <summary>
    /// 円形に弾を発射する
    /// </summary>
    /// <param name="bulletCount">一方向の発射弾数</param>
    /// <param name="directionCount">何方向に飛ばすか</param>
    /// <param name="angleStep">弾の間隔角度(bulletCountに対応)</param>
    /// <param name="agnleOffset">何度ずらすか(directionCOuntに対応)</param>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireBurstCircle(int bulletCount, int directionCount, float angleStep = 5f, float agnleOffset = 0f, float speed = 5f)
    {
        var r = 360 / directionCount;

        for (int i = 0; i < bulletCount; i++)
        {
            for (int j = 0; j < directionCount; j++)
            {
                var bullet = BBulletPool.Instance.GetBullet();
                bullet.transform.position = _firePoint.position;
                //r * jは方向を決めるための角度 
                bullet.transform.rotation = Quaternion.Euler(0, 0, (r * j) + (angleStep * i) + agnleOffset);
                bullet.SetDirection();
                bullet.SetSpeed(speed);
            }
        }
    }

    /// <summary>
    /// 回転しながら弾を発射する
    /// </summary>
    /// <param name="angle">角度(常に変化する値)</param>
    /// <param name="angleOffset">発射時に、この角度分だけずらす(定数)</param>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireAtAngle(float angle, float angleOffset = 0, float speed = 5f)
    {
        var bullet = BBulletPool.Instance.GetBullet();
        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
        bullet.SetDirection();
        bullet.SetSpeed(speed);
    }

    /// <summary>
    /// プレイヤーに向けて弾を発射する
    /// </summary>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireToTarget(float speed = 5f)
    {
        var bullet = BBulletPool.Instance.GetBullet();
        bullet.transform.position = _firePoint.position;

        var r = GetAngleToTarget();

        bullet.transform.rotation = Quaternion.Euler(0, 0, r);
        bullet.SetDirection();
        bullet.SetSpeed(speed);
    }

    /// <summary>
    /// プレイヤーに向けて拡散する弾を発射する
    /// </summary>
    /// <param name="bulletCount">一方向の発射弾数</param>
    /// <param name="angleStep">弾の間隔角度</param>
    /// <param name="speed">弾速(デフォルトは5f)</param>
    public void FireSpreadToTarget(int bulletCount = 5, float angleStep = 5f, float speed = 5f)
    {
        var half = bulletCount >> 1;//半分にする

        var r = GetAngleToTarget();

        //for文で端から端へ弾を生成するため、弾数の半分だけ回転させる
        r -= angleStep * half;

        for (int i = 0; i < bulletCount; i++)
        {
            var bullet = BBulletPool.Instance.GetBullet();
            bullet.transform.position = _firePoint.position;
            //角度をずらす
            bullet.transform.rotation = Quaternion.Euler(0, 0, r + (angleStep * i));
            bullet.SetDirection();
            bullet.SetSpeed(speed);
        }
    }

    private float GetAngleToTarget()
    {
        //ターゲットへの方向ベクトル
        var tp = _target.transform.position - _self.position;
        //ターゲットに角度を合わせる
        var result = Mathf.Atan2(tp.y, tp.x) * Mathf.Rad2Deg;

        return result;
    }
}
