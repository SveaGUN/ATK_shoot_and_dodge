using UnityEngine;

namespace AkaneTools.BulletHell
{
    public class BulletSpawner : MonoBehaviour
    {
        public static BulletSpawner Instance { get; private set; } = null;

        private EnemyBulletPool _enemyPool = null;
        private BossBulletPool _bossPool = null;

        public BulletType TypeBullet { get; set; } = BulletType.Enemy;

        private void Start()
        {
            if (Instance == null) { Instance = this; }

            _enemyPool = EnemyBulletPool.Instance;
            _bossPool = BossBulletPool.Instance;
        }

        /// <summary>
        /// ’ÊíËŒ‚
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="firePoint"></param>
        /// <param name="param"></param>
        public void SpawnNormal(BulletFirePattern pattern, Transform firePoint, BulletPatternParam param)
        {
            switch (pattern)
            {
                case BulletFirePattern.Simple:
                    FireAtAngle(firePoint, param.BaseAngle, param.AngleOffset, param.Speed);
                    break;
                case BulletFirePattern.Circle:
                    FireCircle(firePoint, param.BulletCount, param.DirectionCount, param.BaseAngle, param.AngleStep, param.AngleOffset, param.Speed);
                    break;
                case BulletFirePattern.Spred:
                    FireSpread(firePoint, param.BulletCount, param.BaseAngle, param.AngleStep, param.AngleOffset, param.Speed);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ƒ^[ƒQƒbƒg‚ÉŒü‚¯‚Ä”­Ë‚·‚éƒpƒ^[ƒ“‚ğ¶¬
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="firePoint"></param>
        /// <param name="target"></param>
        /// <param name="param"></param>
        public void SpawnFollow(BulletFirePattern pattern, Transform firePoint, Transform target, BulletPatternParam param)
        {
            switch (pattern)
            {
                case BulletFirePattern.Simple:
                    FireToTarget(firePoint, target, param.Speed);
                    break;
                case BulletFirePattern.Circle:
                    FireCircleToTarget(firePoint, target, param.BulletCount, param.DirectionCount, param.AngleStep, param.Speed);
                    break;
                case BulletFirePattern.Spred:
                    FireSpreadToTarget(firePoint, target, param.BulletCount, param.AngleStep, param.AngleOffset, param.Speed);
                    break;
                default:
                    break;
            }
        }

        //===========ƒwƒ‹ƒp[===========
        private Vector2 TargetDirection(Transform from, Transform to)
        {
            var direction = (to.position - from.position).normalized;

            return new Vector2(direction.x, direction.y);
        }

        private Vector2 AngleToDirection(float angleDeg)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        private float DirectionToAngle(Vector2 direction) => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        private float TargetDirToAngle(Transform from, Transform to)
        {
            var direction = to.position - from.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        private void Fire(Transform firepoint, Vector2 direction, float speed)
        {
            switch (TypeBullet)
            {
                case BulletType.Enemy:
                    var e = _enemyPool.GetBullet();
                    e.transform.position = firepoint.position;
                    e.Init(direction, speed);
                    break;
                case BulletType.Boss:
                    var b = _bossPool.GetBullet();
                    b.transform.position = firepoint.position;
                    b.Init(direction, speed);
                    break;
                default:
                    var d = _enemyPool.GetBullet();
                    d.transform.position = firepoint.position;
                    d.Init(direction, speed);
                    break;
            }
        }
        //===========ƒwƒ‹ƒp[===========

        /// <summary>
        /// Šp“x‚ğ‚Â‚¯‚Ä’e‚ğ”­Ë‚·‚é
        /// </summary>
        /// <param name="angle">Šp“x(í‚É•Ï‰»‚·‚é’l)</param>
        /// <param name="angleOffset">”­Ë‚ÉA‚±‚ÌŠp“x•ª‚¾‚¯‚¸‚ç‚·(’è”)</param>
        /// <param name="speed">’e‘¬(ƒfƒtƒHƒ‹ƒg‚Í5f)</param>
        public void FireAtAngle(Transform firepoint, float angle, float angleOffset, float speed)
        {
            Fire(firepoint, AngleToDirection(angle + angleOffset), speed);
        }

        /// <summary>
        /// ƒvƒŒƒCƒ„[‚ÉŒü‚¯‚Ä’e‚ğ”­Ë‚·‚é
        /// </summary>
        /// <param name="speed">’e‘¬(ƒfƒtƒHƒ‹ƒg‚Í5f)</param>
        public void FireToTarget(Transform firepoint, Transform target, float speed)
        {
            Fire(firepoint, TargetDirection(firepoint, target), speed);
        }

        /// <summary>
        /// ‰~Œ`‚É’e‚ğ”­Ë‚·‚é
        /// </summary>
        /// <param name="bulletCount">ˆê•ûŒü‚Ì”­Ë’e”</param>
        /// <param name="directionCount">‰½•ûŒü‚É”ò‚Î‚·‚©</param>
        /// <param name="angleStep">’e‚ÌŠÔŠuŠp“x(bulletCount‚É‘Î‰)</param>
        /// <param name="angleOffset">‰½“x‚¸‚ç‚·‚©(directionCOunt‚É‘Î‰)</param>
        /// <param name="speed">’e‘¬(ƒfƒtƒHƒ‹ƒg‚Í5f)</param>
        public void FireCircle(Transform firepoint, int bulletCount, int directionCount, float baseAngle, float angleStep, float angleOffset, float speed)
        {
            var directionAngle = 360 / directionCount;
            var halfOffset = (bulletCount >> 1) * angleStep;

            for (int i = 0; i < bulletCount; i++)
            {
                //ˆê•ûŒü
                var radialAngle = angleStep * i- halfOffset;

                for (int j = 0; j < directionCount; j++)
                {
                    var fireDegree = baseAngle + directionAngle * j + radialAngle + angleOffset;
                    Fire(firepoint, AngleToDirection(fireDegree), speed);
                }
            }
        }

        /// <summary>
        /// ‰~Œ`‚É’e‚ğ”­Ë‚·‚é
        /// </summary>
        /// <param name="bulletCount">ˆê•ûŒü‚Ì”­Ë’e”</param>
        /// <param name="directionCount">‰½•ûŒü‚É”ò‚Î‚·‚©</param>
        /// <param name="angleStep">’e‚ÌŠÔŠuŠp“x(bulletCount‚É‘Î‰)</param>
        /// <param name="angleOffset">‰½“x‚¸‚ç‚·‚©(directionCOunt‚É‘Î‰)</param>
        /// <param name="speed">’e‘¬(ƒfƒtƒHƒ‹ƒg‚Í5f)</param>
        public void FireCircleToTarget(Transform firepoint, Transform target, int bulletCount, int directionCount, float angleStep, float speed)
        {
            var baseAngle = 360 / directionCount;
            var halfOffset = (bulletCount >> 1) * angleStep;

            for (int i = 0; i < bulletCount; i++)
            {
                //ˆê•ûŒü
                var radialAngle = angleStep * i + TargetDirToAngle(firepoint, target) - halfOffset;

                for (int j = 0; j < directionCount; j++)
                {
                    var fireDegree = baseAngle * j + radialAngle;
                    Fire(firepoint, AngleToDirection(fireDegree), speed);
                }
            }
        }

        /// <summary>
        /// ŠgU‚·‚é’e‚ğ”­Ë‚·‚é
        /// </summary>
        /// <param name="bulletCount">ˆê•ûŒü‚Ì”­Ë’e”</param>
        /// <param name="angleStep">’e‚ÌŠÔŠuŠp“x</param>
        /// <param name="speed">’e‘¬(ƒfƒtƒHƒ‹ƒg‚Í5f)</param>
        public void FireSpread(Transform firepoint, int bulletCount, float baseAngle, float angleStep, float angleOffset, float speed)
        {
            var half = bulletCount >> 1;
            var startAngle = baseAngle + angleOffset - angleStep * half;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + angleStep * i;
                Fire(firepoint, AngleToDirection(angle), speed);
            }
        }

        /// <summary>
        /// ƒvƒŒƒCƒ„[‚ÉŒü‚¯‚ÄŠgU‚·‚é’e‚ğ”­Ë‚·‚é
        /// </summary>
        /// <param name="bulletCount">ˆê•ûŒü‚Ì”­Ë’e”</param>
        /// <param name="angleStep">’e‚ÌŠÔŠuŠp“x</param>
        /// <param name="speed">’e‘¬(ƒfƒtƒHƒ‹ƒg‚Í5f)</param>
        public void FireSpreadToTarget(Transform firepoint, Transform target, int bulletCount, float angleStep, float angleOffset, float speed)
        {
            var centerAngle = DirectionToAngle(TargetDirection(firepoint, target));
            var half = bulletCount >> 1;
            var startAngle = centerAngle - angleStep * half;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + angleStep * i;
                Fire(firepoint, AngleToDirection(angle), speed);
            }
        }
    }

}
