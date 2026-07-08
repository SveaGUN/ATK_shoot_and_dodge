using UnityEngine;
using UnityEngine.Playables;

namespace AkaneTools.BulletHell.Timeline
{
    //クリップ
    public class BulletPatternClip : PlayableAsset
    {
        [SerializeField]
        private string PatternName = string.Empty;
        [SerializeField, Range(1, 30)]
        private int BulletCount = 1;
        [SerializeField, Range(1, 36)]
        private int DirectionCount = 1;
        [SerializeField]
        private float AngleStep = 5;
        [SerializeField]
        private float AngleOffset = 0;
        [SerializeField, Range(0.01f, 20f)]
        private float Speed = 0;
        [SerializeField, Range(0.01f, 10f)]
        private float FireInterval = 1;
        [SerializeField]
        private bool OnPlayFire = true;

        //todo 発射回数とn秒間発射するの使い分けができるようにする

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<BulletPatternBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.PatternName = PatternName;
            behaviour.BulletCount = BulletCount;
            behaviour.DirectionCount = DirectionCount;
            behaviour.AngleStep = AngleStep;
            behaviour.AngleOffset = AngleOffset;
            behaviour.Speed = Speed;
            behaviour.FireInterval = FireInterval;
            behaviour.OnPlayFire = OnPlayFire;

            return playable;
        }
    }
}