using UnityEngine;
using UnityEngine.Playables;

namespace AkaneTools.BulletHell.Timeline
{
    public class BulletPatternBehaviour : PlayableBehaviour
    {
        public Transform FirePoint = null;
        public Transform Target = null;

        public BulletFirePattern Pattern = BulletFirePattern.Simple;
        public int BulletCount = 0;
        public int DirectionCount = 0;
        public float AngleStep = 0;
        public float AngleOffset = 0;
        public float Speed = 0;
        public float FireInterval = 0;
        public bool OnPlayFire = true;

        private bool _hasSpawned = false;
        private bool _isAntiCrock = false;
        private bool _isFollow = false;//ターゲットに向けて発射するかどうか

        private double _nextFireTime = 0;
        private float _currentAngleOffset = 0;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { return; }
#endif
            if (_hasSpawned) { return; }
            _hasSpawned = true;

            _nextFireTime = FireInterval;

            //フラグ設定
            _isAntiCrock = AngleOffset < 0f;
            _isFollow = Target != null;

            if (OnPlayFire) { Fire(); }

            Debug.Log("Play");
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {

        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var currentTime = playable.GetTime();

            if(currentTime >= _nextFireTime)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) { return; }
#endif

                _nextFireTime += FireInterval;

                _currentAngleOffset += AngleOffset;

                //360を超えたら0に戻す
                if (_currentAngleOffset >= 360f || _currentAngleOffset < -360f)
                {
                    _currentAngleOffset -= _isAntiCrock ? 360f : -360f;
                }

                Fire();
            }
        }

        private void Fire()
        {
            var param = new BulletPatternParam
            {
                BulletCount = this.BulletCount,
                DirectionCount = this.DirectionCount,
                AngleStep = this.AngleStep,
                AngleOffset = _currentAngleOffset,
                Speed = this.Speed,
            };

            if (_isFollow)
            {
                BulletSpawner.Instance.SpawnFollow(Pattern, FirePoint, Target, param);
            }
            else
            {
                BulletSpawner.Instance.SpawnNormal(Pattern, FirePoint, param);
            }

        }
    }
}