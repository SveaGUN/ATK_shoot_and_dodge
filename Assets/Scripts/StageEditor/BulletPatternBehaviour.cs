using UnityEngine;
using UnityEngine.Playables;

namespace AkaneTools.BulletHell.Timeline
{
    public class BulletPatternBehaviour : PlayableBehaviour
    {
        public Transform Target = null;

        public BulletType Type = BulletType.Enemy;
        public BulletFirePattern Pattern = BulletFirePattern.Simple;
        public int BulletCount = 0;
        public int DirectionCount = 0;
        public float AngleStep = 0;
        public float AngleOffset = 0;
        public float Speed = 0;
        public float FireInterval = 0;

        private bool _isAntiCrock = false;
        private bool _isFollow = false;//ターゲットに向けて発射するかどうか

        private double _nextFireTime = 0;
        private float _currentAngleOffset = 0;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { return; }
#endif
            //値の初期化
            _nextFireTime = 0;
            _currentAngleOffset = 0;

            //フラグ設定
            _isAntiCrock = AngleOffset < 0f;
            _isFollow = Target != null;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { return; }
#endif
            var currentTime = playable.GetTime();

            if(currentTime >= _nextFireTime)
            {
                _nextFireTime += FireInterval;

                _currentAngleOffset += AngleOffset;

                //360を超えたら0に戻す
                if (_currentAngleOffset >= 360f || _currentAngleOffset < -360f)
                {
                    _currentAngleOffset -= _isAntiCrock ? 360f : -360f;
                }

                Fire(playerData as Transform);
            }
        }

        private void Fire(Transform firePoint)
        {
            var param = new BulletPatternParam
            {
                BulletCount = this.BulletCount,
                DirectionCount = this.DirectionCount,
                AngleStep = this.AngleStep,
                AngleOffset = _currentAngleOffset,
                Speed = this.Speed,
            };

            BulletSpawner.Instance.TypeBullet = Type;

            if (_isFollow)
            {
                BulletSpawner.Instance.SpawnFollow(Pattern, firePoint, Target, param);
            }
            else
            {
                BulletSpawner.Instance.SpawnNormal(Pattern, firePoint, param);
            }

        }
    }
}