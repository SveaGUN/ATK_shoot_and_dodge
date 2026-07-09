using UnityEngine;
using UnityEngine.Playables;

namespace AkaneTools.BulletHell.Timeline
{
    //クリップ
    public class BulletPatternClip : PlayableAsset
    {
        [SerializeField]
        private ExposedReference<Transform> Target;//アタッチしなくてもよい
        [SerializeField]
        private BulletType TypeBullet = BulletType.Enemy;
        [SerializeField]
        private BulletFirePattern Pattern = BulletFirePattern.Simple;
        [SerializeField, Range(1, 30)]
        private int BulletCount = 1;
        [SerializeField, Range(1, 36)]
        private int DirectionCount = 1;
        [SerializeField]
        private float AngleStep = 5;
        [SerializeField, Range(-359f, 359f)]
        private float AngleOffset = 0;
        [SerializeField, Range(0.01f, 20f)]
        private float Speed = 5f;
        [SerializeField, Range(0.01f, 10f)]
        private float FireInterval = 1;

        //todo 発射回数とn秒間発射するの使い分けができるようにする

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<BulletPatternBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.Target = Target.Resolve(graph.GetResolver());
            behaviour.Type = TypeBullet;
            behaviour.Pattern = Pattern;
            behaviour.BulletCount = BulletCount;
            behaviour.DirectionCount = DirectionCount;
            behaviour.AngleStep = AngleStep;
            behaviour.AngleOffset = AngleOffset;
            behaviour.Speed = Speed;
            behaviour.FireInterval = FireInterval;
            
            return playable;
        }
    }
}