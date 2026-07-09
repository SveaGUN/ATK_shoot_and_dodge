using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TransformTweenClip : PlayableAsset, ITimelineClipAsset
{
    public TransformTweenBehaviour template = new TransformTweenBehaviour();
    public ExposedReference<Transform> startLocation;
    public ExposedReference<Transform> endLocation;

    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<TransformTweenBehaviour>.Create(graph);
    }
}
