using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TransformTweenClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<Transform> StartLocation;
    public ExposedReference<Transform> EndLocation;

    public AnimationCurve EasingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TransformTweenBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.StartLocation = StartLocation.Resolve(graph.GetResolver());
        behaviour.EndLocation = EndLocation.Resolve(graph.GetResolver());
        behaviour.EasingCurve = EasingCurve;

        return playable;
    }
}
